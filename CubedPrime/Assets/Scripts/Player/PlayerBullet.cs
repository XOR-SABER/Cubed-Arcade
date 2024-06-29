using UnityEngine;

public class PlayerBullet : Bullet
{
    //  ------------------ Public ------------------
    public int damage = 50;
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
        enemy.TakeDamage(damage);
        PlayerStats.instance.TotalDamageDealt+= damage;
        updatePierces(other);
        
        
    }
    public override void updatePierces(Collider2D other) {
        numberOfPierces--;
        if(isBouncy) handleBounce(other);
        if(numberOfPierces > 0) return;
        if(_isHit) {
            _isHit = false;
            PlayerStats.instance.TotalShotsHit++;
        }
        // Spawn the partcles for missles.. 
        if(isExplosive) {
            Instantiate(explosionOBJ, transform.position, Quaternion.Inverse(transform.rotation));
            Destroy(gameObject);
            return;
        }


        if (_isEntityHit) Instantiate(bloodParticles, transform.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 180)));
        else Instantiate(wallParticles, transform.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 180)));
        Destroy(gameObject);
    }

    // Todo: Refactor Boss to use enemy
    private void _bossCheck(Collider2D other) {
        Boss1Segments boss = other.GetComponent<Boss1Segments>();
        if(boss == null) return;
        _isHit = true;
        _isEntityHit = true;
        boss.TakeDamage(damage);
        PlayerStats.instance.TotalDamageDealt+= damage;
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

