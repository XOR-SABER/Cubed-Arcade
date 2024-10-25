using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int damage = 1;
    public int startHealth = 100;
    protected int _health;
    public int points = 100;
    public GameObject onDeathEffect;
    public Image healthBar;
    public NavMeshAgent agent;
    public bool isSlime;
    protected int _distanceFromPlayer;
    protected static Transform _player_trans = null;
    protected bool _isCoolingDown;
    
    void OnEnable() {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        TickSystem.onSecAction += OnTick;
    }

    void OnDisable() {
        TickSystem.onSecAction -= OnTick;
    }

    protected void OnTick() {
        enemyBehaviour();
    }
    
    void Start()
    {
        init();
        resetHealth();
        if(_player_trans == null && !PlayerStats.instance.isPlayerDead) 
            _player_trans = PlayerStats.instance.getPlayerRef().transform;
    } 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // The train will insta kill it.. no exceptions.. 
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

    public virtual void EnemyDeath()
    {
        PlayerStats.instance.AddPoints(points);
        PlayerStats.instance.TotalEnemiesKilled++;
        PlayerStats.instance.currentEnemiesCount--;
        if (onDeathEffect != null) Instantiate(onDeathEffect, transform.position, Quaternion.identity);
        
        playDeathSFX();
        Destroy(gameObject);
    }
    
    public int getHealth() {
        return _health;
    }
    public void resetHealth() {
        _health = startHealth;
    }

    public virtual void enemyBehaviour() {
        if(_player_trans == null) return;
        float distanceToPlayer = Vector3.Distance(transform.position, _player_trans.position);
        float idealDistance = _distanceFromPlayer;

        if (distanceToPlayer < idealDistance) 
        {
            Vector3 directionToPlayer = transform.position - _player_trans.position;
            Vector3 newPosition = _player_trans.position + directionToPlayer.normalized * (idealDistance - 0.5f);
            agent.SetDestination(newPosition);
        }
        else
        {
            agent.SetDestination(_player_trans.position);
        }
    }

    public virtual void init() {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        if(_player_trans == null) return;
    }

    public void playDeathSFX() { 
        if (_player_trans == null) return; 
        if (Vector3.Distance(transform.position, _player_trans.position) > 35) return;
        if(!isSlime) AudioManager.instance.PlayOnShot("NormalEnemyDeath");
        else AudioManager.instance.PlayOnShot("SlimeyEnemyDeath");
    }
}   
