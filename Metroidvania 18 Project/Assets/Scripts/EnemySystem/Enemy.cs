using UnityEngine;

[RequireComponent(typeof(DamageableEntity))]
public class Enemy : MonoBehaviour
{
    protected bool _canMove; // If set to true the enemy moves in the direction asigned in _destiationPos.
    protected static Transform _player; // The player transform, used to check if player is in range/vision radius.
    private Vector3 _destinationPos; // The destination of the enemy.

    [Header("Base enemy properties")]
    [Tooltip("Damage the enemy does when the player touches it.")]
    [SerializeField] protected int _damage;
    [Tooltip("The speed of the enemy.")]
    [SerializeField] protected float _speed;
    [Tooltip("Distance where the enemy stops.")]
    [SerializeField] private float _reachedDistance = 0.5f;
    [SerializeField] protected bool _showDebugInfo;

    protected virtual void Start()
    {
        // We search for the player if the variable is not set.
        if (_player == null) _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        MoveEnemy();
    }

    /// <summary>
    /// Set the position where the enemy will go and starts moving.
    /// </summary>
    /// <param name="position">The destination poisition where the enemy will go.</param>
    protected void Move(Vector3 position)
    {
        _destinationPos = position;

        _canMove = true;
    }

    /// <summary>
    /// Moves the enemy towards the destination position and if it reaches the destination it stops moving.
    /// </summary>
    private void MoveEnemy()
    {
        if (!_canMove) return;

        // Calculate the movement direction.
        Vector3 movementDirection = (_destinationPos - transform.position).normalized; 

        // Check if the enemy reached the destination.
        if (Vector2.Distance(transform.position, _destinationPos) <= _reachedDistance)
            _canMove = false;

        transform.Translate(movementDirection * Time.deltaTime * _speed);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        if (collision.collider.TryGetComponent(out DamageableEntity player))
            player.ReceiveDamage(_damage);
    }
}
