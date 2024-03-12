using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Jump data", menuName = "Custom datas/Jump Data")]
public class JumpData : ScriptableObject
{
    [Min(0.1f)] public float jumpHeight;
    [Tooltip("The factor by which gravity is dampened during the upward phase of the jump."),
     Range(0.1f, 1f)] public float gravityDampOnRise;
    [Tooltip("The factor by which the player's upward velocity is cut off when the jump button is released."),
     Min(1f)] public float jumpCutOffFactor;
}
