using System;
using System.Collections;
using Scripts;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public Image healthbar;
    public bool isInMenu = true;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI currentTrack;
    public TextMeshProUGUI FPSCounter;
    public GameObject healParticles;
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
    public bool isBossActive = false;
    public int currentEnemiesCount = 0;
    public int maxEnemies = 50;
    public string currentlyPlaying;
    private PlayerMovement _playerRef;
    private SceneSwitcher _sceneSwitcher;
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
        _sceneSwitcher = SceneSwitcher.instance;
        _playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        if(_playerRef == null) Debug.LogError("Player with the player tag does not exist in scene"); 
        Health = startingHealth;
        healthText.text = string.Format("{0}/{1}", Health, startingHealth);
    }

    private void Update() {
        scoreText.text = string.Format("Score: {0}", points);
        currentTrack.text = string.Format("Currently playing: {0}", currentlyPlaying);
        FPSCounter.text = string.Format("FPS: {0}", math.round(1.0/Time.deltaTime));
    }
     
    public static PlayerStats instance;
    public void TakeDamage(int damageReceived)
    {
        
        TotalDamageTaken += damageReceived;
        if(isPlayerDead) return;
        if(_isInvincible) return;
        AudioManager.instance.PlayOnShot("DamageSound");
        StartCoroutine(Invincibility());
        Health -= damageReceived;
        healthbar.fillAmount = (float)Health / startingHealth;
        healthText.text = string.Format("{0}/{1}", Health, startingHealth);
        if(Health <= 0) onDeath();
        
        
        
    }
    public void heal(int healAmount) {
        Debug.Log("Heal called!");
        Debug.Log("Healed: " + healAmount);
        TotalHealed += healAmount;
        Health += healAmount;
        Health = Math.Clamp(Health, 0, startingHealth);
        healthbar.fillAmount = (float)Health / startingHealth;
        healthText.text = string.Format("{0}/{1}", Health, startingHealth);
        Instantiate(healParticles, _playerRef.transform.position, _playerRef.transform.rotation, _playerRef.transform); 
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
        SaveScore();
        isPlayerDead = true;
        _playerRef.onDeath();
        // Game over screen right there.. 
    }

    private void SaveScore()
    {
        int highScore = PlayerPrefs.GetInt("highScore", 0);
        if (highScore < points)
        {
            PlayerPrefs.SetInt("highScore", points);
        }

        if (_sceneSwitcher != null)
        {
            int levelScore = PlayerPrefs.GetInt(_sceneSwitcher.levelToLoad.levelHighScoreTag, 0);
            if (levelScore < points)
            {
                PlayerPrefs.SetInt(_sceneSwitcher.levelToLoad.levelHighScoreTag, points);
            }
        }
    }

    // Handle Null.. 
    public PlayerMovement getPlayerRef() { 
        return _playerRef;
    }
    
    public void resetStats() {
        isBossActive = false;
        _isInvincible = false;
        isPlayerDead = false;
        Health = startingHealth;
        points = 0;
        TotalHealed = 0;
        TotalDamageDealt = 0;
        TotalDamageTaken = 0;
        TotalEnemiesKilled = 0;
    }
}
