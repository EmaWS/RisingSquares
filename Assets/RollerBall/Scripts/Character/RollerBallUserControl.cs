using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class RollerBallUserControl : MonoBehaviour
{
    private RollerBall ball;
    
    private Transform cameraTransform; // A reference to the main camera in the scenes transform
    private Vector3 camForward, camRight; // The current forward direction of the camera
    private Vector3 moveInputDirection, lookInputDirection;

    private Vector3 desiredMovementDirection;
    private Vector3 lookDirection;

    private Vector3 previousMovementDirection;

    public enum GameMode { _3D , _2D }
    [SerializeField] private GameMode gameMode;


    private void Awake()
    {
        ball = GetComponentInParent<RollerBall>();
        // get the transform of the main camera
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Ball needs a Camera tagged \"MainCamera\", for camera-relative controls.");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInputDirection = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInputDirection = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                ball.TryJump();               
                break;
            case InputActionPhase.Canceled:
                switch (ball.state)
                {
                    case PlayerState.Grounded:
                        //  
                        break;
                    case PlayerState.Airborne:
                        ball.EvaluateJumpCutOff();
                        break;
                    default:
                        break;
                }
                break;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                switch (ball.state)
                {
                    case PlayerState.Grounded:
                        break;
                    case PlayerState.Airborne:
                        switch (gameMode)
                        {
                            case GameMode._3D:
                                StartCoroutine(ball.TryDash(desiredMovementDirection, camForward));
                                break;
                            case GameMode._2D:
                                StartCoroutine(ball.TryDash(desiredMovementDirection, previousMovementDirection));
                                break;
                        }
                        break;
                }                
                break;
            case InputActionPhase.Canceled:
                break;
        }
    }


    private void Update()
    {
        camForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        camRight = cameraTransform.right;
        switch (gameMode)
        {
            case GameMode._3D:
                desiredMovementDirection = (moveInputDirection.y * camForward + moveInputDirection.x * camRight).normalized;
                break;
            case GameMode._2D:
                desiredMovementDirection = (moveInputDirection.x * camRight).normalized;
                if (desiredMovementDirection != Vector3.zero) previousMovementDirection = desiredMovementDirection.normalized;
                break;
        }

        ball.DrawDebugRays();
        Debug.DrawRay(ball.transform.position, desiredMovementDirection, Color.gray);
    }

    private void FixedUpdate()
    {
        
        switch (ball.state)
        {
            case PlayerState.Grounded:
                OnFixedUpdateGrounded();
                break;
            case PlayerState.Airborne:
                OnFixedUpdareAirborne();
                break;
            default:
                break;
        }
    }

    void OnFixedUpdateGrounded()
    {
        ball.GroundedMove(desiredMovementDirection);
    }

    void OnFixedUpdareAirborne()
    {
        ball.AirborneMove(desiredMovementDirection);
    }

    public void SwitchGameMode()
    {
        switch (gameMode)
        {
            case GameMode._3D:
                gameMode = GameMode._2D;
                break;
            case GameMode._2D:
                gameMode = GameMode._3D;
                break;
            default:
                break;
        }
    }    
}
