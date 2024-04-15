using System;
using UnityEngine;

public class BuildingUI : MonoBehaviour
{
    [SerializeField]
    private LevelEditorCursor cursor;
    [SerializeField]
    private PlacementSystem pSys;
    private Animator cursorAnimator; 
    private Animator UIAnimator;
    [SerializeField]
    private TypingText statusText;
    private static string currentStatus = "Welcome to the level editor!";
    private static bool isTextUpdated = false;
    public Sprite[] cursorIcons;
    
    public static void setTextStatus(string newStatus) {
        if(isTextUpdated) isTextUpdated = false;
        currentStatus = newStatus;
    }   
    void Start() { 
        cursorAnimator = cursor.GetComponent<Animator>();
        UIAnimator = GetComponent<Animator>();
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
        cursorAnimator.SetInteger("Cursor", index % cursorIcons.Length);
    }
    public void setVis(bool value) {
        UIAnimator.SetBool("inPlaymode", !value);
    }
    public void setErase() {
        pSys.currentMode = PlacementSystem.Modes.ERASER;
        setCursorIcon((int)PlacementSystem.Modes.ERASER);
        
    }
    public void setPlacement() {
        pSys.currentMode = PlacementSystem.Modes.PLACEMENT;
        setCursorIcon((int)PlacementSystem.Modes.PLACEMENT);
    }
    public void setCopy() {
        pSys.currentMode = PlacementSystem.Modes.COPY;
        setCursorIcon((int)PlacementSystem.Modes.COPY);
    }
    public void setPlay() {
        
    }
}
