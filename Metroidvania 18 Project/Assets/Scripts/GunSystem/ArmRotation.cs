using UnityEngine;

public class ArmRotation : MonoBehaviour
{
    private SpriteRenderer _renderer;

    [SerializeField] private Transform _rotateTowards;
    [SerializeField] private float  _offset;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = _rotateTowards.position - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        if (rotZ < 89 && rotZ > -89)
            _renderer.flipY = false;
        else
        {
            _renderer.flipY = true;
            //_offset = -_offset;
        }
            

        transform.rotation = Quaternion.Euler(0, 0, rotZ + _offset);
    }
}
