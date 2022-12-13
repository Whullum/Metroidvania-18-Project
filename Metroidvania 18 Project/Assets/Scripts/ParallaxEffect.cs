using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private Transform _cameraTransform;
    private Vector3 _lastCameraPosition;
    private float _textureSizeX;
    private float _textureSizeY;
    private bool _reset;

    [SerializeField] private Vector2 _effectMultiplier;
    [SerializeField] private bool _horizontalEffect;
    [SerializeField] private bool _verticalEffect;

    private void Start() => InitializeParallax();

    private void LateUpdate() => Parallax();

    private void InitializeParallax()
    {
        _cameraTransform = Camera.main.transform;
        _lastCameraPosition = Vector3.zero;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Texture2D texture = renderer.sprite.texture;

        _textureSizeX = texture.width / renderer.sprite.pixelsPerUnit;
        _textureSizeY = texture.height / renderer.sprite.pixelsPerUnit;
    }

    private void Parallax()
    {
        Vector3 deltaMovement = _cameraTransform.position - _lastCameraPosition; // How much the camera has moved since the last frame.

        transform.position += new Vector3(deltaMovement.x * _effectMultiplier.x, deltaMovement.y * _effectMultiplier.y);
        _lastCameraPosition = _cameraTransform.position;

        if (_horizontalEffect)
        {
            if (Mathf.Abs(_cameraTransform.position.x - transform.position.x) >= _textureSizeX)
            {
                float offsetPositionX = (_cameraTransform.position.x - transform.position.x) % _textureSizeX;
                transform.position = new Vector3(_cameraTransform.position.x + offsetPositionX, transform.position.y);
            }
        }

        if (_verticalEffect)
        {
            if (Mathf.Abs(_cameraTransform.position.y - transform.position.y) >= _textureSizeY)
            {
                float offsetPositionY = (_cameraTransform.position.y - transform.position.y) % _textureSizeY;
                transform.position = new Vector3(transform.position.x, _cameraTransform.position.y + offsetPositionY);
            }
        }
    }
}
