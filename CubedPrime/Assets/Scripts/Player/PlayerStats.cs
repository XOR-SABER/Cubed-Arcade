using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public Image healthbar;
    public GameObject bloodParticles;
    public int startingHealth = 8;
    public static int Health;
    private bool isInvincible = false;
    public float invincibilityDurationSeconds = 2.0f;
    public int points = 0;
    public int TotalHealed;
    public int TotalDamageDealt;
    public int TotalDamageTaken;
    public int TotalEnemiesKilled;

    //TODO: Add UI for Health, Points, ect...
    
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
        if(isInvincible) return;
        StartCoroutine(Invincibility());
        Health -= damageReceived;
        healthbar.fillAmount = (float)Health / startingHealth;
        Debug.Log("Damage: " + damageReceived + " Health: " + Health + "/" + startingHealth);
        Instantiate(bloodParticles, transform.position, Quaternion.Inverse(transform.rotation));
    }

    private IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDurationSeconds);
        isInvincible = false;
    }

    public void AddPoints(int amount)
    {
        points += amount;
        Debug.Log("Points: " + points);
    }
    
}
