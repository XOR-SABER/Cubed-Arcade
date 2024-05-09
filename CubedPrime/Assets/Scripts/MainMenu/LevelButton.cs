using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    private LevelSelector _levelSelector;
    private Level _level;

    private void Start()
    {
        _levelSelector = FindObjectOfType<LevelSelector>();
    }

    public void OnClickLoadLevel()
    {
        _levelSelector.LoadLevel(_level);
    }

    public void SetLevel(Level level)
    {
        _level = level;
    }
}
