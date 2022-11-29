using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Gun Setting", menuName = "Enemy/Enemy Gun Setting")]
public class EnemyGunSetting : ScriptableObject
{
    [Range(1, 1000)]
    [Tooltip("Damage per bullet.")]
    public int Damage;

    [Range(1, 100)]
    [Tooltip("Range of the weapon. When the bullet reaches this range, it disappears.")]
    public float Range;

    [Range(0.1f, 360f)]
    [Tooltip("Rotation of the enemy gun.")]
    public float RotationSpeed;

    [Range(0.01f, 50f)]
    [Tooltip("Rate of fire in bullets per second.")]
    public float FireRate;

    [Range(0.1f, 500f)]
    [Tooltip("The speed at which the bullet will be launched.")]
    public float BulletSpeed;

    [Range(0f, 180f)]
    [Tooltip("Random amount of dispersion applied for each bullet.")]
    public float BulletSpread;

    [Range(0f, 1000f)]
    [Tooltip("Cost of shooting one bullet. Greater the cost will rapidly empty the magazine size.")]
    public float BulletCost;

    [Range(0f, 60f)]
    [Tooltip("When the magazine is empty, the time to wait before the enemy can shoot again.")]
    public float ReloadTime;

    [Range(1, 1000)]
    [Tooltip("How many bullets this setting can shoot before reloading.")]
    public int MagazineSize;

    [Tooltip("Bullet prefab to be instantiated by the enemy.")]
    public BulletController BulletPrefab;
}
