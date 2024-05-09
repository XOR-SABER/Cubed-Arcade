using Unity.Mathematics;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 50f;
    public int damage = 50;
    public int numberOfPierces = 1;
    public GameObject wallParticles;
    public GameObject bloodParticles;
    private bool _isHit = false; 
    private Vector2 _playerVelocity;
    
    void Start() {
        PlayerStats.instance.TotalShotsTaken++;
    }

    public void SetPlayerVelocity(Vector2 velocity)
    {
        _playerVelocity = velocity;
    }

    void Update()
    {
        transform.Translate((Vector2.up + _playerVelocity) * (speed * Time.deltaTime));
        Destroy(gameObject, 2f);
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
            Destroy(gameObject);
            if(_isHit) PlayerStats.instance.TotalShotsHit++;
            Instantiate(bloodParticles, transform.position, Quaternion.Inverse(transform.rotation));
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
            Destroy(gameObject);
            if(_isHit) PlayerStats.instance.TotalShotsHit++;
            Instantiate(bloodParticles, transform.position, Quaternion.Inverse(transform.rotation));
        }
    }
    void BarrelCheck(Collider2D other) {
        explosiveBarrel body = other.GetComponent<explosiveBarrel>();
        if(body != null) {
            _isHit = true;
            body.destroyBarrel();
        }
        if(numberOfPierces <= 0) {
            Destroy(gameObject);
            if(_isHit) PlayerStats.instance.TotalShotsHit++;
            Instantiate(wallParticles, transform.position, Quaternion.Inverse(transform.rotation));
        }
    }
    void solidCheck(Collider2D other) {
        Rigidbody2D body = other.GetComponent<Rigidbody2D>();
        if(body == null) return;
        if(body.CompareTag("BulletSolid") || body.CompareTag("Train")) {
            numberOfPierces--;
        }
        if(numberOfPierces <= 0) {
            Destroy(gameObject);
            if(_isHit) PlayerStats.instance.TotalShotsHit++;
            Instantiate(wallParticles, transform.position, Quaternion.Inverse(transform.rotation));
        }
    }
}

