using Unity.Mathematics;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 50f;
    public int damage = 50;
    public int numberOfPierces = 1;
    public GameObject wallParticles;
    public GameObject bloodParticles;

    private Vector2 _playerVelocity;
    

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
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.TakeDamage(damage);
            numberOfPierces--;
        }
        if(numberOfPierces <= 0) {
            Destroy(gameObject);
            Instantiate(bloodParticles, transform.position, Quaternion.Inverse(transform.rotation));
        }
        Boss1Segments boss = other.GetComponent<Boss1Segments>();
        if(boss != null) {
            boss.TakeDamage(damage);
            numberOfPierces--;
        }
        if(numberOfPierces <= 0) {
            Destroy(gameObject);
            Instantiate(bloodParticles, transform.position, Quaternion.Inverse(transform.rotation));
        }
        Rigidbody2D body = other.GetComponent<Rigidbody2D>();
        if(body == null) return;
        if(body.CompareTag("BulletSolid")) {
            numberOfPierces--;
        }
        if(numberOfPierces <= 0) {
            Destroy(gameObject);
            Instantiate(wallParticles, transform.position, Quaternion.Inverse(transform.rotation));
        }
    }
}