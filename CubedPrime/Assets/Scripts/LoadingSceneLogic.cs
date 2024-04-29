using System.Collections;
using Scripts;
using UnityEngine;

public class LoadingSceneLogic : MonoBehaviour
{
    public SceneSwitcher sceneSw;
    public AudioManager audMan;
    public TypingText bottomTextField;
    public TypingText topTextField;
    // Start is called before the first frame update
    void Start()
    {
        sceneSw = SceneSwitcher.instance;
        audMan = AudioManager.instance;
        audMan.fadeInNewTrack(sceneSw.levels[sceneSw.levelToGoto].levelTheme, 1f);
        topTextField.startType(sceneSw.levels[sceneSw.levelToGoto].GameMode);
        bottomTextField.startType(sceneSw.levels[sceneSw.levelToGoto].LevelName);
        DiscordPresence.state = "Loading into " + sceneSw.levels[sceneSw.levelToGoto].LevelName;
        StartCoroutine(loadTimer(7.5f));
    }

    IEnumerator loadTimer(float sec) {
        yield return new WaitForSeconds(sec);
        // Load to recRoom for now.. 
        sceneSw.FadeTo(sceneSw.levels[sceneSw.levelToGoto].SceneName);
    }
}
