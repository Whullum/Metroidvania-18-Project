using UnityEngine;

public class ArmRotation : MonoBehaviour
{
    private SpriteRenderer _renderer;
    

    [SerializeField] private ArmSide _side;
    [SerializeField] private Transform _leftSideGrip;
    [SerializeField] private Transform _rightSideGrip;
    [SerializeField] private float  _offset;
    [SerializeField] private Gun _gun;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Vector3 side = Vector3.zero;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (_gun.IsFacingRight && _side == ArmSide.Right)
        {
            side = _rightSideGrip.position;
            _renderer.sortingOrder = 2;
        }
        if (_gun.IsFacingRight && _side == ArmSide.Left)
        {
            side = _leftSideGrip.position;
            _renderer.sortingOrder = 0;
        }
        if (!_gun.IsFacingRight && _side == ArmSide.Right)
        {
            side = _leftSideGrip.position;
            _renderer.sortingOrder = 0;
        }
        if (!_gun.IsFacingRight && _side == ArmSide.Left)
        {
            side = _rightSideGrip.position;
            _renderer.sortingOrder = 2;
        }

        Vector3 gripRotation = side - transform.position;
        float rotZ = Mathf.Atan2(gripRotation.y, gripRotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ + _offset);
    }

    private enum ArmSide
    {
        Left,
        Right
    }
}
