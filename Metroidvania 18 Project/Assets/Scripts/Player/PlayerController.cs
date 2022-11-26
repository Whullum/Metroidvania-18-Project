using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    private int _currentHealth;

    [SerializeField] private int _maxHealth;
    [SerializeField] private bool _showDebugInfo;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void Death()
    {
        gameObject.SetActive(false);
    }

    public void ReceiveDamage(int damageAmmount)
    {
        _currentHealth -= damageAmmount;

        if (_currentHealth <= 0)
            Death();
    }

    private void OnGUI()
    {
        if (!_showDebugInfo) return;

        GUILayout.BeginArea(new Rect(400, 10, 200, 100));
        GUILayout.Label("Player Health : " + _currentHealth);
        GUILayout.EndArea();
    }
}
