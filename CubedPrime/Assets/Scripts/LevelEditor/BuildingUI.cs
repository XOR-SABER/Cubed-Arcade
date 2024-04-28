using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingUI : MonoBehaviour
{
    [SerializeField]
    private PlacementSystem pSys;
    [SerializeField]
    private GameObject GlobalSettingsUI;
    [SerializeField]
    private GameObject AssetsUI;
    [SerializeField]
    private GameObject GridVisualizer;
    private bool isGridActive = true;
    private bool isGlobalSettingsActive = false;
    private bool isAssetUIActive = false;
    private Animator UIAnimator;
    [SerializeField]
    private TypingText statusText;
    private static string currentStatus = "Welcome to the level editor!";
    private static bool isTextUpdated = false;
    [SerializeField]
    private Sprite[] icons;
    public static void setTextStatus(string newStatus) {
        if(isTextUpdated) isTextUpdated = false;
        currentStatus = newStatus;
    }   
    void Start() { 
        UIAnimator = GetComponent<Animator>();
        Cursor.SetCursor(icons[0].texture, Vector2.zero, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isTextUpdated) {
            isTextUpdated = true;
            statusText.startType(currentStatus);
        }
    }
    public void setCursorIcon(int index) {
        Cursor.SetCursor(icons[index % icons.Length].texture, Vector2.zero, CursorMode.Auto);
    }
    public void setVis(bool value) {
        UIAnimator.SetBool("inPlaymode", !value);
        if(isAssetUIActive) toggleAssetUI();
        if(isGlobalSettingsActive) toggleGridUI();
    }
    public void setErase() {
        
        pSys.currentMode = PlacementSystem.Modes.ERASER;
        setCursorIcon((int)PlacementSystem.Modes.ERASER);
        setTextStatus("Eraser Tool Selected!");
        
    }
    public void setPlacement() {
        pSys.currentMode = PlacementSystem.Modes.PLACEMENT;
        setCursorIcon((int)PlacementSystem.Modes.PLACEMENT);
        setTextStatus("Placement Tool Selected!");
    }
    public void setCopy() {
        pSys.currentMode = PlacementSystem.Modes.COPY;
        setCursorIcon((int)PlacementSystem.Modes.COPY);
        setTextStatus("Copy Tool Selected!");
    }
    public void setPlay() {
        
    }
    
    public void toggleGrid() { 
        isGridActive = !isGridActive;
        GridVisualizer.SetActive(isGridActive);
    }   
    public void toggleGridUI() { 
        isGlobalSettingsActive = !isGlobalSettingsActive;
        GlobalSettingsUI.SetActive(isGlobalSettingsActive);
        if(isAssetUIActive) toggleAssetUI();
    }   
    public void toggleAssetUI() {
        isAssetUIActive = !isAssetUIActive;
        AssetsUI.SetActive(isAssetUIActive);
        if(isGlobalSettingsActive) toggleGridUI();
    }
}
