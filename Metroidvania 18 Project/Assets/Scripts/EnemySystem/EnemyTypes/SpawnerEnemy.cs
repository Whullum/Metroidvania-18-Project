using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : Enemy
{
    /// <summary>
    /// Counter of the current alive EnemyDrones.
    /// </summary>
    public int AliveDrones { get; set; }

    /// <summary>
    /// List containing the EnemyDrones that are currently chasing the player.
    /// </summary>
    public List<Transform> ActiveDrones { get; private set; } = new List<Transform>();

    private int _spawnedEnemiesCount;
    private float _elapsedSpawnTime;
    private bool _startSpawn = false;
    private bool _isBlinking;

    [Header("Spawner enemy properties")]
    [Tooltip("Maximum number of EnemyDrones this spawner will create.")]
    [SerializeField] private int _maxEnemySpawn;
    [Tooltip("Maximum number of EnemyDrones alive at the same time.")]
    [SerializeField] private int _aliveEnemyCap;
    [Tooltip("EnemyDrones per second that will be created.")]
    [SerializeField] private float _spawnRate;
    [Tooltip("The distance where if the player reaches the spawner is activated.")]
    [SerializeField] private float _triggerDistance;
    [Tooltip("Time this enemy will check if the player is inside its range.")]
    [SerializeField] private float _playerCheckTime = 0.5f;
    [Tooltip("The EnemyDrone prefab that will be instantiated.")]
    [SerializeField] private DroneEnemy _enemyPrefab;
    [Tooltip("Prefab of the circle to be instantiated as the visible trigger radius.")]
    [SerializeField] private GameObject _visibleTriggerRadius;
    [Tooltip("Ammount of times the visible trigger radius will blink when tha player is in.")]
    [SerializeField] private int _blinkCount = 2;

    protected override void Start()
    {
        base.Start();

        InvokeRepeating("TriggerSpawner", 0f, _playerCheckTime);

        CreateVisibleRadius();
    }

    protected override void Update()
    {
        base.Update();

        _elapsedSpawnTime -= Time.deltaTime * _spawnRate;

        if (!_startSpawn || _spawnedEnemiesCount > _maxEnemySpawn) return;

        if (_elapsedSpawnTime <= 0)
            SpawnEnemy();
    }

    /// <summary>
    /// Checks if this spawner can continue spawning enemies and creates one.
    /// </summary>
    private void SpawnEnemy()
    {
        // Check if the cap is reached.
        if (AliveDrones >= _aliveEnemyCap)
        {
            _startSpawn = false;

            InvokeRepeating("TriggerSpawner", 0f, _playerCheckTime);

            return;
        }

        // Create a new drone.
        DroneEnemy newDrone = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);

        newDrone.Spawner = this;

        // Add the new drone to the active drones list.
        ActiveDrones.Add(newDrone.transform);
        // Set the counter of alive drones.
        AliveDrones++;
        // Set the total spawned enemies.
        _spawnedEnemiesCount++;

        _elapsedSpawnTime = 1;
    }

    /// <summary>
    /// Activates the spawner if the player gets close enough.
    /// </summary>
    private void TriggerSpawner()
    {
        float distance = Vector2.Distance(transform.position, _player.position);

        if (distance < _triggerDistance)
        {
            if(!_isBlinking)
                StartCoroutine(BlinkTriggerRadius());

            _startSpawn = true;

            CancelInvoke("TriggerSpawner");
        }
    }

    /// <summary>
    /// Removes an EnemyDrone from the ActiveDrones list.
    /// </summary>
    /// <param name="drone">The EnemyDrone to be removed.</param>
    public void RemoveDrone(Transform drone)
    {
        ActiveDrones.Remove(drone);
    }

    /// <summary>
    /// Creates the visible trigger radius with the specified radius.
    /// </summary>
    private void CreateVisibleRadius()
    {
        GameObject prefab = _visibleTriggerRadius;

        _visibleTriggerRadius = Instantiate(prefab, transform.position, Quaternion.identity);
        _visibleTriggerRadius.transform.parent = transform;
        _visibleTriggerRadius.transform.localScale = new Vector3(_triggerDistance, _triggerDistance, _triggerDistance) * 2;
    }

    /// <summary>
    /// When the player enters the enemy radius, the enemy gives some feedback blinking the visible trigger radius.
    /// </summary>
    /// <returns>Blinking time.</returns>
    private IEnumerator BlinkTriggerRadius()
    {
        _isBlinking = true;

        for (int i = 0; i < _blinkCount; i++)
        {
            _visibleTriggerRadius.SetActive(false);

            yield return new WaitForSeconds(0.1f);

            _visibleTriggerRadius.SetActive(true);

            yield return new WaitForSeconds(0.1f);
        }

        _isBlinking = false;
    }

    private void OnDrawGizmos()
    {
        if (!_showDebugInfo) return;

        Gizmos.DrawWireSphere(transform.position, _triggerDistance);
    }
}
