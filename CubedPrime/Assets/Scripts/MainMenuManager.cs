using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    
    public AudioManager audioMan;
    public GameObject mainMenuCamera;
    public GameObject mainMenuCanvas;
    public Animator startAnimation;

    public GameObject changeLogCamera;

    public float transitionDelay = 0f;

    public string gameScene;
    public string levelEditorScene;
    public string tutorialScene;
    
    // Start is called before the first frame update
    void Start()
    {
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
