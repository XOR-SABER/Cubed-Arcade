using UnityEngine;
public class PlacementSystem : MonoBehaviour {
    [SerializeField]
    private GameObject mouseIndicator, cellIndicator; 
    [SerializeField]
    private InputSystemLevelEditor inputManager;
    [SerializeField]
    private Grid grid;
    private void Update() {
        Vector3 mousePos = inputManager.GetSelectedMapPos();
        Vector3Int gridPosition = grid.WorldToCell(mousePos);
        Vector3 cellWorldPos = grid.CellToWorld(gridPosition);

        // Debug.Log("Mouse Position: " + mousePos);
        // Debug.Log("Grid Position: " + gridPosition);
        // Debug.Log("Cell World Position: " + cellWorldPos);

        mouseIndicator.transform.position = mousePos;
        cellIndicator.transform.position = cellWorldPos;
    } 
}