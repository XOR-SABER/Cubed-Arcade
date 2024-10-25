using UnityEngine;

public class PlayerBullet : Bullet
{
    //  ------------------ Private ------------------
    private bool _isHit = false; 
    void OnTriggerEnter2D(Collider2D other)
    {
        _enemyCheck(other);
        _bossCheck(other);
        _barrelCheck(other);
        _solidCheck(other);
    }
    private void _enemyCheck(Collider2D other) {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null) return;
        _isHit = true;
        _isEntityHit = true;
        enemy.TakeDamage(stats.enemyDamage);
        PlayerStats.instance.TotalDamageDealt+= stats.enemyDamage;
        updatePierces(other);
    }
    public override void updatePierces(Collider2D other) {
        numberOfPierces--;
        if(stats.isBouncy) handleBounce(other);
        if(numberOfPierces > 0) return;
        if(_isHit) {
            _isHit = false;
            PlayerStats.instance.TotalShotsHit++;
        }
        // Spawn the partcles for missles.. 
        if(stats.isExplosive) {
            Instantiate(stats.explosionOBJ, transform.position, Quaternion.Inverse(transform.rotation));
            Destroy(gameObject);
            return;
        }


        if (_isEntityHit) Instantiate(stats.bloodParticles, transform.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 180)));
        else Instantiate(stats.wallParticles, transform.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 180)));
        Destroy(gameObject);
    }

    // Todo: Refactor Boss to use enemy
    private void _bossCheck(Collider2D other) {
        Boss1Segments boss = other.GetComponent<Boss1Segments>();
        if(boss == null) return;
        _isHit = true;
        _isEntityHit = true;
        boss.TakeDamage(stats.enemyDamage);
        PlayerStats.instance.TotalDamageDealt+= stats.enemyDamage;
        updatePierces(other);
    }
    private void _barrelCheck(Collider2D other) {
        explosiveBarrel body = other.GetComponent<explosiveBarrel>();
        if(body == null) return;
        _isHit = true;
        _isEntityHit = false;
        body.destroyBarrel();
        updatePierces(other);
    }
}

