using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CameraZoom : MonoBehaviour
{
    private CinemachineCamera vCam;
    public List<GameObject> uiPanels = new List<GameObject>(); // —писок ю≥ де зум блокувати

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
        // якщо магазин ю≥ ≤ миша знаходитьс€ над ним Ч зум не працюЇ
        if (IsMouseOverAnyPanel())
        {
            return;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            targetSize -= scroll * zoomSensitivity;
            targetSize = Mathf.Clamp(targetSize, minSize, maxSize);
        }

        vCam.Lens.OrthographicSize = Mathf.Lerp(vCam.Lens.OrthographicSize, targetSize, Time.deltaTime * 10f);
    }

    // ƒопом≥жна функц≥€, €ка перев≥р€Ї, чи миша саме над ю≥
    private bool IsMouseOverAnyPanel()
    {
        // якщо кл≥кнули або крутимо над UI
        if (EventSystem.current == null) return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            foreach (var panel in uiPanels)
            {
                // ѕерев≥р€Їмо, чи панель ≥снуЇ, чи вона активна ≥ чи миша над нею (або њњ д≥тьми)
                if (panel != null && panel.activeInHierarchy)
                {
                    if (result.gameObject == panel || result.gameObject.transform.IsChildOf(panel.transform))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}