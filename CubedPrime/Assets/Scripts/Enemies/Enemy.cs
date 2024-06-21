using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int damage = 1;
    public int startHealth = 100;
    protected int _health;
    public int points = 100;
    public Image healthBar;
    public bool delayedDeath = false; 

    void Start()
    {
        resetHealth();
    } 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Train")) TakeDamage(10000);
    }

    public virtual void TakeDamage(int damageAmount)
    {
        _health -= damageAmount;
        healthBar.fillAmount = (float)_health / startHealth;
        if (_health <= 0)
        {
            EnemyDeath();
        }
    }

    public void EnemyDeath()
    {
        PlayerStats.instance.AddPoints(points);
        PlayerStats.instance.TotalEnemiesKilled++;
        PlayerStats.instance.currentEnemiesCount--;
        if(delayedDeath) return;
        Turret tur = GetComponent<Turret>();
        if(tur != null) {
            tur.turretDeath();
            Destroy(tur.mainObject);
        }
         else Destroy(gameObject);
    }
    
    public int getHealth() {
        return _health;
    }
    public void resetHealth() {
        _health = startHealth;
    }
}   
