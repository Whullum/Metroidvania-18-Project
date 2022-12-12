using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public bool EnableInput { get; set; } = true;
    public bool IsFacingRight { get { return !_spriteRenderer.flipY; } }
    public float CurrentMagazine { get { return _currentMagazineSize; } }
    public GunSetting ActiveGunSetting { get { return _activeSetting; } }
    public GunSetting[] UnlockedGunSettings { get { return _gunSettings.ToArray(); } }

    private int _gunSettingIndex = 0; // Index of the current active setting.
    private bool _isReloading;
    private float _nextFire; // Time until the creation of a new bullet.
    private float _currentMagazineSize;
    private Vector3 _mousePos;
    private GunSetting _activeSetting;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _playerRBody;

    [Tooltip("The point where the bullets will be spawned.")]
    [SerializeField] private Transform _shootPoint;
    [Tooltip("List containing all the unlocked Gun Settings. Is mandatory that minimum one Gun Setting is set.")]
    [SerializeField] private List<GunSetting> _gunSettings = new List<GunSetting>();
    [SerializeField] private bool _showDebugInfo;
    [Tooltip("Player Audio script for sound effects playback")]
    [SerializeField] private PlayerAudio _playerAudio;
    [SerializeField] private ParticleSystem _shootParticles;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerRBody = GetComponentInParent<Rigidbody2D>();

        InitializeGun();
    }

    private void Update()
    {
        _nextFire -= Time.deltaTime * _activeSetting.FireRate;

        if (!EnableInput) return;

        AimAtCursor();

        if (Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(1))
            SwitchSetting();

        if (Input.GetMouseButton(0))
            Shoot();

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }
    }

    /// <summary>
    /// Makes the gun rotate towards the mouse position. It also flips the sprite depending on the gun rotation.
    /// </summary>
    private void AimAtCursor()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = _mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        if (rotZ < 89 && rotZ > -89)
            _spriteRenderer.flipY = false;
        else
            _spriteRenderer.flipY = true;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    /// <summary>
    /// Changues the current active Gun Setting for the next one in the list and starts reloading the gun.
    /// </summary>
    private void SwitchSetting()
    {
        _gunSettingIndex++;

        if (_gunSettingIndex >= _gunSettings.Count)
            _gunSettingIndex = 0;

        _activeSetting = _gunSettings[_gunSettingIndex];

        _currentMagazineSize = 0;
        StartCoroutine(Reload());

        _playerAudio.SetWwiseSwitch(_activeSetting.WwiseSwitch);

        PlayerUI.Instance.UpdateUIValues();
    }

    /// <summary>
    /// Shoots a bullet depending on the fire rate and if the Shotgun Mode is enabled.
    /// </summary>
    private void Shoot()
    {
        if (_isReloading) return;

        if (_nextFire <= 0)
        {
            // If the magazine is empty, reload the gun and prevent shooting.
            if (_currentMagazineSize <= 0)
            {
                StartCoroutine(Reload());
                return;
            }

            // Shotgun Mode shooting
            if (_activeSetting.ShotgunMode)
                StartCoroutine(ShotgunFireMode());
            // Normal fire mode.
            else
            {
                SpawnBullet();
                CameraEvents.CameraShake?.Invoke(_activeSetting.CameraShakeDuration, _activeSetting.CameraShakeForce);
            }
            AddRecoil();

            // Play gunshot sound effect
            _playerAudio.PostWwiseEvent(_playerAudio._sfxPlayerGunShot);
        }
    }

    /// <summary>
    /// Prevents the gun from shooting before the reload finishes and reloads the gun.
    /// </summary>
    /// <returns>The reload time.</returns>
    private IEnumerator Reload()
    {
        // Play reload sound effect
        _playerAudio.PostWwiseEvent(_playerAudio._sfxPlayerReload);
        _isReloading = true;

        yield return new WaitForSeconds(_activeSetting.ReloadTime);

        _currentMagazineSize = _activeSetting.MagazineSize;
        _isReloading = false;
        PlayerUI.Instance.UpdateUIValues();
    }

    /// <summary>
    /// Creates a new bullet an set a radom rotation depending on the active setting Bullet Spread.
    /// </summary>
    private void SpawnBullet()
    {
        // Generate random rotation.
        float spread = Random.Range(-_activeSetting.BulletSpread, _activeSetting.BulletSpread);

        BulletController newBullet = Instantiate(_activeSetting.BulletPrefab, _shootPoint.position, _shootPoint.rotation).GetComponent<BulletController>();

        newBullet.GetComponent<SpriteRenderer>().color = _activeSetting.Color;
        newBullet.BulletDamage = _activeSetting.Damage; // Set the bullet damage.
        newBullet.GunSetting = _activeSetting.ID;
        newBullet.transform.Rotate(0f, 0f, spread); // Give the bullet the random generated rotation.
        newBullet.LaunchBullet(newBullet.transform.right * _activeSetting.BulletSpeed); // Launch the bullet with the Gun Setting speed.

        // Destroy the bullet if reaches the maximum range.
        Destroy(newBullet.gameObject, _activeSetting.Range / _activeSetting.BulletSpeed);

        _currentMagazineSize -= _activeSetting.BulletCost;
        _nextFire = 1;

        if (_activeSetting.ID != GunSettingID.STREAM_FIRE)
            _shootParticles.Play();

        PlayerUI.Instance.UpdateUIValues();
    }

    /// <summary>
    /// Shoots all the bullets inside the magazine at the same time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShotgunFireMode()
    {
        CameraEvents.CameraShake?.Invoke(_activeSetting.CameraShakeDuration, _activeSetting.CameraShakeForce);

        while (_currentMagazineSize > 0)
        {
            if (_nextFire <= 0)
                SpawnBullet();

            _nextFire -= Time.deltaTime * _activeSetting.FireRate;

            yield return null;
        }
        StartCoroutine(Reload());
    }

    /// <summary>
    /// Pushes the player rigidbody in opossite the direction the gun is shooting.
    /// </summary>
    private void AddRecoil()
    {
        Vector3 recoilDirection = (transform.position - _mousePos).normalized * 10;

        _playerRBody.AddForce(recoilDirection * _activeSetting.Recoil, ForceMode2D.Force);
    }

    /// <summary>
    /// Add a new Gun Setting to the Gun Setting list.
    /// </summary>
    /// <param name="upgrade">The new Gun Setting to add.</param>
    public void UpgradeGun(GunSetting upgrade)
    {
        if (_gunSettings.Contains(upgrade))
        {
            Debug.LogWarning("Gun WARNING : Gun already has " + upgrade);
            return;
        }

        _gunSettings.Add(upgrade);
    }

    public void LoadGunSettings(string[] unlockedGunSettingsIDs)
    {
        GunSetting[] gunSettings = Resources.LoadAll<GunSetting>("Gun");

        if (gunSettings.Length <= 0) { Debug.LogError("GUN ERROR : There are not any Gun Settings to load."); return; }

        for (int i = 0; i < unlockedGunSettingsIDs.Length; i++)
        {
            GunSettingID unlockedSetting = (GunSettingID)System.Enum.Parse(typeof(GunSettingID), unlockedGunSettingsIDs[i]);

            for (int j = 0; j < gunSettings.Length; j++)
            {
                bool canUpgrade = true;

                foreach (GunSetting upgrade in _gunSettings)
                {
                    if (upgrade.ID.Equals(unlockedSetting))
                    {
                        canUpgrade = false;
                        continue;
                    }
                }

                if (canUpgrade && gunSettings[j].ID.Equals(unlockedSetting))
                {
                    _gunSettings.Add(gunSettings[j]);
                }
            }
        }
    }

    private void InitializeGun()
    {
        if (_gunSettings.Count <= 0)
        {
            Debug.LogError("Gun ERROR : at leats one Gun Setting must be assigned.");

            return;
        }

        // Set the current active setting to the first one in the list.
        _activeSetting = _gunSettings[0];
        // Set the magazine to the current active setting capacity.
        _currentMagazineSize = _activeSetting.MagazineSize;
    }

    private void OnGUI()
    {
        if (!_showDebugInfo) return;

        GUI.backgroundColor = Color.black;
        GUILayout.BeginArea(new Rect(10, 10, 200, 600));
        GUILayout.Label("Active Gun Setting : " + _activeSetting.ID);
        GUILayout.Label("Current Magazine Size : " + _currentMagazineSize);
        GUILayout.Label("Time until next fire : " + _nextFire);
        GUILayout.Label("Reloading : " + _isReloading);
        GUILayout.EndArea();
    }
}
