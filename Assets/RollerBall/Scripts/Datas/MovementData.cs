using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "Movement data", menuName = "Custom datas/Movement Data")]
public class MovementData : ScriptableObject
{
    [Tooltip("Time in seconds for the object to accelerate from zero to max speed."),
    CustomInspector.ShowIfNot(nameof(manualMode), style = CustomInspector.DisabledStyle.GreyedOut),
    Min(.0001f)] public float accelerationTime = 1f;
    [Tooltip("The maximum movement speed that the object can reach.")]
    public float maxSpeed = 6f;
    [Tooltip("Time in seconds for the object to decelerate from zero to max speed."),
    CustomInspector.ShowIfNot(nameof(manualMode), style = CustomInspector.DisabledStyle.GreyedOut),
    Min(.0001f)] public float decelerationTime = 3f;
    [Tooltip("A multiplier that provides a bonus to the object's acceleration when attempting direction changes.")]
    [Min(1f)] public float turningFactor = 1.5f;
    [Space]
    [Space]
    [Tooltip("When enabled, allows direct setting of 'Acceleration' and 'Deceleration', bypassing the values in 'Acceleration Time' and 'Deceleration Time'.")]
    public bool manualMode = false;    
    [CustomInspector.ShowIf(nameof(manualMode), style = CustomInspector.DisabledStyle.GreyedOut),
    Min(.0001f)] public float acceleration = 6f, deceleration = 2f; // Mettiamo Min a zero? Poi lo gestisce la funzione?

    void OnValidate()
    {
        Calculate();
    }

    public void Calculate()
    {
        if (!manualMode)
        {
            acceleration = maxSpeed / accelerationTime;
            deceleration = maxSpeed / decelerationTime;
        }        
    }
}
