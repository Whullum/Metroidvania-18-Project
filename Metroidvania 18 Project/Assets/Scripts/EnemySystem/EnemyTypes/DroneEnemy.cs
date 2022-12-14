using System.Collections;
using UnityEngine;

public class DroneEnemy : Enemy
{
    /// <summary>
    /// The spawner of this DroneEnemy.
    /// </summary>
    public SpawnerEnemy Spawner { get; set; }

    [SerializeField]
    private Animator _droneAnim;

    // Space between other EnemyDrone entities.
    private float _spaceBetween = 1.0f;

    private void OnDisable()
    {
        Spawner.AliveDrones--;
        Spawner.RemoveDrone(transform);
    }

    protected override void Start()
    {
        base.Start();

        Move(_player.position);
    }

    protected override void Update()
    {
        base.Update();

        if (!_player) return;

        // We set the desired position to move to the current player position.
        Move(_player.position);
        // We prevent the EnemyDrones from overlaping each other.
        Flocking();
    }

    /// <summary>
    /// Basi flocking behavior. Prevents EnemyDrone entities from overlaping each other.
    /// </summary>
    private void Flocking()
    {
        foreach(Transform drone in Spawner.ActiveDrones)
        {
            // If the EnemyDrone is this one, we skip it.
            if (drone == this.transform) continue;

            // Calculate the distance between this EnemyDrone and the other one.
            float distance = Vector2.Distance(drone.position, transform.position);

            // If the distance is less than the desired, we changue directions and move opposite the other EnemyDrone position.
            if(distance <= _spaceBetween)
            {
                Vector3 direction = (transform.position - drone.position).normalized;

                transform.Translate(direction * Time.deltaTime * _speed);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out DamageableEntity player))
                player.ReceiveDamage(_damage);

            GetComponent<DamageableEntity>().Death();
        }
    }
}
