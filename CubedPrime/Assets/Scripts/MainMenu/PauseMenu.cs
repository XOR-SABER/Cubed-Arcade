using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public string menuSceneName = "MainMenu";
    public GameObject ui;
    private AudioManager _audMan;

    private PlayersControls _playerControls;
    private InputAction _pause;
    void Start() {
        _audMan = AudioManager.instance;
    }

    private void Awake()
    {
        _playerControls = new PlayersControls();
        _pause = _playerControls.Player.GoToPauseMenu;
        _pause.Enable();
        _pause.performed += _ => Toggle();
    }

    private void OnEnable()
    {
        _pause.Enable();
    }

    private void OnDisable()
    {
        _pause.Disable();
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Toggle();
        }*/
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
        if (ui.activeSelf) Time.timeScale = 0f;
        else Time.timeScale = 1f;
        
    }

    public void Continue()
    {
        _audMan.Play("ButtonDeConfirm");
        Toggle();
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