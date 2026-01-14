using UnityEngine;

public class CameraMouseFollow : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector2 mapBoundsMin;
    [SerializeField] private Vector2 mapBoundsMax;

    [Header("Tracking Mode")]
    [Range(0.1f, 1f)]
    [SerializeField] private float mouseInfluence = 0.5f; // How far the camera leans toward the mouse

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        // 1. Get the mouse position in world space
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z; // Keep the camera's original Z depth

        // 2. Calculate the target position
        // We blend between the current position and the mouse position based on influence
        Vector3 targetPos = Vector3.Lerp(transform.position, mousePos, mouseInfluence);

        // 3. Apply Smoothing
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);

        // 4. Clamp the movement so the camera doesn't show the "void" outside your map
        float clampedX = Mathf.Clamp(smoothedPosition.x, mapBoundsMin.x, mapBoundsMax.x);
        float clampedY = Mathf.Clamp(smoothedPosition.y, mapBoundsMin.y, mapBoundsMax.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}