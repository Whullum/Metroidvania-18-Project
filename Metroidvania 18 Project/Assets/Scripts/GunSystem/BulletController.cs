using UnityEngine;

public class BulletController : MonoBehaviour
{
    /// <summary>
    /// Ammount of damage this bullet does.
    /// </summary>
    public int BulletDamage { get; set; }

    private Rigidbody2D _rBody;

    private void Awake()
    {
        _rBody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out DamageableEntity damageable))
        {
            damageable.ReceiveDamage(BulletDamage);
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Applies the bullet rigidbody a force in a specific direction.
    /// </summary>
    /// <param name="direction">The direction of the force.</param>
    public void LaunchBullet(Vector3 direction)
    {
        _rBody.AddForce(direction, ForceMode2D.Impulse);
    }
}
