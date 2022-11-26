using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Gun Setting", menuName = "Enemy/Enemy Gun Setting")]
public class EnemyGunSetting : ScriptableObject
{
    [Range(1, 100)]
    public float Range;
    [Range(0.1f, 360f)]
    public float RotationSpeed;
    [Range(0.01f, 50f)]
    public float FireRate;
    [Range(0.1f, 500f)]
    public float BulletSpeed;
    [Range(0f, 180f)]
    public float BulletSpread;
    [Range(0f, 1000f)]
    public float BulletCost;
    [Range(0f, 60f)]
    public float ReloadTime;
    [Range(1, 1000)]
    public int MagazineSize;
    public BulletController BulletPrefab;
}
