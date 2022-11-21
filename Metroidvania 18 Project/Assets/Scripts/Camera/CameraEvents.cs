using System;
using UnityEngine;

public class CameraEvents : MonoBehaviour
{
    /// <summary>
    /// Makes the camera shake for an ammount of time with a specific force. First param is duration, second is force.
    /// </summary>
    public static Action<float, float> CameraShake;
}
