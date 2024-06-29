using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //  ------------------ Public ------------------
    public float speed = 50f;
    public int numberOfPierces = 1;
    public float bulletLifeTime = 10f;
    public bool isExplosive; 
    public GameObject explosionOBJ;
    public bool isBouncy;
    public bool isOnlyEnemyPiercing;
    public bool isTracking; 
    public float bounceOffset = 0.25f;
    public GameObject wallParticles;
    public GameObject bloodParticles;
    // ------------------ Protected ------------------
    protected Vector2 _bulletVelocity;
    protected bool _isEntityHit = false;
    protected Transform _trackingTrans; 
    void Start() {
        _bulletVelocity = Vector2.up;
        if(isTracking) _trackingTrans = _trackInit(); 
        Destroy(gameObject, bulletLifeTime);
    }

    void Update()
    {
        transform.Translate(_bulletVelocity * (speed * Time.deltaTime));
        if(isTracking) _track();
    }

    public virtual void handleBounce(Collider2D other)
    {
        // Look for the normal.. 
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
            // Approx.. 
            Vector2 collisionNormal = (transform.position - other.transform.position).normalized;
            _bulletVelocity = Vector2.Reflect(_bulletVelocity, collisionNormal);
        }
        
    }

    public virtual void updatePierces(Collider2D other) {
        numberOfPierces--;
        if(isBouncy) handleBounce(other);
        if(numberOfPierces > 0) return;
        // Spawn the partcles for missles.. 
        if(isExplosive) {
            createMissleExplosion();
            return;
        }


        if (_isEntityHit) Instantiate(bloodParticles, transform.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 180)));
        else Instantiate(wallParticles, transform.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 180)));
        Destroy(gameObject);
    }

    public void createMissleExplosion() {
        Instantiate(explosionOBJ, transform.position, Quaternion.Inverse(transform.rotation));
        Destroy(gameObject);
    }

    protected void _solidCheck(Collider2D other) {
        Rigidbody2D body = other.GetComponent<Rigidbody2D>();
        if(body == null) return;
        if(body.CompareTag("BulletSolid") || body.CompareTag("Train")) {
            _isEntityHit = false;
            updatePierces(other);
        } else _isEntityHit = true;
        
    }
    // Stub.. to add enemy tracking in the future..  
    protected virtual Transform _trackInit() { 
        return null;
    }

    // Stub.. to add enemy tracking in the future.. 
    protected virtual void _track()
    {
        Vector2 targetDirection = (_trackingTrans.position - transform.position).normalized;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, speed * Time.deltaTime);
        transform.position += transform.up * (speed * Time.deltaTime);
    }
    
}
