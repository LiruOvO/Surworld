using UnityEngine;
using Unity.Cinemachine; // Для Unity 6 використовуємо цей namespace

public class CameraZoom : MonoBehaviour
{
    private CinemachineCamera vCam;

    [Header("Zoom Settings")]
    public float minSize = 3f;     
    public float maxSize = 10f;    
    public float zoomSensitivity = 1f; 
    private float targetSize;

    void Start()
    {
        vCam = GetComponent<CinemachineCamera>();
        targetSize = vCam.Lens.OrthographicSize;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            targetSize -= scroll * zoomSensitivity;
            targetSize = Mathf.Clamp(targetSize, minSize, maxSize);
        }

        // Плавно змінюємо розмір до цільового
        vCam.Lens.OrthographicSize = Mathf.Lerp(vCam.Lens.OrthographicSize, targetSize, Time.deltaTime * 10f);
    }
}