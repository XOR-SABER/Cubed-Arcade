using System.Collections;
using Scripts;
using UnityEngine;
public class MainMenuManager : MonoBehaviour
{
    
    public AudioManager audioMan;
    public GameObject mainMenuCamera;
    public GameObject mainMenuCanvas;
    public Animator startAnimation;
    public SceneSwitcher sceneSwitch;

    public GameObject changeLogCamera;

    public float transitionDelay = 0f;

    public string gameScene;
    public string levelEditorScene;
    public string tutorialScene;
    
    // Start is called before the first frame update
    void Start()
    {
        startAnimation.SetTrigger("StartTransition");
        audioMan.fadeInNewTrack("MainMenuTheme", 0.5f);
    }

    public void OnClickLoadGameScene()
    {
        audioMan.Play("ModeButtonSelect");
        audioMan.fadeOutCurrentTrack(1f);
        sceneSwitch.FadeTo(gameScene);
    }

    public void OnClickLoadTutorialScene()
    {
        audioMan.Play("ModeButtonSelect");
        audioMan.fadeOutCurrentTrack(1f);
        sceneSwitch.FadeTo(tutorialScene);
    }
    
    public void OnClickLoadLevelEditorScene()
    {
        audioMan.Play("ModeButtonSelect");
        audioMan.fadeOutCurrentTrack(1f);
        sceneSwitch.FadeTo(levelEditorScene);
    }

    public void OnClickLoadChangeLog()
    {
        audioMan.Play("ModeButtonSelect");
        mainMenuCanvas.SetActive(false);
        mainMenuCamera.SetActive(false);
        changeLogCamera.SetActive(true);
    }

    public void OnClickLoadMainMenu()
    {
        audioMan.Play("ModeButtonSelect");
        changeLogCamera.SetActive(false);
        mainMenuCamera.SetActive(true);
        StartCoroutine(DelayTransition(transitionDelay));
    }

    IEnumerator DelayTransition(float delay)
    {
        yield return new WaitForSeconds(delay);
        mainMenuCanvas.SetActive(true);
    }
}
