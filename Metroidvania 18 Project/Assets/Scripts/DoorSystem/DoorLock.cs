using UnityEngine;

public class DoorLock : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [Tooltip("The Gun Setting that opens this lock.")]
    [SerializeField] private GunSettingID _key;
    [SerializeField] private Door _door;
    [SerializeField] private ParticleSystem _doorOpenEffect;

    public GunSettingID Key { get => _key; set => _key = value; }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        LoadGunSettingsColor();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BulletController bullet))
        {
            if (_key == GunSettingID.NONE)
                return;

            if (_key == GunSettingID.ANY_FIRE)
                OpenDoor();
            else if (bullet.GunSetting == _key)
                OpenDoor();
        }
    }

    private void LoadGunSettingsColor()
    {
        GunSetting[] settings = Resources.LoadAll<GunSetting>("Gun");

        if (settings.Length <= 0) { Debug.LogError("Door System ERROR: Cannot load Gun Settings color property."); return; }

        for (int i = 0; i < settings.Length; i++)
        {
            if (!_door.IsTraversable)
            {
                ChangeDoorColor(Color.red);
                _key = GunSettingID.NONE;
            }
            else if (settings[i].ID == _key)
                ChangeDoorColor(settings[i].Color);

            if (_key == GunSettingID.ANY_FIRE)
                ChangeDoorColor(Color.white);
        }
    }

    private void ChangeDoorColor(Color selectColor)
    {
        _spriteRenderer.color = selectColor;
    }

    private void OpenDoor()
    {
        var particles = Instantiate(_doorOpenEffect, transform.position, Quaternion.identity);
        var particleSystem = particles.main;

        particleSystem.startColor = _spriteRenderer.color;
        particles.transform.rotation = transform.rotation;

        Destroy(gameObject);
    }
}
