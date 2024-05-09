using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverInfoManager : MonoBehaviour
{
    private PlayerStats _playerStats;
    public AnimationCurve curve;
    public float scaleTime;
    public List<PointsAnimation> pointsAnimations;
    private List<int> _playerInformation = new List<int>();
    private int _currentIndex = 0;
    public float transitionTime = 1f;
    private void Awake()
    {
        _playerStats = PlayerStats.instance;
        _playerInformation.Add(_playerStats.points);
        _playerInformation.Add(_playerStats.TotalHealed);
        _playerInformation.Add(_playerStats.TotalDamageDealt);
        _playerInformation.Add(_playerStats.TotalDamageTaken);
        _playerInformation.Add(_playerStats.TotalEnemiesKilled);
        _playerInformation.Add(_playerStats.TotalShotsTaken);
        _playerInformation.Add(_playerStats.TotalShotsHit);
        NextText();
    }

    public void NextText()
    {
        if (_currentIndex < pointsAnimations.Count)
        {
            _currentIndex++;
            pointsAnimations[_currentIndex - 1].Type(_playerInformation[_currentIndex - 1], scaleTime, curve, transitionTime);
        }
    }
}
