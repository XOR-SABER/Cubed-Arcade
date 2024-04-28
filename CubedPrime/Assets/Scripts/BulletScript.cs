using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletScript : MonoBehaviour
{
    public float speed = 50f;
    public int damage = 50;
    
    private Vector2 playerVelocity;

    public void SetPlayerVelocity(Vector2 velocity)
    {
        playerVelocity = velocity;
    }

    void Update()
    {
        transform.Translate((Vector2.up + playerVelocity) * (speed * Time.deltaTime));
        Destroy(gameObject, 2f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}