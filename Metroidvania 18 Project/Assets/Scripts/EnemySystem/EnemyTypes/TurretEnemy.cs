using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TurretEnemy : Enemy
{
    private float _currentMagazineSize;
    private float _nextFire; // Time to fire again.
    protected float _distanceToPlayer;
    private bool _isReloading;
    private bool _isAttacking;
    private bool _playerNear;
    private LineRenderer _laserSight; // The Line Renderer component.

    [Header("Turret enemy properties")]
    [Tooltip("Every Check Time the enemy will calculate if the player is in range. Lower Check Time will make the enemy react faster.")]
    [Range(0.01f, 10)]
    [SerializeField] private float _playerCheckTime = 0.5f;
    [Tooltip("The angle needed towards the player for the Gun to shoot.")]
    [SerializeField] private float _angleToShoot = 20.0f;
    [Tooltip("The Enemy Gun Setting that this Turret Enemy will use to shoot at the player.")]
    [SerializeField] private EnemyGunSetting _enemyGunSetting;
    [Tooltip("The gun GameObject. Used for rotation.")]
    [SerializeField] private Transform _gun;
    [Tooltip("Point where the bullets will spawn.")]
    [SerializeField] private Transform _shootPoint;

    protected override void Start()
    {
        base.Start();

        _laserSight = GetComponent<LineRenderer>();

        _currentMagazineSize = _enemyGunSetting.MagazineSize;

        InvokeRepeating("SearchPlayer", 0f, _playerCheckTime);
    }

    protected override void Update()
    {
        base.Update();

        _nextFire -= Time.deltaTime * _enemyGunSetting.FireRate;

        if (!_playerNear) return;

        RotateTowardsPlayer();

        if (!_isAttacking) return;

        ShootPlayer();
    }

    /// <summary>
    /// Check if the distance between this enemy and the player is less than the range of the enemy, if so, starts attacking the player.
    /// </summary>
    private void SearchPlayer()
    {
        _distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        if (_distanceToPlayer <= _enemyGunSetting.Range)
        {
            _playerNear = true;
            _animator.SetBool("playerSpotted", _playerNear);
        }
        else
        {
            _playerNear = false;
            _animator.SetBool("playerSpotted", _playerNear);
        }
    }

    /// <summary>
    /// Set the line renderer to create a straight line from the gun to the collision point.
    /// </summary>
    private void EnableLaserSight()
    {
        // Raycast to get the collision point.
        RaycastHit2D laserHit = Physics2D.Raycast(_gun.position, _gun.transform.right, _enemyGunSetting.Range * 10, ~LayerMask.GetMask("Enemy", "EnemyBullet", "Confiner", "Bullet"));

        // Set the second point to be the collision point.
        if (laserHit)
        {
            _laserSight.SetPosition(1, laserHit.point);

            if (laserHit.collider.CompareTag("Player"))
                _isAttacking = true;
            else
                _isAttacking = false;
        }
        // If theres no collision, we create our own point far away from the gun shoot direction.
        else
            _laserSight.SetPosition(1, _gun.position + _gun.transform.right * _enemyGunSetting.Range * 10);

        // Set the initial position to be the gun position.
        _laserSight.SetPosition(0, _gun.transform.position);
    }

    /// <summary>
    /// Rotates the Gun towards the player position with a specific speed.
    /// </summary>
    private void RotateTowardsPlayer()
    {
        Vector3 playerDirection = _player.position - transform.position;
        RaycastHit2D playerOnSight = Physics2D.Raycast(transform.position, playerDirection, _enemyGunSetting.Range * 10, ~LayerMask.GetMask("Enemy", "EnemyBullet", "Confiner", "Bullet"));

        if (playerOnSight.collider.CompareTag("Player"))
        {
            _laserSight.enabled = true;

            EnableLaserSight();

            Vector3 rotation = _player.position - transform.position;
            float angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            _gun.rotation = Quaternion.Lerp(_gun.rotation, lookRotation, Time.deltaTime * _enemyGunSetting.RotationSpeed);
        }
        else
            _laserSight.enabled = false;
    }

    /// <summary>
    /// Checks the fire rate and reloading status and shoots bullets.
    /// </summary>
    private void ShootPlayer()
    {
        if (_isReloading) return;

        if (_nextFire <= 0)
        {
            Vector3 lookAtPlayer = _player.position - transform.position;
            float angleToPlayer = Vector3.Angle(_gun.right, lookAtPlayer);

            if (lookAtPlayer.x >= 0)
                _gun.GetComponentInChildren<SpriteRenderer>().flipY = true;
            else
                _gun.GetComponentInChildren<SpriteRenderer>().flipY = false;

            if (angleToPlayer < _angleToShoot)
            {
                if (_currentMagazineSize <= 0)
                    StartCoroutine(Reload());
                else
                    CreateBullet();
            }
        }
    }

    /// <summary>
    /// Creates a bullet with the specified Enemy Gun Settings.
    /// </summary>
    private void CreateBullet()
    {
        // Generate a random rotation for the bullet spread.
        float rndSpread = Random.Range(-_enemyGunSetting.BulletSpread, _enemyGunSetting.BulletSpread);

        BulletController newBullet = Instantiate(_enemyGunSetting.BulletPrefab, _shootPoint.position, _shootPoint.rotation);

        newBullet.BulletDamage = _enemyGunSetting.Damage;
        newBullet.GunSetting = GunSettingID.NONE;
        newBullet.transform.Rotate(0f, 0f, rndSpread);
        newBullet.LaunchBullet(newBullet.transform.right * _enemyGunSetting.BulletSpeed);

        Destroy(newBullet.gameObject, _enemyGunSetting.Range * 2 / _enemyGunSetting.BulletSpeed);

        _currentMagazineSize -= _enemyGunSetting.BulletCost;
        _nextFire = 1;
    }

    /// <summary>
    /// Prevents the Gun from shooting and starts reloading it.
    /// </summary>
    /// <returns>The reload time.</returns>
    private IEnumerator Reload()
    {
        _isReloading = true;

        yield return new WaitForSeconds(_enemyGunSetting.ReloadTime);

        _isReloading = false;
        _currentMagazineSize = _enemyGunSetting.MagazineSize;
    }

    protected virtual void OnDrawGizmos()
    {
        if (!_showDebugInfo || _enemyGunSetting == null) return;

        Vector3 frontRay = _gun.position + (_gun.transform.right * _enemyGunSetting.Range);

        Gizmos.DrawWireSphere(transform.position, _enemyGunSetting.Range);
        Debug.DrawLine(_gun.position, frontRay, Color.red);
    }

    private void OnGUI()
    {
        if (!_showDebugInfo) return;

        GUILayout.BeginArea(new Rect(10, 10, 400, 600));
        GUILayout.Label("Can Move : " + _canMove);
        GUILayout.Label("Current Magazine Size : " + _currentMagazineSize);
        GUILayout.Label("Next Fire : " + _nextFire);
        GUILayout.Label("Reloading : " + _isReloading);
        GUILayout.Label("Attacking : " + _isAttacking);
        GUILayout.EndArea();
    }
}
