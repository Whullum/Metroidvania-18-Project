using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private int _gunSettingIndex = 0;
    private bool _isReloading;
    private float _nextFire;
    private float _currentMagazineSize;
    private GunSetting _activeSetting;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Transform _shootPoint;
    [SerializeField] private List<GunSetting> _gunSettings = new List<GunSetting>();
    [SerializeField] private bool _showDebugInfo;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _activeSetting = _gunSettings[0];
        _currentMagazineSize = _activeSetting.MagazineSize;
    }

    private void Update()
    {
        AimAtCursor();
        SwitchSetting();
        Shoot();
    }

    private void AimAtCursor()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePosition - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        if (rotZ < 89 && rotZ > -89)
            _spriteRenderer.flipY = true;
        else
            _spriteRenderer.flipY = false;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    private void SwitchSetting()
    {
        if(Input.GetMouseButtonDown(2))
        {
            _gunSettingIndex++;

            if (_gunSettingIndex >= _gunSettings.Count)
                _gunSettingIndex = 0;

            _activeSetting = _gunSettings[_gunSettingIndex];

            _currentMagazineSize = 0;
            StartCoroutine(Reload());
        }
    }

    private void Shoot()
    {
        _nextFire -= Time.deltaTime * _activeSetting.FireRate;

        if (!Input.GetMouseButton(0)) { return; }

        if (_isReloading) return;

        if(_nextFire <= 0)
        {
            if (_currentMagazineSize <= 0)
            {
                StartCoroutine(Reload());
                return;
            }
            if (_activeSetting.ShotgunMode)
                StartCoroutine(ShotgunFireMode());
            else
                SpawnBullet();
        }
    }

    private IEnumerator Reload()
    {
        _isReloading = true;

        yield return new WaitForSeconds(_activeSetting.ReloadTime);
        
        _currentMagazineSize = _activeSetting.MagazineSize;
        _isReloading = false;
    }

    private void SpawnBullet()
    {
        float spread = Random.Range(-_activeSetting.BulletSpread, _activeSetting.BulletSpread);

        BulletController newBullet = Instantiate(_activeSetting.BulletPrefab, _shootPoint.position, _shootPoint.rotation).GetComponent<BulletController>();

        newBullet.BulletDamage = _activeSetting.Damage;
        newBullet.transform.Rotate(0f, 0f, spread);
        newBullet.GetComponent<Rigidbody2D>().AddForce(newBullet.transform.right * _activeSetting.BulletSpeed, ForceMode2D.Impulse);

        Destroy(newBullet.gameObject, _activeSetting.Range / _activeSetting.BulletSpeed);

        _currentMagazineSize -= _activeSetting.BulletCost;
        _nextFire = 1;
    }

    private IEnumerator ShotgunFireMode()
    {
        while(_currentMagazineSize > 0)
        {
            if(_nextFire <= 0)
                SpawnBullet();

            _nextFire -= Time.deltaTime * _activeSetting.FireRate;

            yield return null;
        }
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
