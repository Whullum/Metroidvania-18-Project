using UnityEngine;

[CreateAssetMenu(fileName = "New Gun Setting", menuName = "Gun/Gun Setting")]
public class GunSetting : ScriptableObject
{
    public GunSettingID ID;
    public string GunSettingName;
    [Range(1, 1000)]
    [Tooltip("Damage per bullet.")]
    public int Damage;
    [Range(1, 1000)]
    [Tooltip("How many bullets this setting can shoot before reloading.")]
    public int MagazineSize;
    [Range(0.5f, 50)]
    public float FireRate;
    [Range(1.0f, 100.0f)]
    public float Range;
    [Range(0.1f, 30.0f)]
    public float ReloadTime;
    public GameObject BulletPrefab;
    [Range(0.0f, 180.0f)]
    public float BulletSpread;
    [Range(1.0f, 100.0f)]
    public float BulletSpeed;
    [Range(0.1f, 100.0f)]
    public float BulletCost;
    public bool ShotgunMode;
}
