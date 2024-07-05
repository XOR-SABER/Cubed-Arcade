using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : Enemy
{

    public float fireRate = 1f;
    public int range = 15;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public GameObject ExplosionFX;
    public LayerMask playerLayerMask; 
    public LineRenderer sightline; 
    public LayerMask enviromentLayerMask;
    public bool isMissleTurret;
    public bool isShotgunTurret;
    public int numOfShotgunPellets = 3;
    public int shotGunPelletSpreadAngle = 15;
    public float timeUntillCooldown;
    private bool _hasPlayerInRange;
    private float _nextFireTime;
    private float _timeShooting;
    private float _timeToRetract; 
    private Vector3 _prev_pos;
    private Vector3 _DEFAULT_DIST = new Vector3(0f, 1f, 0f);
    public override void EnemyDeath() {
        PlayerStats.instance.AddPoints(points);
        PlayerStats.instance.TotalEnemiesKilled++;
        PlayerStats.instance.currentEnemiesCount--;
        Instantiate(ExplosionFX, transform.position, transform.rotation);
        Destroy(transform.parent.gameObject);
    }

    public override void init() {
        if(_player_trans == null) {
            Debug.LogError("Player is null, and is being accessed by the Turret");
        }
    }

    // Unsubscribe since it doesn't use pathfinding.   
    void OnEnable() {
        TickSystem.onSecAction -= OnTick;
    }

    public override void enemyBehaviour() {
        if (_player_trans == null) return; 
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, range, Vector2.right, 0f, playerLayerMask);  
        if (hit.collider != null) {
            handleTurret();
        } else {
            _timeShooting = 0;
            
            if(_hasPlayerInRange) {
                _hasPlayerInRange = false;
            }

            if(sightline.GetPosition(1) != _DEFAULT_DIST) {
                _timeToRetract += Time.deltaTime;
                sightline.SetPosition(1, Vector3.Slerp(_prev_pos, _DEFAULT_DIST, _timeToRetract));
                if(_timeToRetract >= 1f) {
                    sightline.SetPosition(1, _DEFAULT_DIST);
                    _timeToRetract = 0f;
                }
            }
        }
    }

    void Update() {
        enemyBehaviour();
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red; // Setting the color of the Gizmo to red
    //     Gizmos.DrawWireSphere(transform.position, range); // Drawing a wireframe sphere at the transform's position
    // }

    void handleTurret() {
        Vector3 direction = _player_trans.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Vector3.Distance(transform.position, _player_trans.position), enviromentLayerMask);
        if (hit.collider != null) return;
        if(!_hasPlayerInRange) AudioManager.instance.PlayOnShot("Code_RedChargeUp");
        _hasPlayerInRange = true;
        // We have a player.. 
        _timeShooting+= Time.deltaTime;
        transform.rotation = _handleRotation(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        _prev_pos = transform.InverseTransformPoint(_player_trans.position);
        float lerpProgress = Mathf.Pow(_timeShooting, 2f);
        sightline.SetPosition(1, Vector3.Slerp(_DEFAULT_DIST, _prev_pos, lerpProgress));
        
        if(_timeShooting < 1f) return;
        // Vector3.Distance(transform.position, _player_trans.position) <= range && 
        if (Time.time >= _nextFireTime)
        {
            if(isShotgunTurret) StartCoroutine(shotgunShoot());
            else Shoot();
            _nextFireTime = Time.time + 1f / fireRate;
        }
        
    }
        
    public void Shoot()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        if(isMissleTurret) AudioManager.instance.PlayOnShot("MissleLaunch");
        else AudioManager.instance.PlayOnShot("GlockShot");

    }
    public IEnumerator shotgunShoot()
    {
        float startAngle = -shotGunPelletSpreadAngle * (numOfShotgunPellets - 1) / 2f;
        for (int i = 0; i < numOfShotgunPellets; i++)
        {
            Quaternion spreadRotation = Quaternion.Euler(0, 0, startAngle + shotGunPelletSpreadAngle * i);
            Instantiate(projectilePrefab, firePoint.position, firePoint.transform.rotation * spreadRotation);
        }
        AudioManager.instance.PlayOnShot("ShortShot");
        yield return null;

    }

    protected Quaternion _handleRotation(float angle) { 
        Quaternion rotation = Quaternion.AngleAxis(angle + 270, Vector3.forward);
        return Quaternion.Slerp(transform.rotation, rotation, Mathf.Clamp01(Time.deltaTime * 5f + 0.25f));
    }

}
