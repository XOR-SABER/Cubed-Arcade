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
        _sceneSw = SceneSwitcher.instance;
        _audMan = AudioManager.instance;
        _audMan.fadeInNewTrack(_sceneSw.levels[_sceneSw.levelToGoto].levelTheme, 1f);
        topTextField.startType(_sceneSw.levels[_sceneSw.levelToGoto].GameMode);
        bottomTextField.startType(_sceneSw.levels[_sceneSw.levelToGoto].LevelName);
        DiscordPresence.state = "Loading into " + _sceneSw.levels[_sceneSw.levelToGoto].LevelName;
        StartCoroutine(loadTimer(7.5f));
    }

    IEnumerator loadTimer(float sec) {
        yield return new WaitForSeconds(sec);
        // Load to recRoom for now.. 
        _sceneSw.FadeTo(_sceneSw.levels[_sceneSw.levelToGoto].SceneName);
    }
}
