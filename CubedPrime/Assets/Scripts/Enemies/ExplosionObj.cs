using UnityEngine;

public class ExplosionObj : MonoBehaviour
{

    public float radius;
    public float damageRadius;
    public GameObject explosionParticleEffect; 
    public int damage;
    public string target_tags;
    public string soundName;
    public LayerMask enemyLayer;
    public LayerMask playerLayer;

    void Start() {
        if(target_tags == "Enemy") {
            dealEnemyDamage();
        } else if (target_tags == "Player") {
            dealPlayerDamage();
        } else if (target_tags == "All") {
            dealAll();
        }
        GameObject partcles = Instantiate(explosionParticleEffect, transform.position, transform.rotation);
        ParticleSystem part = partcles.GetComponent<ParticleSystem>();
        var main = part.main;
        main.startSpeed = radius;
        Destroy(gameObject);
        AudioManager.instance.PlayOnShot(soundName);
    }

    void dealEnemyDamage() {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, damageRadius, Vector2.right, 0f, enemyLayer);
        if(hits.Length > 0) {
            foreach(RaycastHit2D hit in hits) {
                Enemy obj = hit.collider.gameObject.GetComponent<Enemy>();
                if(obj != null) obj.TakeDamage(150);
            }
        }
    }

    void dealPlayerDamage() {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, damageRadius, Vector2.right, 0f, playerLayer);
        if(hit.collider != null) {
            if(hit.collider.CompareTag("Player")) PlayerStats.instance.TakeDamage(1);
        }
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, damageRadius);
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawWireSphere(transform.position, damageRadius);
    // }

    void dealAll() {
        dealPlayerDamage();
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, damageRadius, Vector2.right, 0f, enemyLayer);
        if(hits.Length > 0) {
            foreach(RaycastHit2D hit in hits) {
                Enemy obj = hit.collider.gameObject.GetComponent<Enemy>();
                if(obj != null) obj.TakeDamage(150);
            }
        }
    }

    
}
