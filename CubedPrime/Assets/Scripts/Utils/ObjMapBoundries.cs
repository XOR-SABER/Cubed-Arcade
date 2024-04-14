using UnityEngine;

public class ObjMapBoundries : MonoBehaviour {
    [Header("Map size")]
    [SerializeField]
    private Vector2 mapSize = new Vector2(256f, 128f); // Size of the map

    void LateUpdate()
    {
        Vector2 objectSize = new Vector2(transform.localScale.x, transform.localScale.y);
        Vector2 minBoundaries = -(mapSize / 2) + (objectSize / 2);
        Vector2 maxBoundaries = (mapSize / 2) - (objectSize / 2);

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBoundaries.x, maxBoundaries.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minBoundaries.y, maxBoundaries.y);
        transform.position = clampedPosition;
    }
}
