using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    [Header("Zoom")]
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 20f;

    [Header("Pan")]
    public float panSpeed = 1f;

    [Header("Map Settings")]
    public float singleMapWidth;   // Width of ONE map piece
    public float mapMinY;
    public float mapMaxY;

    private float wrapHalfWidth;   // Half of effective wrap span
    private Camera cam;
    private Vector3 lastMousePos;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        // We have 3 maps (Left, Center, Right)
        // Wrapping happens at +/- half of total width.
        wrapHalfWidth = singleMapWidth / 2f;
    }

    void Update()
    {
        HandleZoom();
        HandlePan();
        HandleHorizontalWrap();
        ClampVerticalProperly();
    }

    // ----------------------------------------------------------
    // ZOOM (Safe: no flicker)
    // ----------------------------------------------------------
    void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (scroll == 0)
            return;

        float previousSize = cam.orthographicSize;

        // Apply zoom
        cam.orthographicSize -= scroll * zoomSpeed * Time.deltaTime;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);

        // Zoom toward cursor
        Vector3 worldBefore = cam.ScreenToWorldPoint(Input.mousePosition);
        float ratio = cam.orthographicSize / previousSize;
        Vector3 worldAfter = cam.ScreenToWorldPoint(Input.mousePosition);

        // Move camera to keep cursor stable
        transform.position += worldBefore - worldAfter;
    }

    // ----------------------------------------------------------
    // DRAG TO PAN
    // ----------------------------------------------------------
    void HandlePan()
    {
        if (Input.GetMouseButtonDown(0))
            lastMousePos = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            Vector3 delta =
                cam.ScreenToWorldPoint(lastMousePos) -
                cam.ScreenToWorldPoint(Input.mousePosition);

            transform.position += delta * panSpeed;
            lastMousePos = Input.mousePosition;
        }
    }

    // ----------------------------------------------------------
    // INFINITE HORIZONTAL WRAP (no clamp)
    // ----------------------------------------------------------
    void HandleHorizontalWrap()
    {
        float camX = transform.position.x;

        // If camera goes outside half width, shift it
        if (camX > wrapHalfWidth)
            transform.position -= new Vector3(singleMapWidth, 0f, 0f);

        else if (camX < -wrapHalfWidth)
            transform.position += new Vector3(singleMapWidth, 0f, 0f);
    }

    // ----------------------------------------------------------
    // SAFE VERTICAL CLAMP (prevents flicker)
    // ----------------------------------------------------------
    void ClampVerticalProperly()
    {
        float camHeight = cam.orthographicSize;
        float maxY = mapMaxY - camHeight;
        float minY = mapMinY + camHeight;

        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

        // ❗ Only modify Y — X stays untouched (important for wrapping)
        transform.position = new Vector3(
            transform.position.x,
            clampedY,
            transform.position.z
        );
    }
}
