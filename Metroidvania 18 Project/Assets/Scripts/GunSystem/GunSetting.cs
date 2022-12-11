using UnityEngine;

[CreateAssetMenu(fileName = "New Gun Setting", menuName = "Gun/Gun Setting")]
public class GunSetting : ScriptableObject
{
    [Tooltip("Unique ID of this setting.")]
    public GunSettingID ID;

    [Range(1, 1000)]
    [Tooltip("Damage per bullet.")]
    public int Damage;

    [Range(1, 1000)]
    [Tooltip("How many bullets this setting can shoot before reloading.")]
    public int MagazineSize;

    [Tooltip("Rate of fire in bullets per second.")]
    [Range(0.5f, 50)]
    public float FireRate;

    [Tooltip("Range of the weapon. When the bullet reaches this range, it disappears.")]
    [Range(1.0f, 100.0f)]
    public float Range;

    [Tooltip("When the magazine is empty, the time to wait before the player can shoot again.")]
    [Range(0.1f, 30.0f)]
    public float ReloadTime;

    [Tooltip("Prefab to be instantiated by the gun.")]
    public GameObject BulletPrefab;

    [Tooltip("Random amount of dispersion applied for each bullet.")]
    [Range(0.0f, 180.0f)]
    public float BulletSpread;

    [Tooltip("The speed at which the bullet will be launched.")]
    [Range(1.0f, 200.0f)]
    public float BulletSpeed;

    [Tooltip("Cost of shooting one bullet. Greater the cost will rapidly empty the magazine size.")]
    [Range(0.1f, 100.0f)]
    public float BulletCost;

    [Tooltip("Amount of force applied opposite the shooting direction.")]
    [Range(0.0f, 100.0f)]
    public float Recoil;

    [Tooltip("Duration of the shake when the player shoots.")]
    [Range(0.0f, 5.0f)]
    public float CameraShakeDuration;

    [Tooltip("Force of the camera shake.")]
    [Range(0.0f, 50.0f)]
    public float CameraShakeForce;

    [Tooltip("If the Shotgun Mode is enabled, all the bullets inside the magazine will be launched in one click. This needs to be set with a higher Fire Rate and a lower Magazine Size so it makes the illusion of pellets coming out.")]
    public bool ShotgunMode;

    [Tooltip("Wwise Switch for correct sound effect playback.")]
    public AK.Wwise.Switch WwiseSwitch;

    [Tooltip("This color represents wich doors can be opened by this Gun Setting. Also, the bullets will get this color.")]
    public Color Color;
}
