using UnityEngine;

public class GunUpgrade : MonoBehaviour
{
    private static Gun _gun;

    [SerializeField] private GunSetting _gunSetting;
    [SerializeField] private AK.Wwise.Event _pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) { return; }

        if (_gun == null)
            _gun = collision.GetComponentInChildren<Gun>();

        if(!_gun)
        {
            Debug.LogError("Gun Upgrade ERROR : Cannot find player Gun.");
            return;
        }

        _gun.UpgradeGun(_gunSetting);
        _pickupSound.Post(gameObject);

        Destroy(gameObject);
    }
}
