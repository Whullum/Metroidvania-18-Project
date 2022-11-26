using UnityEngine;

public class PatrolEnemy : Enemy
{
    private int _patrolIndex;

    [Header("Patrol enemy properties")]
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private bool _randomPatrol;

    private void Start()
    {
        _patrolIndex = 0;
        Move(_patrolPoints[0]);
    }

    protected override void Update()
    {
        base.Update();

        NextPoint();
    }

    private void NextPoint()
    {
        if (_canMove) return;

        if (_randomPatrol)
        {
            Move(_patrolPoints[Random.Range(0, _patrolPoints.Length)]);
        }
        else
        {
            _patrolIndex++;

            if (_patrolIndex >= _patrolPoints.Length)
                _patrolIndex = 0;

            Move(_patrolPoints[_patrolIndex]);
        }
    }
}
