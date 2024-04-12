using Unity.Mathematics;
using UnityEngine;
public class PlacementSystem : MonoBehaviour {
    [SerializeField]
    private GameObject mouseIndicator, cellIndicator; 
    [SerializeField]
    private InputSystemLevelEditor inputManager;
    [SerializeField]
    private Grid grid;
    [Range(0f, 1f)]
    public float cellInterp = 0.1f;
    private void Update() {
        Vector3 mousePos = inputManager.GetSelectedMapPos();
        Vector3Int gridPosition = grid.WorldToCell(mousePos);
        Vector3 cellWorldPos = grid.CellToWorld(gridPosition);
        mouseIndicator.transform.position = mousePos;
        // cellIndicator.transform.position = cellWorldPos;
        cellIndicator.transform.position = Vector3.Lerp(cellIndicator.transform.position, cellWorldPos, cellInterp);
    } 
}