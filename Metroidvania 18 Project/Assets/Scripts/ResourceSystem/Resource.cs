using UnityEngine;

public class Resource : UniqueGUID
{
    private bool _collected;
    
    [SerializeField] private int _resourceAmount = 0;

    private void Start()
    {
        InitializeResource();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            CollectResource();
    }

    private void CollectResource()
    {
        _collected = true;

        ResourceManager.CollectResource(ID, _collected, _resourceAmount);

        var particle = GetComponentInChildren<ParticleSystem>();

        particle.transform.parent = null;
        particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        PlayerUI.Instance.UpdateUIValues();

        Destroy(gameObject);
    }

    private void InitializeResource()
    {
        ResourceData resource = ResourceManager.GetResource(ID);

        if(resource != null)
        {
            if (resource.Collected)
                Destroy(gameObject);
        }
        else
        {
            ResourceManager.AddResource(ID, _collected);
        }
    }
}
