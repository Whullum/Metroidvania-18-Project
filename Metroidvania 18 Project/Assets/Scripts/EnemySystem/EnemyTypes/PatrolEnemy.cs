using System.Collections;
using UnityEngine;

public class PatrolEnemy : Enemy
{
    // Current patrol point index.
    private int _patrolIndex;
    private float _movingSpeed;
    private Vector3 _startPosition; // Used for the sine wave motion.

    [Header("Patrol enemy properties")]
    [Tooltip("The patrol points that this enemy will use to move.")]
    [SerializeField] private Transform[] _patrolPoints;
    [Tooltip("Time the enemy will stop after reaching a patrol point.")]
    [SerializeField] private float _stopTime= 3.0f;
    [Tooltip("Acceleration of the enemy.")]
    [Range(0.1f, 20)]
    [SerializeField] private float _acceleration = 2.0f;
    [Range(0, 10)]
    [Tooltip("Distance moved verticaly.")]
    [SerializeField] private float _sineWaveMagnitude = 1f;
    [Range(0, 10)]
    [Tooltip("Frequency of the waves.")]
    [SerializeField] private float _sineWaveFrequency = 1f;

    protected override void Start()
    {
        base.Start();

        //Initialize all values.
        _patrolIndex = 0;
        transform.position = _patrolPoints[_patrolIndex].position;
        _startPosition = transform.position;
        _movingSpeed = _speed;

        Move(_patrolPoints[_patrolIndex].position);
    }

    protected override void Update()
    {
        base.Update();

        SineWaveMotion();
        CalculateNextPoint();
    }

    /// <summary>
    /// Calculates the next point that the enemy will move towards.
    /// </summary>
    private void CalculateNextPoint()
    {
        if (_canMove) return;

        _patrolIndex++;

        if (_patrolIndex >= _patrolPoints.Length)
            _patrolIndex = 0;

        Move(_patrolPoints[_patrolIndex].position);
        StartCoroutine(Accelerate());
    }

    /// <summary>
    /// Sine Wave motion that simulates that the enemy is flying.
    /// </summary>
    private void SineWaveMotion()
    {
        
        Vector3 motion = Vector3.zero;
        motion.x = transform.position.x;
        motion.y = _startPosition.y + Mathf.Sin(Time.time * _sineWaveFrequency) * _sineWaveMagnitude;
        motion.z = transform.position.z;

        transform.position = motion;
    }

    /// <summary>
    /// Accelerates the enemy until it reaches its defined speed.
    /// </summary>
    private IEnumerator Accelerate()
    {
        _speed = 0;

        yield return new WaitForSeconds(_stopTime);

        while(_speed < _movingSpeed)
        {
            _startPosition.y = Mathf.Lerp(_startPosition.y, _patrolPoints[_patrolIndex].position.y, Time.deltaTime * _acceleration);

            _speed += Time.deltaTime * _acceleration;

            yield return null;
        }
    }
}
