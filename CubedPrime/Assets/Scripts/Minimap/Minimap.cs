using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Minimap : MonoBehaviour
{
    public float angle;
    private Transform _player;
    public Transform minimapOverlay;
    private Transform _offset;
    private float _prevZ;
    // Start is called before the first frame update
    void Start()
    {
        _prevZ = transform.position.z;
        _player = PlayerStats.instance.getPlayerRef().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerStats.instance.isPlayerDead) return;
        if(_player == null) return;
        transform.position = new Vector3(_player.position.x, _player.position.y, _prevZ);
        RotateOverlay();
    }

    private void RotateOverlay()
    {
        minimapOverlay.localRotation = Quaternion.Euler(0, 0, (_player.eulerAngles.z - angle));
    }
}
