using UnityEngine;

public class GunUpgrade : PickUp
{
    [SerializeField] private GunSetting _gunSetting;
    [SerializeField] private AK.Wwise.Event _pickupSound;

    protected override void CollectPickUp()
    {
        base.CollectPickUp();

        GameManager.Instance.Player.Gun.UpgradeGun(_gunSetting);
        _pickupSound.Post(gameObject);
    }
}
