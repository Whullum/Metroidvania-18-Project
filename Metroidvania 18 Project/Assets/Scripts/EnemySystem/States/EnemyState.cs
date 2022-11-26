using UnityEngine;

[RequireComponent(typeof(Enemy))]
public abstract class EnemyState : MonoBehaviour
{
    public int Priority { get { return _priority; } }
    public bool CheckState { get { return _checkState; } }
    public float CheckTime { get { return _checkTime; } }

    protected bool _checkState = false;
    protected Enemy _enemy;

    [SerializeField] private int _priority;
    [SerializeField] private float _checkTime;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    public abstract void ExecuteState();

    public virtual void Check()
    {
        if (!_checkState)
            return;
    }
}
