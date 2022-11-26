using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Transform _player;

    [SerializeField] private float _visionRadius = 5.0f;
    [SerializeField] private float _chaseDistance = 5.0f;
    [SerializeField] private float _attackDistance = 1.0f;

    private void Start()
    {
        _checkState = true;
    }

    public override void ExecuteState()
    {
        Debug.Log("Persiguiendo");

        float distance = Vector2.Distance(transform.position, _player.position);

        if (distance > _chaseDistance)
        {
            //_enemy.PopState();

            Debug.Log("Saluiendo de perseguir.");
        }

        if(distance < _attackDistance)
        {
            
        }
    }

    public override void Check()
    {
        base.Check();

        Collider2D player = Physics2D.OverlapCircle(transform.position, _visionRadius, LayerMask.GetMask("Player"));

        if (player == null)
            return;

        _player = player.transform;
        //_enemy.PushState(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _visionRadius);
        Gizmos.DrawWireSphere(transform.position, _attackDistance);
    }
}
