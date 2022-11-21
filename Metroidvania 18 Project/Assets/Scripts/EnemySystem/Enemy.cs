using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    protected int _currentHealth;
    protected bool _canMove;
    private Transform _destinationPos;

    [Header("Base enemy properties")]
    [SerializeField] private int _maxHealth;
    [SerializeField] protected int _damage;
    [SerializeField] private float _speed;
    [SerializeField] protected float _attackSpeed;
    [SerializeField] private float _reachedDistance;
    [SerializeField] protected bool _showDebugInfo;

    protected virtual void Start()
    {
        _currentHealth = _maxHealth;
    }

    protected virtual void Update()
    {
        MoveEnemy();
    }

    protected void Move(Transform position)
    {
        _destinationPos = position;

        _canMove = true;
    }

    private void MoveEnemy()
    {
        if (!_canMove) return;

        if (Vector2.Distance(transform.position, _destinationPos.position) <= _reachedDistance)
            _canMove = false;

        transform.position = Vector2.MoveTowards(transform.position, _destinationPos.position, Time.deltaTime * _speed);
    }

    public void ReceiveDamage(int damageAmmount)
    {
        _currentHealth -= damageAmmount;

        if (_currentHealth <= 0)
            Death();
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    protected virtual void OnDrawGizmos()
    {
        if (!_showDebugInfo) return;
    }
}
