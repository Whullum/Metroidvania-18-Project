using System.Collections;
using UnityEngine;

public class TurretEnemy : Enemy
{
    private float _currentMagazineSize;
    private float _nextFire;
    private bool _isReloading;
    private bool _isAttacking;
    private Transform _player;

    [Header("Turret enemy properties")]
    [SerializeField] private float _checkTime;
    [SerializeField] private EnemyGunSetting _enemyGunSetting;
    [SerializeField] private Transform _gun;
    [SerializeField] private Transform _shootPoint;

    protected override void Start()
    {
        base.Start();

        _currentMagazineSize = _enemyGunSetting.MagazineSize;

        InvokeRepeating("SearchPlayer", 0f, _checkTime);
    }

    protected override void Update()
    {
        base.Update();

        _nextFire -= Time.deltaTime * _enemyGunSetting.FireRate;

        if (!_isAttacking) return;

        RotateTowardsPlayer();
        ShootPlayer();
    }

    private void SearchPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, _enemyGunSetting.Range, LayerMask.GetMask("Player"));

        if (player != null)
        {
            _isAttacking = true;

            _player = player.transform;
        }
        else
        {
            _isAttacking = false;

            _player = null;
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 rotation = _player.position - transform.position;
        float angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        _gun.rotation = Quaternion.Lerp(_gun.rotation, lookRotation, Time.deltaTime * _enemyGunSetting.RotationSpeed);
    }

    private void ShootPlayer()
    {
        if (_isReloading) return;

        if (_nextFire <= 0)
        {
            if (_currentMagazineSize <= 0)
                StartCoroutine(Reload());
            else
                CreateBullet();
        }
    }

    private void CreateBullet()
    {
        float rndSpread = Random.Range(-_enemyGunSetting.BulletSpread, _enemyGunSetting.BulletSpread);

        BulletController newBullet = Instantiate(_enemyGunSetting.BulletPrefab, _shootPoint.position, _shootPoint.rotation);

        newBullet.BulletDamage = _damage;
        newBullet.transform.Rotate(0f, 0f, rndSpread);
        newBullet.LaunchBullet(newBullet.transform.right * _enemyGunSetting.BulletSpeed);

        Destroy(newBullet.gameObject, _enemyGunSetting.Range * 2 / _enemyGunSetting.BulletSpeed);

        _currentMagazineSize -= _enemyGunSetting.BulletCost;
        _nextFire = 1;
    }

    private IEnumerator Reload()
    {
        _isReloading = true;

        yield return new WaitForSeconds(_enemyGunSetting.ReloadTime);

        _isReloading = false;
        _currentMagazineSize = _enemyGunSetting.MagazineSize;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (_enemyGunSetting == null) return;

        Gizmos.DrawWireSphere(transform.position, _enemyGunSetting.Range);
    }

    private void OnGUI()
    {
        if (!_showDebugInfo) return;

        GUILayout.BeginArea(new Rect(10, 10, 400, 600));
        GUILayout.Label("Health : " + _currentHealth);
        GUILayout.Label("Can Move : " + _canMove);
        GUILayout.Label("Current Magazine Size : " + _currentMagazineSize);
        GUILayout.Label("Next Fire : " + _nextFire);
        GUILayout.Label("Reloading : " + _isReloading);
        GUILayout.Label("Attacking : " + _isAttacking);
        GUILayout.EndArea();
    }
}
