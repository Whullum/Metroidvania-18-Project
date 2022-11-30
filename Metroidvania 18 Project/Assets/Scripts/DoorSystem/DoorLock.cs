using UnityEngine;

public class DoorLock : MonoBehaviour
{
    [Tooltip("The Gun Setting that opens this lock.")]
    [SerializeField] private GunSettingID _key;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BulletController bullet))
        {
            if (bullet.GunSetting == _key)
                OpenDoor();
        }
    }

    private void OpenDoor()
    {
        Destroy(gameObject);
    }
}
