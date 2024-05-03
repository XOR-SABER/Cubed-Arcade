using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage = 1;
    public int startHealth = 100;
    private int health;

    public int points = 100;
    

    void Start()
    {
        health = startHealth;
    } 
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats.instance.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStats.instance.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            EnemyDeath();
        }
    }

    public void EnemyDeath()
    {
        PlayerStats.instance.AddPoints(points);
        Destroy(gameObject);
    }
    
    
}
