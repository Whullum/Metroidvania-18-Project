using System;
using UnityEngine;

public class DamageableEntity : MonoBehaviour
{
    /// <summary>
    /// Invoked when this entity is hit.
    /// </summary>
    public Action DamageReceived { get; set; }

    private int _currentHealth;
    private Color _baseColor;
    private SpriteRenderer _spriteRenderer;

    [Tooltip("The maximum health of the enemy.")]
    [SerializeField] private int _maxHealth;
    [Tooltip("Color shown when the enemy is hit by something.")]
    [SerializeField] private Color _hitColor;
    [Tooltip("Particles spawned when this entity dies.")]
    [SerializeField] private GameObject _deathParticles;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _baseColor = _spriteRenderer.color;
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    /// <summary>
    /// Changues the sprite color of the enemy.
    /// </summary>
    private void EnableHitFeedback()
    {
        _spriteRenderer.color = _hitColor;
    }

    /// <summary>
    /// Restores the previous sprite color of the enemy.
    /// </summary>
    private void DisableHitFeedback()
    {
        _spriteRenderer.color = _baseColor;
    }

    /// <summary>
    /// Hits this entity with a specific damage.
    /// </summary>
    /// <param name="damageAmmount">Ammount of damage that this entity will absorb.</param>
    public void ReceiveDamage(int damageAmmount)
    {
        DamageReceived?.Invoke();

        Invoke("EnableHitFeedback", 0f);
        Invoke("DisableHitFeedback", 0.1f);

        _currentHealth -= damageAmmount;

        if (_currentHealth <= 0)
            Death();
    }

    /// <summary>
    /// If this entity current health is below or equal than 0, it dies.
    /// </summary>
    public virtual void Death()
    {
        Instantiate(_deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
