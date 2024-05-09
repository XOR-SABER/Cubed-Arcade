using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    
    private AudioManager audioMan;
    public GameObject mainMenuCamera;
    public GameObject mainMenuCanvas;
    public GameObject customizationCanvas;
    public Animator startAnimation;

    public GameObject changeLogCamera;
    public GameObject settingsCamera;
    public GameObject levelSelectorCamera;

    public float transitionDelay = 0f;

    public string gameScene;
    public string levelEditorScene;
    public string tutorialScene;
    
    // Start is called before the first frame update
    void Start()
    {
        customizationCanvas.SetActive(false);
        audioMan = FindObjectOfType<AudioManager>();
        startAnimation.SetTrigger("StartTransition");
        audioMan.Play("MainMenuTheme");
    }

    public void OnClickLoadGameScene()
    {
        audioMan.Play("ModeButtonSelect");
        audioMan.fadeOutCurrentTrack(1f);
        SceneManager.LoadScene(gameScene);
    }

    public void OnClickLoadTutorialScene()
    {
        audioMan.Play("ModeButtonSelect");
        audioMan.fadeOutCurrentTrack(1f);
        SceneManager.LoadScene(tutorialScene);
    }
    
    public void OnClickLoadLevelEditorScene()
    {
        audioMan.Play("ModeButtonSelect");
        audioMan.fadeOutCurrentTrack(1f);
        SceneManager.LoadScene(levelEditorScene);
    }

    public void OnClickLoadChangeLog()
    {
        audioMan.Play("ModeButtonSelect");
        mainMenuCanvas.SetActive(false);
        mainMenuCamera.SetActive(false);
        changeLogCamera.SetActive(true);
    }
    
    public void OnClickCustomization()
    {
        audioMan.Play("ModeButtonSelect");
        mainMenuCanvas.SetActive(false);
        customizationCanvas.SetActive(true);
    }
    
    public void OnClickLoadLevelSelector()
    {
        audioMan.Play("ModeButtonSelect");
        mainMenuCanvas.SetActive(false);
        mainMenuCamera.SetActive(false);
        levelSelectorCamera.SetActive(true);
    }
    
    public void OnClickSettings()
    {
        audioMan.Play("ModeButtonSelect");
        mainMenuCanvas.SetActive(false);
        mainMenuCamera.SetActive(false);
        settingsCamera.SetActive(true);
    }

    public void OnClickLoadMainMenu()
    {
        audioMan.Play("ModeButtonSelect");
        changeLogCamera.SetActive(false);
        settingsCamera.SetActive(false);
        levelSelectorCamera.SetActive(false);
        mainMenuCamera.SetActive(true);
        customizationCanvas.SetActive(false);
        StartCoroutine(DelayTransition(transitionDelay));
    }
    
    public void OnClickFastLoadMainMenu()
    {
        audioMan.Play("ModeButtonSelect");
        changeLogCamera.SetActive(false);
        settingsCamera.SetActive(false);
        levelSelectorCamera.SetActive(false);
        customizationCanvas.SetActive(false);
        mainMenuCamera.SetActive(true);
        mainMenuCanvas.SetActive(true);
    }

    IEnumerator DelayTransition(float delay)
    {
        yield return new WaitForSeconds(delay);
        mainMenuCanvas.SetActive(true);
    }

    public void OnClickQuitGame()
    {
        Application.Quit();
    }
}
