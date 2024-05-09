using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectiles : MonoBehaviour
{
    // Bullets
    public bool isMissile;
    public GameObject explosionOBJ;
    // Stats
    public float speed = 10f;
    public int damage = 1;
    public float bulletLifeTime = 5f;
    public GameObject wallParticles;
    // Player
    private GameObject player;
    private Transform playerTransform;
    private Vector3 _direction;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.GetComponent<Transform>();
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            if (isMissile)
            {
                Vector2 targetDirection = (playerTransform.position - transform.position).normalized;
                float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, q, speed * Time.deltaTime);
                transform.position += transform.up * (speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector2.up * (speed * Time.deltaTime));
                Destroy(gameObject, bulletLifeTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(isMissile) {
                GameObject OBJ = Instantiate(explosionOBJ, transform.position, Quaternion.Inverse(transform.rotation));
                ExplosionObj exp = OBJ.GetComponent<ExplosionObj>();
                exp.target_tags = "Player";
                exp.radius = 15f;
                exp.damageRadius = 5.0F;
                Destroy(gameObject);
            } else {
                PlayerStats.instance.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        if(other.CompareTag("BulletSolid")) {
            if(isMissile) {
                GameObject OBJ = Instantiate(explosionOBJ, transform.position, Quaternion.Inverse(transform.rotation));
                ExplosionObj exp = OBJ.GetComponent<ExplosionObj>();
                exp.target_tags = "Player";
                exp.radius = 15f;
                exp.damageRadius = 5.0F;
                Destroy(gameObject);
            } else {
                PlayerStats.instance.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
