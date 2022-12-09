using UnityEngine;

public class DoubleJumpUpgrade : PickUp
{
    protected override void CollectPickUp()
    {
        base.CollectPickUp();

        GameManager.Instance.Player.UnlockDoubleJump();
    }
}
