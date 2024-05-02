using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    
    
    public GameObject mainMenuCamera;
    public GameObject mainMenuCanvas;
    public Animator startAnimation;

    public GameObject changeLogCamera;
    public GameObject settingsCamera;

    public float transitionDelay = 0f;

    public string gameScene;
    public string levelEditorScene;
    public string tutorialScene;
    private AudioManager _audioMan;
    // Start is called before the first frame updates
    void Start()
    {
        _audioMan = AudioManager.instance;
        startAnimation.SetTrigger("StartTransition");
        _audioMan.Play("MainMenuTheme");
    }

    public void OnClickLoadGameScene()
    {
        _audioMan.Play("ModeButtonSelect");
        SceneManager.LoadScene(gameScene);
    }

    public void OnClickLoadTutorialScene()
    {
        _audioMan.Play("ModeButtonSelect");
        SceneManager.LoadScene(tutorialScene);
    }
    
    public void OnClickLoadLevelEditorScene()
    {
        _audioMan.Play("ModeButtonSelect");
        SceneManager.LoadScene(levelEditorScene);
    }

    public void OnClickLoadChangeLog()
    {
        _audioMan.Play("ModeButtonSelect");
        mainMenuCanvas.SetActive(false);
        mainMenuCamera.SetActive(false);
        changeLogCamera.SetActive(true);
    }
    
    public void OnClickSettings()
    {
        _audioMan.Play("ModeButtonSelect");
        mainMenuCanvas.SetActive(false);
        mainMenuCamera.SetActive(false);
        settingsCamera.SetActive(true);
    }

    public void OnClickLoadMainMenu()
    {
        _audioMan.Play("ModeButtonSelect");
        changeLogCamera.SetActive(false);
        settingsCamera.SetActive(false);
        mainMenuCamera.SetActive(true);
        StartCoroutine(DelayTransition(transitionDelay));
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
