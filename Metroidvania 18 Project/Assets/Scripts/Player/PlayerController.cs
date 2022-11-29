using UnityEngine;

[RequireComponent(typeof(DamageableEntity))]
public class PlayerController : MonoBehaviour
{
    private DamageableEntity _damageable;

    [Tooltip("Ammount of time the camera will shake when the player gets hit.")]
    [SerializeField] private float _cameraShakeHitDuration;
    [Tooltip("Ammount of force applied to the camera shake when the player gets hit.")]
    [SerializeField] private float _cameraShakeHitForce;

    private void Awake()
    {
        // Get the Damageable component.
        _damageable = GetComponent<DamageableEntity>();

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        //Subscribe the GetHit method to the Damageable Hit Event.
        _damageable.DamageReceived += GetHit;
    }

    private void OnDisable()
    {
        //Unsubscribe the GetHit method to the Damageable Hit Event.
        _damageable.DamageReceived -= GetHit;
    }

    /// <summary>
    /// If the player gets hit, we tell the camera to shake.
    /// </summary>
    private void GetHit()
    {
        CameraEvents.CameraShake(_cameraShakeHitDuration, _cameraShakeHitForce);
    }
}
