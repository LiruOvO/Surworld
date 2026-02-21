using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CameraZoom : MonoBehaviour
{
    private CinemachineCamera vCam;
    public GameObject shopPanel; // ѕерет€гни сюди панель магазину в ≥нспектор≥

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
        // якщо магазин в≥дкритий ≤ миша знаходитьс€ над ним Ч зум не працюЇ
        if (shopPanel != null && shopPanel.activeInHierarchy && IsMouseOverShop())
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

    // ƒопом≥жна функц≥€, €ка перев≥р€Ї, чи миша саме над магазином
    private bool IsMouseOverShop()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            // ѕерев≥р€Їмо, чи Ї серед об'Їкт≥в п≥д мишею наш магазин
            if (result.gameObject == shopPanel || result.gameObject.transform.IsChildOf(shopPanel.transform))
            {
                return true;
            }
        }
        return false;
    }
}