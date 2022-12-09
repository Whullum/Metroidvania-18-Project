using UnityEngine;

public class Resource : PickUp
{
    [SerializeField] private int _resourceAmount = 0;

    protected override void CollectPickUp()
    {
        base.CollectPickUp();
        _promptText = "Resource Collected " + _resourceAmount;
        ResourceManager.CollecResource(_resourceAmount);
        PlayerUI.Instance.UpdateUIValues();
    }
}
