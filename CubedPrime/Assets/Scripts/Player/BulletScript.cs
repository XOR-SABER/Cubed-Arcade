using Unity.Mathematics;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 50f;
    public int damage = 50;
    public int numberOfPierces = 1;
    public GameObject wallParticles;
    public GameObject bloodParticles;
    public bool isMissle; 
    public GameObject explosionOBJ;
    private bool _isHit = false; 
    private Vector2 _playerVelocity;
    
    public void SetPlayerVelocity(Vector2 velocity)
    {
        _playerVelocity = velocity;
    }

    void Update()
    {
        transform.Translate((Vector2.up + _playerVelocity) * (speed * Time.deltaTime));
        Destroy(gameObject, 10f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        dealEnemyDamage(other);
        dealBossDamage(other);
        BarrelCheck(other);
        solidCheck(other);
    }
    void dealEnemyDamage(Collider2D other) {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null) {
            _isHit = true;
            enemy.TakeDamage(damage);
            PlayerStats.instance.TotalDamageDealt+= damage;
            numberOfPierces--;
        }
        if(numberOfPierces <= 0) {
            if(_isHit) {
                _isHit = false;
                PlayerStats.instance.TotalShotsHit++;
            }
            hitObj(bloodParticles);
        }
    }
    void dealBossDamage(Collider2D other) {
        Boss1Segments boss = other.GetComponent<Boss1Segments>();
        if(boss != null) {
            _isHit = true;
            boss.TakeDamage(damage);
            PlayerStats.instance.TotalDamageDealt+= damage;
            numberOfPierces--;
        }
        if(numberOfPierces <= 0) {
            if(_isHit) {
                _isHit = false;
                PlayerStats.instance.TotalShotsHit++;
            }
            hitObj(bloodParticles);
        }
    }
    void BarrelCheck(Collider2D other) {
        explosiveBarrel body = other.GetComponent<explosiveBarrel>();
        if(body != null) {
            _isHit = true;
            body.destroyBarrel();
        }
        if(numberOfPierces <= 0) {
            if(_isHit) {
                _isHit = false;
                PlayerStats.instance.TotalShotsHit++;
            }
            hitObj(wallParticles);
        }
    }
    void solidCheck(Collider2D other) {
        Rigidbody2D body = other.GetComponent<Rigidbody2D>();
        if(body == null) return;
        if(body.CompareTag("BulletSolid") || body.CompareTag("Train")) {
            numberOfPierces--;
        }
        if(numberOfPierces <= 0) {
            if(_isHit) {
                _isHit = false;
                PlayerStats.instance.TotalShotsHit++;
            }
            hitObj(wallParticles);
        }
    }

    void hitObj(GameObject partcleEffect) {
        if(isMissle) {
            GameObject OBJ = Instantiate(explosionOBJ, transform.position, Quaternion.Inverse(transform.rotation));
            ExplosionObj exp = OBJ.GetComponent<ExplosionObj>();
            exp.target_tags = "Enemy";
            exp.radius = 20F;
            exp.damageRadius = 7.5F;
        }
        Destroy(gameObject);
        Instantiate(partcleEffect, transform.position, Quaternion.Inverse(transform.rotation));
    }
}

