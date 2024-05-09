using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public List<Level> levels = new List<Level>();
    public GameObject levelUIPrefab;
    public Transform levelHandler;
    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    private RectTransform _sampleListItem;
    public VerticalLayoutGroup verticalLayoutGroup;

    public float snapSpeedShreshold = 50;
    public float snapForce;
    private float _snapSpeed;
    private bool _isSnapped;

    private int _currentIndex = 0;
    private AudioManager audioMan;

    public TypingText nameText;
    public TypingText descriptionText;

    private SceneSwitcher _sceneSwitcher;
    void Start()
    {
        audioMan = FindObjectOfType<AudioManager>();
        LoadLevels();
        _sampleListItem = levelUIPrefab.GetComponent<RectTransform>();
        _isSnapped = false;
        UpdateDisplay();
        
        _sceneSwitcher = SceneSwitcher.instance;
    }
    
    private void LoadLevels()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            GameObject level = Instantiate(levelUIPrefab, levelHandler);
            level.GetComponentInChildren<TMP_Text>().text = (i + 1).ToString();
            level.GetComponent<LevelButton>().SetLevel(levels[i]);
        }
    }

    private void Update()
    {
        if (levels.Count != 0)
        {
            if (!Input.GetKey(KeyCode.Mouse0))
            {
                int index = Mathf.RoundToInt(contentPanel.localPosition.y / 
                                             (_sampleListItem.rect.height + verticalLayoutGroup.spacing));
                index = Math.Clamp(index, 0, levels.Count - 1);
                if (index != _currentIndex)
                {
                    _currentIndex = index;
                    UpdateDisplay();
                }
                Snap();
            }
        }
        
    }

    private void Snap()
    {
        if (scrollRect.velocity.magnitude < snapSpeedShreshold && !_isSnapped)
        {
            scrollRect.velocity = Vector2.zero;
            _snapSpeed += snapForce * Time.deltaTime;
            contentPanel.localPosition =
                new Vector3(contentPanel.localPosition.x, 
                    Mathf.MoveTowards(contentPanel.localPosition.y, 
                        _currentIndex * (_sampleListItem.rect.height + verticalLayoutGroup.spacing), _snapSpeed), 
                    contentPanel.localPosition.z);

            if (Mathf.Approximately(contentPanel.localPosition.y, _currentIndex * (_sampleListItem.rect.height + verticalLayoutGroup.spacing)))
            {
                _isSnapped = true;
            }
        }
        else
        {
            _isSnapped = false;
            _snapSpeed = 0;
        }
    }

    private void UpdateDisplay()
    {
        Level level = levels[_currentIndex];
        nameText.RestartType(level.levelName);
        descriptionText.RestartType(level.levelDescription);
    }

    public void LoadLevel(Level level)
    {
        audioMan.Play("ModeButtonSelect");
        audioMan.fadeOutCurrentTrack(1f);
        _sceneSwitcher.levelToLoad = level;
        _sceneSwitcher.FadeTo("LoadingScene1");
    }
}
