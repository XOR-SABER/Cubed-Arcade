using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int damage = 1;
    public int startHealth = 100;
    [HideInInspector]
    public int health;

    public bool delayDeath;

    public int points = 100;
    
    public Image healthBar;

    public GameObject onDeathEffect;

    void Start()
    {
        health = startHealth;
    } 
    
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         PlayerStats.instance.TakeDamage(damage);
    //         Destroy(gameObject);
    //     }
    // }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         PlayerStats.instance.TakeDamage(damage);
    //         // Destroy(gameObject);
    //     }
    // }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        healthBar.fillAmount = (float)health / startHealth;
        if (health <= 0 && ! delayDeath)
        {
            EnemyDeath();
        }
    }

    public void EnemyDeath()
    {
        PlayerStats.instance.AddPoints(points);
        PlayerStats.instance.TotalEnemiesKilled++;
        Turret tur = GetComponent<Turret>();
        if(tur != null) {
            if(onDeathEffect != null)
            {
                Instantiate(onDeathEffect, transform.position, quaternion.identity);
            }            
            tur.turretDeath();
            Destroy(tur.mainObject);
        }
         else
         {
             if(onDeathEffect != null)
             {
                 Instantiate(onDeathEffect, transform.position, quaternion.identity);
             } 
             Destroy(gameObject);
         }
    }
    
    
}
