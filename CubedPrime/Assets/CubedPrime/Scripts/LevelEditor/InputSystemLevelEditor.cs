using UnityEngine;

public class InputSystemLevelEditor : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;
    private Vector3 lastPosition = default;
    [SerializeField]
    private LayerMask placementLayermask; 

    public Vector3 GetSelectedMapPos()
    {
        Vector3 mousePosition = Input.mousePosition;
        // mousePos.z = sceneCamera.nearClipPlane;
        Vector2 mousePos2D = sceneCamera.ScreenToWorldPoint(mousePosition); // Convert to 2D world point

        // RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 0, placementLayermask); // Use 2D raycast
        // if (hit.collider != null)
        // {
        //     lastPosition = hit.point;
        //     Debug.Log("Hit point: " + lastPosition);
        // }
        // else
        // {
        //     Debug.Log("No hit detected.");
        // }

        // Debug.Log("Mouse position: " + mousePos2D);
        // Debug.Log("Last position: " + lastPosition);
        lastPosition = mousePos2D;
        return lastPosition;
    }
}
 