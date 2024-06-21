using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 50f;
    public int damage = 50;
    public int numberOfPierces = 1;
    public GameObject wallParticles;
    public GameObject bloodParticles;
    public bool isMissle; 
    public bool isBouncy;
    public bool isOnlyEnemyPiercing;
    public float bounceOffset = 0.25f;
    public GameObject explosionOBJ;
    private bool _isHit = false; 
    private bool _isEnemyHit = false;
    private Vector2 _bulletVelocity;
    void Start() {
        _bulletVelocity = Vector2.up;
    }

    void Update()
    {
        transform.Translate(_bulletVelocity * (speed * Time.deltaTime));
        Destroy(gameObject, 10f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        dealEnemyDamage(other);
        dealBossDamage(other);
        BarrelCheck(other);
        solidCheck(other);
    }
    void handleBounce(Collider2D other)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _bulletVelocity);

        if (hit.collider != null)
        {
            Vector2 collisionNormal = hit.normal;
            collisionNormal += new Vector2(Random.Range(-bounceOffset, bounceOffset), Random.Range(-bounceOffset, bounceOffset));
            collisionNormal.Normalize();
            _bulletVelocity = Vector2.Reflect(_bulletVelocity, collisionNormal);
        }
        else
        {
            Vector2 collisionNormal = (transform.position - other.transform.position).normalized;
            _bulletVelocity = Vector2.Reflect(_bulletVelocity, collisionNormal);
        }
        
    }
    void dealEnemyDamage(Collider2D other) {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null) {
            _isHit = true;
            _isEnemyHit = true;
            enemy.TakeDamage(damage);
            PlayerStats.instance.TotalDamageDealt+= damage;
            updatePierces(other);
        }
        
    }
    void updatePierces(Collider2D other) {
        numberOfPierces--;
        if(isBouncy) handleBounce(other);
        if(numberOfPierces > 0) return;
        if(_isHit) {
            _isHit = false;
            
            PlayerStats.instance.TotalShotsHit++;
        }
        // Spawn the partcles for missles.. 
        if(isMissle) {
            GameObject OBJ = Instantiate(explosionOBJ, transform.position, Quaternion.Inverse(transform.rotation));
            ExplosionObj exp = OBJ.GetComponent<ExplosionObj>();
            exp.target_tags = "Enemy";
            exp.radius = 20F;
            exp.damageRadius = 7.5F;
        }


        if (_isEnemyHit) 
            Instantiate(bloodParticles, transform.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 180)));
        else 
            Instantiate(wallParticles, transform.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 180)));
        Destroy(gameObject);
    }
    void dealBossDamage(Collider2D other) {
        Boss1Segments boss = other.GetComponent<Boss1Segments>();
        if(boss != null) {
            _isHit = true;
            _isEnemyHit = true;
            boss.TakeDamage(damage);
            PlayerStats.instance.TotalDamageDealt+= damage;
            updatePierces(other);
        }
    
    }
    void BarrelCheck(Collider2D other) {
        explosiveBarrel body = other.GetComponent<explosiveBarrel>();
        if(body != null) {
            _isHit = true;
            _isEnemyHit = false;
            body.destroyBarrel();
            updatePierces(other);
        }
    }
    void solidCheck(Collider2D other) {
        Rigidbody2D body = other.GetComponent<Rigidbody2D>();
        if(body == null) return;
        if(body.CompareTag("BulletSolid") || body.CompareTag("Train")) {
            _isEnemyHit = false;
            updatePierces(other);

        }
        
    }
}

