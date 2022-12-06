using UnityEngine;

[RequireComponent(typeof(DamageableEntity))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    public DamageableEntity Health { get { return _damageable; } }

    private DamageableEntity _damageable;
    private PlayerMovement _movement;
    private Gun _gun;

    [Tooltip("Ammount of time the camera will shake when the player gets hit.")]
    [SerializeField] private float _cameraShakeHitDuration;
    [Tooltip("Ammount of force applied to the camera shake when the player gets hit.")]
    [SerializeField] private float _cameraShakeHitForce;

    private void Awake()
    {
        _damageable = GetComponent<DamageableEntity>();
        _movement = GetComponent<PlayerMovement>();
        _gun = GetComponentInChildren<Gun>();

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

    public void SetInput(bool toggle)
    {
        _movement.SetMovement(toggle);
        _gun.EnableInput = toggle;
    }
}
