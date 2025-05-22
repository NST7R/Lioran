using UnityEngine;

public class cameraController : MonoBehaviour
{
    [Header("Player Tracking")]
    [SerializeField] private Transform player;             // Player to follow
    [SerializeField] private float behindDistance;    // Horizontal lag behind the player
    [SerializeField] private float cameraSpeed;       // How smoothly the camera follows
    [SerializeField] private float verticalOffset;  // Camera height offset above player

    [Header("Wall Clamping")]
    [SerializeField] private LayerMask wallLayer;               // Layer assigned to walls
    [SerializeField] private float wallDetectionRange;   // How far to check for walls
    [SerializeField] private float wallPadding;          // Leave some space from wall edges

    private float lookBehind;
    private Vector3 lastPlayerPos;
    private Camera cam;

    private float minCamX, maxCamX;

    void Start()
    {
        cam = Camera.main;

        if (!player || !cam)
        {
            Debug.LogError("Player or Main Camera not assigned.");
            enabled = false;
            return;
        }

        lastPlayerPos = player.position;
    }

    void Update()
    {
        float horizontalInput = player.position.x - lastPlayerPos.x;
        lastPlayerPos = player.position;

        // Horizontal camera delay when moving
        if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            lookBehind = Mathf.Lerp(lookBehind, behindDistance * Mathf.Sign(horizontalInput), Time.deltaTime * cameraSpeed);
        }
        else
        {
            lookBehind = Mathf.Lerp(lookBehind, 0f, Time.deltaTime * cameraSpeed);
        }

        // Base target position before clamping
        float targetX = player.position.x + lookBehind;
        float targetY = player.position.y + verticalOffset;

        // Get half the width of the camera in world units
        float halfWidth = cam.orthographicSize * cam.aspect;

        // Detect walls and apply padding
        minCamX = GetWallEdge(-1) + halfWidth + wallPadding;
        maxCamX = GetWallEdge(1) - halfWidth - wallPadding;

        // Clamp X so camera stays within padded wall limits
        float clampedX = Mathf.Clamp(targetX, minCamX, maxCamX);

        // Smooth follow
        float newX = Mathf.Lerp(transform.position.x, clampedX, Time.deltaTime * cameraSpeed);
        float newY = Mathf.Lerp(transform.position.y, targetY, Time.deltaTime * cameraSpeed);

        transform.position = new Vector3(newX, newY, transform.position.z);
    }

    /// <summary>
    /// Detect wall position in a given direction
    /// </summary>
    float GetWallEdge(int dir)
    {
        Vector2 origin = new Vector2(player.position.x, player.position.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right * dir, wallDetectionRange, wallLayer);

        if (hit.collider != null)
        {
            return hit.point.x;
        }

        // Return a very far value if no wall is detected
        return dir == -1 ? -10000f : 10000f;
    }
}
