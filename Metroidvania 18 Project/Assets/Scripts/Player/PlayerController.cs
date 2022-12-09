using UnityEngine;

[RequireComponent(typeof(DamageableEntity))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    public DamageableEntity Health { get { return _damageable; } }
    public Gun Gun { get { return _gun; } }

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

    private void Start()
    {
        if(!GameManager.Instance.IsNewGame)
            LoadPlayerData();

        PlayerUI.Instance.UpdateUIValues();
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
        PlayerUI.Instance.UpdateUIValues();

        if (_damageable.CurrentHealth <= 0)
            GameManager.Instance.RespawnPlayer();
    }

    public void SetInput(bool toggle)
    {
        _movement.SetMovement(toggle);
        _gun.EnableInput = toggle;
    }

    public PlayerData GetPlayerData()
    {
        string[] unlockedGunSettings = new string[_gun.UnlockedGunSettings.Length];

        for(int i = 0; i < _gun.UnlockedGunSettings.Length; i++)
            unlockedGunSettings[i] = _gun.UnlockedGunSettings[i].ID.ToString();

        return new PlayerData(_damageable.MaxHealth, _damageable.CurrentHealth, unlockedGunSettings, _movement.CanDoubleJump);
    }

    public void UnlockDoubleJump()
    {
        _movement.CanDoubleJump = true;
    }

    private void LoadPlayerData()
    {
        PlayerData player = SaveSystem.GameData.PlayerData;

        _damageable.MaxHealth = player.MaxHealth;
        _damageable.CurrentHealth = player.CurrentHealth;
        _movement.CanDoubleJump = player.DoubleJump;

        _gun.LoadGunSettings(player.UnlockedGunSettings);
    }
}
