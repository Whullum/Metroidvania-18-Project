using UnityEngine;

[CreateAssetMenu(fileName = "New Door Connection", menuName = "Door/Door Connection")]
public class DoorConnection : ScriptableObject
{
    [Tooltip("Time in seconds to transition to another scene.")]
    public float TransitionTime;
    [Tooltip("Color of the transition.")]
    public Color TransitionColor;
}
