using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public Image healthbar;
    public GameObject bloodParticles;
    public GameObject healParticles;
    public GameObject deathPartcles;
    public int startingHealth = 8;
    public static int Health;
    private bool _isInvincible = false;
    public float invincibilityDurationSeconds = 2.0f;
    public int points = 0;
    public int TotalHealed = 0;
    public int TotalDamageDealt = 0;
    public int TotalDamageTaken = 0;
    public int TotalEnemiesKilled = 0;
    public int TotalShotsTaken = 0;
    public int TotalShotsHit = 0;
    public bool isPlayerDead = false;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
     private void Start()
    {
        Health = startingHealth;
    }
     
    public static PlayerStats instance;

    public void TakeDamage(int damageReceived)
    {
        TotalDamageTaken += damageReceived;
        if(isPlayerDead) return;
        if(_isInvincible) return;
        StartCoroutine(Invincibility());
        Health -= damageReceived;
        healthbar.fillAmount = (float)Health / startingHealth;
        Instantiate(bloodParticles, transform.position, Quaternion.Inverse(transform.rotation));
        if(Health <= 0) onDeath();
    }
    public void heal(int healAmount) {
        Debug.Log("Heal called!");
        Debug.Log("Healed: " + healAmount);
        TotalHealed += healAmount;
        Health += healAmount;
        Health = Math.Clamp(Health, 0, startingHealth);
        Instantiate(healParticles, transform.position, transform.rotation); 
    }

    private IEnumerator Invincibility()
    {
        _isInvincible = true;
        yield return new WaitForSeconds(invincibilityDurationSeconds);
        _isInvincible = false;
    }

    public void AddPoints(int amount)
    {
        points += amount;
        Debug.Log("Points: " + points);
    }

    public void onDeath() {
        Debug.Log("DEATH");
        isPlayerDead = true;
        Instantiate(deathPartcles, transform.position, transform.rotation);
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) Debug.LogError("Please set the player object with the 'Player' Tag");
        else {
            var pl = player.GetComponent<PlayerMovement>();
            pl.onDeath();
        }
        // Game over screen right there.. 
    }
    
    // public void reset() {
    //     Health = startingHealth;
    //     points = 0;
    //     TotalHealed = 0;
    //     TotalDamageDealt = 0;
    //     TotalDamageTaken = 0;
    //     TotalEnemiesKilled = 0;
    // }
}
