using UnityEngine;

public class ExplosionObj : MonoBehaviour
{

    public float radius;
    public GameObject explosionParticleEffect; 
    public int damage;
    public string target_tags;
    public string soundName;

    void Start() {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.right, 0);
        if(hits.Length <= 0) {
            Debug.Log("No hits");
        } else {
            Debug.Log("Hits: " + hits.Length);
            foreach (RaycastHit2D hit in hits)
            {
                Debug.Log("Hit " + hit.collider.name); // Log the name of each collider that was hit
            }
        }
        GameObject partcles = Instantiate(explosionParticleEffect, transform.position, transform.rotation);
        ParticleSystem part = partcles.GetComponent<ParticleSystem>();
        var main = part.main;
        main.startSpeed = radius;
        Destroy(gameObject);
        AudioManager.instance.PlayOnShot(soundName);
    }

    
}
