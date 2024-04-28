using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Minimap : MonoBehaviour
{
    public float angle;
    private Transform _player;

    private GameObject[] _enemies;

    public Transform minimapOverlay;

    private Transform _offset;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _player.position;
        HandleEnemyVisible();
        RotateOverlay();
    }

    private void HandleEnemyVisible()
    {
        for (int i = 0; i < _enemies.Length; i++)
        {
            _enemies[i].SetActive(Vector3.Angle(_player.transform.up, _enemies[i].transform.position - _player.position) <= angle);
        }
    }

    private void RotateOverlay()
    {
        minimapOverlay.localRotation = Quaternion.Euler(0, 0, (_player.eulerAngles.z - angle));
    }
}
