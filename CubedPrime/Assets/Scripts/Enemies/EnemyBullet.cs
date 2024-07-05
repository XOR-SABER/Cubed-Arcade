using UnityEngine;

public class EnenyBullet : Bullet {
    void OnTriggerEnter2D(Collider2D other) {
        _solidCheck(other);
        _playerCheck(other);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        _playerCheck(collision.collider);
    }

    protected override Transform _trackInit() {
        // Holy Fuck this is better... 
        if(!stats.isTracking) return null;
        var _playerRef = PlayerStats.instance.getPlayerRef();
        if(_playerRef == null) {
            // Don't track if nothing is there.. 
            Debug.LogError("Player with the player tag does not exist in scene: Tracking disabled");
            stats.isTracking = false;
            return null;
        } 
        return _playerRef.transform;
    }

    protected void _playerCheck(Collider2D other) {
        if(!other.CompareTag("Player")) return;
        if(stats.isExplosive) createMissleExplosion();
        else {
            PlayerStats.instance.TakeDamage(stats.playerDamage);
            updatePierces(other);
        }
    }
}