using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [HideInInspector]
    public float speed;
    public float startSpeed = 3f;
    
    public int damage = 1;

    public int startHealth = 100;
    private int health;

    public int points = 100;
    
    public GameObject player;
    PlayerStats playerStats;
    
    //TODO: Enemy AI to engage player, Use player gameobject for position? Different Script?

    private void Awake()
    {
        playerStats = player.GetComponent<PlayerStats>();
    }

    void Start()
    {
        health = startHealth;
        speed = startSpeed;
    } 
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerStats.TakeDamage(damage);
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
        playerStats.AddPoints(points);
        Destroy(gameObject);
    }
    
    
}
