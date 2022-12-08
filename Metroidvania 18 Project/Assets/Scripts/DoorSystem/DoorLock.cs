using UnityEngine;

public class DoorLock : MonoBehaviour
{
    [Tooltip("The Gun Setting that opens this lock.")]
    [SerializeField] private GunSettingID _key;

    public GunSettingID Key { get => _key; set => _key = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BulletController bullet))
        {
            if (_key == GunSettingID.ANY_FIRE)
                OpenDoor();
            else if (bullet.GunSetting == _key)
                OpenDoor();
        }
    }

    private void OpenDoor()
    {
        Destroy(gameObject);
    }
}
