using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int damage = 1;
    public int startHealth = 100;
    private int health;

    public int points = 100;
    
    public Image healthBar;
    public bool delayedDeath = false; 

    void Start()
    {
        health = startHealth;
    } 

    public int getHealth() {
        return health;
    }
    
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         PlayerStats.instance.TakeDamage(damage);
    //         Destroy(gameObject);
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Train")) TakeDamage(10000);
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        healthBar.fillAmount = (float)health / startHealth;
        if (health <= 0)
        {
            EnemyDeath();
        }
    }

    public void EnemyDeath()
    {
        PlayerStats.instance.AddPoints(points);
        PlayerStats.instance.TotalEnemiesKilled++;
        if(delayedDeath) return;
        Turret tur = GetComponent<Turret>();
        if(tur != null) {
            tur.turretDeath();
            Destroy(tur.mainObject);
        }
         else Destroy(gameObject);
    }
    
    public int getHealth() {
        return health;
    }
    
}
