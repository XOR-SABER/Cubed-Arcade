using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
public class PlacementSystem : MonoBehaviour {
    [Header("Placement System")]
    [SerializeField]
    private bool isActive = true;
    [Header("Indicators")]
    [SerializeField]
    private GameObject mouseIndicator;
    [SerializeField]
    private GameObject cellIndicator;
    [Header("Controls")]
    [SerializeField]
    private GameObject camTrans;
    [SerializeField]
    private bool doMovement = true;
    [SerializeField]
	private float panSpeed = 30f;
    [SerializeField]
	private float scrollSpeed = 100f;
    [SerializeField]
	private float minDis = 10f;
    [SerializeField]
	private float maxDis = 50f;
    [Header("Camera")]
    [SerializeField]
    private Camera sceneCamera;
    [SerializeField]
    private CinemachineVirtualCamera vCam;
    [Header("Tilemap")]
    [SerializeField]
    private Tilemap tiles;
    [Range(0f, 1f)]
    [SerializeField]
    private float cellInterp = 0.1f;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private TileBase[] allTiles;
    private Vector2 lastPosition;
    private int currentIndex = 0;
    public enum Modes{
        PLACEMENT,
        ERASER,
        COPY,
    }
    public Modes currentMode;
    private void Awake() {
        lastPosition = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
        DiscordPresence.details = "Messing around with the level editor..";
        if(isActive == true) {
            Debug.Log("Build System Enabled..");
        }
    }
    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        lastPosition = sceneCamera.ScreenToWorldPoint(mousePosition);
        Vector2 mousePos = lastPosition;
        Vector3Int gridPosition = getGridPos(mousePos);
        Vector3 cellWorldPos = getCellWorldPos(gridPosition);
        mouseIndicator.transform.position = new Vector3(mousePos.x, mousePos.y, -1);
        cellIndicator.transform.position = Vector3.Lerp(cellIndicator.transform.position, cellWorldPos, cellInterp);
        handleInput(gridPosition);
    }
    // Helper functions
    private void handleInput(Vector3Int gridPosition) {
        cameraMovement();
        // Click stuff.. 
        if (!EventSystem.current.IsPointerOverGameObject()) {
            if (Input.GetMouseButton(0))
            {
                if(currentMode == Modes.PLACEMENT) placeTile(gridPosition, allTiles[currentIndex]);
                if(currentMode == Modes.ERASER) removeTile(gridPosition);
            }
            // if (Input.GetMouseButton(1))
            // {
            //     removeTile(gridPosition);
            // }
        }
    }
    private void cameraMovement()
    {
        if (!doMovement) return;
        Vector3 movement = Vector3.zero;
        if (Input.GetKey("w")) movement += Vector3.up;
        if (Input.GetKey("s")) movement += Vector3.down;
        if (Input.GetKey("d")) movement += Vector3.right;
        if (Input.GetKey("a")) movement += Vector3.left;

        // Couldn't use the Vcam transform directly.. so we made a empty game obj to do the thing.
        camTrans.transform.position += movement * panSpeed * Time.deltaTime;
        Vector3 pos = camTrans.transform.position;
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        vCam.m_Lens.OrthographicSize -=  scroll * 1000 * scrollSpeed * Time.deltaTime;
        vCam.m_Lens.OrthographicSize = Mathf.Clamp(vCam.m_Lens.OrthographicSize, minDis, maxDis);
        camTrans.transform.position = pos;
    }
    
    // Grid and Cell Functions
    public Vector3Int getGridPos(Vector3 mousePos) {
        return grid.WorldToCell(mousePos);
    }
    public Vector3 getCellWorldPos(Vector3Int GridPos) {
        return grid.CellToWorld(GridPos);
    }
    public bool checkTilePlaced(Vector3Int position)
    {
        TileBase tile = tiles.GetTile(position);
        return tile != null;
    }
    public bool placeTile(Vector3Int position, TileBase tileToPlace)
    {
        if (checkTilePlaced(position)) return false;
        tiles.SetTile(position, tileToPlace);
        return true;
    }
    public bool removeTile(Vector3Int position)
    {
        if (!checkTilePlaced(position)) return false;
        tiles.SetTile(position, null);
        return true;
    }
}

