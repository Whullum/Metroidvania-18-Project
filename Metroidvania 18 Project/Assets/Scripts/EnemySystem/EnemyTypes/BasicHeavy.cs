using UnityEngine;

public class BasicHeavy : TurretEnemy
{
    private bool _isChasing;
    private Vector3 _initialPosition; // Inital position of the enemy.

    [Header("Basic Heavy enemy properties")]
    [Tooltip("Distance the player needs to be from the enemy to start chasing it.")]
    [SerializeField] private float _chaseDistance;

    protected override void Start()
    {
        base.Start();

        _initialPosition = transform.position;
    }

    protected override void Update()
    {
        base.Update();

        ChasePlayer();
    }

    /// <summary>
    /// If the player is inside the chase distance, the enemy will start chasing it.
    /// </summary>
    private void ChasePlayer()
    {
        if (_distanceToPlayer < _chaseDistance)
        {
            Move(_player.position);

            _isChasing = true;
        }
        else if (_isChasing)
        {
            Invoke("ReturnToInitialPosition", 4);

            _isChasing = false;
        }
    }

    /// <summary>
    /// If the enemy losses sight of the player, it return to its initial position.
    /// </summary>
    private void ReturnToInitialPosition()
    {
        Move(_initialPosition);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (!_showDebugInfo) return;

        Gizmos.DrawWireSphere(transform.position, _chaseDistance);
    }
}
