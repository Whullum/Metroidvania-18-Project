using UnityEngine;

public class GunUpgrade : MonoBehaviour
{
    private static Gun _gun;

    [SerializeField] private GunSetting _gunSetting;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) { return; }

        if (_gun == null)
            _gun = collision.GetComponentInChildren<Gun>();

        Destroy(gameObject);
    }
}
