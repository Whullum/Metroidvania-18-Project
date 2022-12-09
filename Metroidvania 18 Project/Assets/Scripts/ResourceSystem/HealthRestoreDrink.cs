using UnityEngine;

[CreateAssetMenu(fileName = "New Health Restore Drink", menuName = "Drinks/Health Restore")]
public class HealthRestoreDrink : Drink
{
    public int HealthRestore;
    public ParticleSystem _healthRestoreEffect;

    public override void UseDrink()
    {
        GameManager.Instance.Player.Health.RestoreHealth(HealthRestore);
        Instantiate(_healthRestoreEffect, GameManager.Instance.Player.transform.position, Quaternion.identity).transform.parent = GameManager.Instance.Player.transform;
    }
}
