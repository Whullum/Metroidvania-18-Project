using UnityEditor;
using UnityEngine;

public class PickUp : UniqueGUID
{
    private bool _collected;

    [SerializeField] protected string _promptText;
    [SerializeField] private PickUpPromptUI _ui;
    [SerializeField] private float _rotationSpeed = 30.0f;

    private void Start()
    {
        InitializePickUp();
    }

    private void Update()
    {
        RotatePickUp();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CollectPickUp();
            CreatePromptUI();
            Destroy(gameObject);
        }
    }

    protected virtual void CollectPickUp()
    {
        _collected = true;

        ResourceManager.CollectPickUp(ID, _collected);

        var particle = GetComponentInChildren<ParticleSystem>();

        particle.transform.parent = null;
        particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        PlayerUI.Instance.UpdateUIValues();
    }

    private void InitializePickUp()
    {
        PickUpData pickUp = ResourceManager.GetPickUp(ID);

        if (pickUp != null)
        {
            if (pickUp.Collected)
                Destroy(gameObject);
        }
        else
        {
            ResourceManager.AddPickUp(ID, _collected);
        }
    }

    private void CreatePromptUI()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);

        Instantiate(_ui, position, Quaternion.identity).SetPromptText(_promptText);
    }

    private void RotatePickUp()
    {
        transform.Rotate(new Vector3(0f, 0f, _rotationSpeed * Time.deltaTime));
    }
}
