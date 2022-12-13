using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _vCam;
    private CinemachineBasicMultiChannelPerlin _noisePerlin;

    private void Awake()
    {
        _vCam = GetComponent<CinemachineVirtualCamera>();
        _noisePerlin = _vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        StopShake();
    }

    private void Start()
    {
        SetCameraFollow();
        CreateConfinerCollider();
    }

    private void OnEnable()
    {
        CameraEvents.CameraShake += StartShake;
    }

    private void OnDisable()
    {
        CameraEvents.CameraShake -= StartShake;
    }

    /// <summary>
    /// Shakes the camera with a fixed duration and force.
    /// </summary>
    /// <param name="shakeTime">The duration of the shake.</param>
    /// <param name="force">The force of the shake.</param>
    private void StartShake(float shakeTime, float force)
    {
        if (_noisePerlin == null)
        {
            Debug.LogError("Camera Controller ERROR : Cinemachine noise profile needs to be set.");
            return;
        }

        _noisePerlin.m_AmplitudeGain = force / 10;
        _noisePerlin.m_FrequencyGain = force;

        CancelInvoke("StopShake");
        Invoke("StopShake", shakeTime);
    }

    /// <summary>
    /// Stops shaking the camera.
    /// </summary>
    private void StopShake()
    {
        _noisePerlin.m_AmplitudeGain = 0f;
        _noisePerlin.m_FrequencyGain = 0f;
    }

    /// <summary>
    /// Set the follow and look at properties of the cinemachine component.
    /// </summary>
    private void SetCameraFollow()
    {
        _vCam.Follow = GameManager.Instance.Player.transform;
        _vCam.LookAt = GameManager.Instance.Player.transform;
    }

    private void CreateConfinerCollider()
    {
        GameObject confiner = new GameObject("Confiner");
        confiner.AddComponent<PolygonCollider2D>();
        confiner.layer = LayerMask.NameToLayer("Confiner");

        PolygonCollider2D boundary = confiner.GetComponent<PolygonCollider2D>();
        Tilemap tileMap = FindObjectOfType<Tilemap>();
        tileMap.CompressBounds();

        Vector2[] path = new Vector2[4];

        path[0] = new Vector2(tileMap.cellBounds.xMin, tileMap.cellBounds.yMax);
        path[1] = new Vector2(tileMap.cellBounds.xMin, tileMap.cellBounds.yMin);
        path[2] = new Vector2(tileMap.cellBounds.xMax, tileMap.cellBounds.yMin);
        path[3] = new Vector2(tileMap.cellBounds.xMax, tileMap.cellBounds.yMax);
        boundary.pathCount = 1;
        boundary.SetPath(0, path);
        boundary.isTrigger = true;

        CinemachineConfiner2D cinemachineConfiner = _vCam.GetComponent<CinemachineConfiner2D>();

        cinemachineConfiner.m_BoundingShape2D = boundary;
    }
}
