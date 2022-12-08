using UnityEngine;

public class DoorLock : MonoBehaviour
{
    [Tooltip("The Gun Setting that opens this lock.")]
    [SerializeField] private GunSettingID _key;
    [SerializeField] private Door _door;

    public GunSettingID Key { get => _key; set => _key = value; }

    private void Start()
    {
        if(!_door.IsTraversable)
        {
            ChangeDoorColor(Color.red);
            _key = GunSettingID.NONE;
        }
        else
        {
            switch (_key)
            {
                case GunSettingID.ANY_FIRE:
                    ChangeDoorColor(Color.blue);
                    break;
                case GunSettingID.SHOTGUN_FIRE:
                    ChangeDoorColor(Color.green);
                    break;
                case GunSettingID.STREAM_FIRE:
                    ChangeDoorColor(Color.cyan);
                    break;
                case GunSettingID.BURST_FIRE:
                    ChangeDoorColor(Color.yellow);
                    break;

            }
        }
    }

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

    private void ChangeDoorColor(Color selectColor)
    {
        GetComponent<SpriteRenderer>().color = selectColor;
    }

    private void OpenDoor()
    {
        Destroy(gameObject);
    }
}
