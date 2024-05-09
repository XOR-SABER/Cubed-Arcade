using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public string menuSceneName = "MainMenu";
    public GameObject ui;
    private AudioManager _audMan;
    private PlayerStats _playerStats;
    private bool _isPlaying = true;
    
    void Start() {
        _playerStats = PlayerStats.instance;
        _audMan = AudioManager.instance;
    }
    
    void Update()
    {
        if (_playerStats.isPlayerDead)
        {
            StopGame();
        }
    }

    private void StopGame()
    {
        if (_isPlaying)
        {
            _isPlaying = false;
            Toggle();
        }
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);
        Debug.Log(AudioManager.currentTrack);
        Debug.Log(ui.activeSelf);
        if(AudioManager.currentTrack != null) {
            if(ui.activeSelf) AudioManager.currentTrack.Pause();
            else AudioManager.currentTrack.UnPause();
        }
        // Setting the time scale down.
        //if (ui.activeSelf) Time.timeScale = 0f;
        //else Time.timeScale = 1f;
        
    }
    
    public void Retry()
    {
        _audMan.Play("ButtonDeConfirm");
        Toggle();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        _audMan.Play("ButtonDeConfirm");
        if(AudioManager.currentTrack != null) {
            AudioManager.currentTrack.Stop();
            AudioManager.currentTrack = null;
        }
        Toggle();
        SceneManager.LoadScene(menuSceneName);
    }
}
