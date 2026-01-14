using UnityEngine;

public class CameraEdgeController : MonoBehaviour
{
    [Header("Edge Scrolling")]
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float edgeThreshold = 25f; // Pixels from the screen edge to trigger move

    [Header("Map Boundaries")]
    [SerializeField] private Vector2 mapBoundsMin;
    [SerializeField] private Vector2 mapBoundsMax;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSmoothSpeed = 10f;
    [SerializeField] private float minOrthographicSize = 3f;
    [SerializeField] private float maxOrthographicSize = 12f;
    [SerializeField] private float zoomSensitivity = 5f;

    private Camera cam;
    private float targetZoom;

    void Start()
    {
        cam = GetComponent<Camera>();
        targetZoom = cam.orthographicSize;
    }

    void LateUpdate()
    {
        HandleEdgeScrolling();
        HandleZoom();
    }

    void HandleEdgeScrolling()
    {
        Vector3 moveDir = Vector3.zero;

        // Check Horizontal Mouse Position
        if (Input.mousePosition.x >= Screen.width - edgeThreshold)
            moveDir.x += 1;
        else if (Input.mousePosition.x <= edgeThreshold)
            moveDir.x -= 1;

        // Check Vertical Mouse Position
        if (Input.mousePosition.y >= Screen.height - edgeThreshold)
            moveDir.y += 1;
        else if (Input.mousePosition.y <= edgeThreshold)
            moveDir.y -= 1;

        // Apply movement
        Vector3 targetPos = transform.position + (moveDir.normalized * moveSpeed * Time.deltaTime);

        // Clamp within map bounds
        float clampedX = Mathf.Clamp(targetPos.x, mapBoundsMin.x, mapBoundsMax.x);
        float clampedY = Mathf.Clamp(targetPos.y, mapBoundsMin.y, mapBoundsMax.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        targetZoom -= scrollInput * zoomSensitivity;
        targetZoom = Mathf.Clamp(targetZoom, minOrthographicSize, maxOrthographicSize);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSmoothSpeed * Time.deltaTime);
    }
}