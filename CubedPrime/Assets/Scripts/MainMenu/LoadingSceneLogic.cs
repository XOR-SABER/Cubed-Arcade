using System.Collections;
using Scripts;
using UnityEngine;

public class LoadingSceneLogic : MonoBehaviour
{

    public TypingText bottomTextField;
    public TypingText topTextField;
    private SceneSwitcher _sceneSw;
    private AudioManager _audMan;
    // Start is called before the first frame update
    void Start()
    {
        _audMan = AudioManager.instance;
        _sceneSw = SceneSwitcher.instance;
        _audMan.fadeInNewTrack(_sceneSw.levelToLoad.levelTheme, 1f);
        topTextField.startType(_sceneSw.levelToLoad.gameMode);
        bottomTextField.startType(_sceneSw.levelToLoad.levelName);
        DiscordPresence.details = _sceneSw.levelToLoad.sceneName;
        DiscordPresence.state = "Loading into " + _sceneSw.levelToLoad.sceneName;
        
        StartCoroutine(loadTimer(7.5f));
    }

    IEnumerator loadTimer(float sec) {
        yield return new WaitForSeconds(sec);
        // Load to recRoom for now.. 
        _sceneSw.FadeTo(_sceneSw.levelToLoad.sceneName);
    }
}
