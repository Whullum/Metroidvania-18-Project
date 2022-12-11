using System;
using UnityEngine;

public class DamageableEntity : MonoBehaviour
{
    /// <summary>
    /// Invoked when this entity is hit.
    /// </summary>
    public Action DamageReceived { get; set; }

    public int CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
    public int MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
    public bool IsPlayer { get; set; } = false;

    private int _currentHealth;
    private Color _baseColor;
    private SpriteRenderer _spriteRenderer;

    [Tooltip("The maximum health of the enemy.")]
    [SerializeField] private int _maxHealth;
    [Tooltip("Color shown when the enemy is hit by something.")]
    [SerializeField] private Color _hitColor;
    [Tooltip("Particles spawned when this entity dies.")]
    [SerializeField] private GameObject _deathParticles;

    [Header ("Audio for damage sound effects")]
    [Tooltip("Wwise switch for correct sound playback")]
    [SerializeField] private AK.Wwise.Switch _switchEntityDamage;
    [Tooltip("Sound the entitty makes when it is damaged.")]
    [SerializeField] private AK.Wwise.Event _sfxPlayEntityDamaged;
    [Tooltip("Sound the entitty makes when it dies.")]
    [SerializeField] private AK.Wwise.Event _sfxPlayEntityDeath;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _baseColor = _spriteRenderer.color;
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
        _switchEntityDamage.SetValue(gameObject);
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
    /// <param name="damageAmount">Amount of damage that this entity will absorb.</param>
    public void ReceiveDamage(int damageAmount)
    {
        DamageReceived?.Invoke();

        Invoke("EnableHitFeedback", 0f);
        Invoke("DisableHitFeedback", 0.1f);

        _currentHealth -= damageAmount;

        // Play the damaged sound effect for this entity
        _sfxPlayEntityDamaged.Post(gameObject);

        if (_currentHealth <= 0)
            Death();
    }

    public void RestoreHealth(int amount)
    {
        if (_currentHealth + amount > _maxHealth)
            _currentHealth = _maxHealth;
        else
            _currentHealth += amount;
    }

    public void UpgradeHealth(int amount)
    {
        _maxHealth += amount;
    }

    /// <summary>
    /// If this entity current health is below or equal than 0, it dies.
    /// </summary>
    public virtual void Death()
    {
        // Play the death sound effect for this entity
        _sfxPlayEntityDeath.Post(gameObject);

        Instantiate(_deathParticles, transform.position, Quaternion.identity);

        if(!IsPlayer)
            Destroy(gameObject);
    }
}
