using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    [Header("References")]
    public Transform player; // The player's transform
    public Camera minimapCamera; // The minimap camera
    public RectTransform mapRect; // The RectTransform of the minimap UI element

    private Transform target; // The world object this icon should track

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void LateUpdate()
    {
        if (target == null || player == null || minimapCamera == null || mapRect == null)
        {
            // Disable ourselves if anything is missing
            gameObject.SetActive(false);
            return;
        }

        // Calculate the position of the target relative to the player
        Vector3 positionDelta = target.position - player.position;

        // Rotate the delta so that "forward" is always up on the minimap
        positionDelta = Quaternion.Euler(0, -player.eulerAngles.y, 0) * positionDelta;

        // Calculate the position on the map based on camera zoom
        float mapScale = minimapCamera.orthographicSize * 2f;
        Vector2 mapPosition = new Vector2(positionDelta.x / mapScale, positionDelta.z / mapScale);

        // Clamp the icon to the edges of the minimap if the target is far away
        mapPosition = Vector2.ClampMagnitude(mapPosition, 0.5f);

        // Apply the final position to the icon's RectTransform
        RectTransform iconRect = GetComponent<RectTransform>();
        iconRect.anchoredPosition = new Vector2(mapPosition.x * mapRect.rect.width, mapPosition.y * mapRect.rect.height);
    }
}