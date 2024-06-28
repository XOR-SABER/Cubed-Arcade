using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnenyBullet : Bullet {
    //  ------------------ Public ------------------
    public int damage = 1;
    void OnTriggerEnter2D(Collider2D other) {
        _solidCheck(other);
        _playerCheck(other);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        _playerCheck(collision.collider);
    }

    protected override Transform _trackInit()
    {
        // Holy Fuck this is better... 
        if(!isTracking) return null;
        var _playerRef = PlayerStats.instance.getPlayerRef();
        if(_playerRef == null) {
            // Don't track if nothing is there.. 
            Debug.LogError("Player with the player tag does not exist in scene: Tracking disabled");
            isTracking = false;
            return null;
        } 
        return _playerRef.transform;
    }

    protected void _playerCheck(Collider2D other) {
        Debug.Log("Somthing happened here");
        if(!other.CompareTag("Player")) return;
        if(isMissle) createMissleExplosion();
        else {
            PlayerStats.instance.TakeDamage(damage);
            updatePierces(other);
        }
    }
}