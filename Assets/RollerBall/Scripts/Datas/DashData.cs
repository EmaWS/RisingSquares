using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Dash data", menuName = "Custom datas/Dash Data")]
public class DashData : ScriptableObject
{
    [Min(0.1f)] public float dashSpeed;
    [Min(0.1f)] public float dashDuration;
    public bool canChangeDirection;
    [Space]
    [Tooltip("On dash start, freeze the player for this number of frames"),
    Min(0)] public int dashFreezeFrames;
}
