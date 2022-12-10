using UnityEngine;

public class BulletController : MonoBehaviour
{
    /// <summary>
    /// Ammount of damage this bullet does.
    /// </summary>
    public int BulletDamage { get; set; }

    /// <summary>
    /// The Gun Setting that shoot this bullet.
    /// </summary>
    public GunSettingID GunSetting { get; set; }

    private Rigidbody2D _rBody;

    [SerializeField] private GameObject _impactEffect;
    [Range(0, 100)]
    [SerializeField] private float _impactProbability = 30;

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

        CreateHitEffect();
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

    private void CreateHitEffect()
    {
        float probability = ((float)_impactProbability / 100f) * 100;
        int rndImpact = Random.Range(0, 100);

        Debug.Log("Probability: " + probability);
        Debug.Log("Impact: " + rndImpact);

        if (rndImpact <= probability)
            Instantiate(_impactEffect, transform.position, Quaternion.identity);
    }
}
