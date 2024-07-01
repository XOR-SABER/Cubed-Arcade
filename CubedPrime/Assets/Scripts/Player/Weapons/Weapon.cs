using System;
using System.Collections;
using Cinemachine;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    //  ------------------ Public ------------------
    // Appearance
    [Header("Appearance")]
    [Tooltip("The point where the bullet spawns in the sprite")]
    public Transform firePoint;

    [Tooltip("Toggle dual sprite mode, if the gun has a different holstered sprite.")]
    public bool isDualSprite;

    // Stats
    [Header("Stats")] 
    [Tooltip("Linked Scriptable OBJ that holds the gun stats")]
    public GunStats stat;
    
    // Hide in Insepctor
    [HideInInspector]
    public bool isEquiped;
    //  ------------------ Private ------------------
    private static CinemachineImpulseSource _impulseSource = null;
    // This exists for Rev-UP as well as Lazer
    private float _timeHeld = 0;

    // This exists exclusively for the Rev-Up Mechanic;
    private float _currentFireRate;

    // Time since the last shot.. 
    private double _nextFireTime;
    private void Start()
    {
        // Very smart idea from a friend.. 
        if(_impulseSource == null) _impulseSource = FindObjectOfType<CinemachineImpulseSource>();
        _currentFireRate = stat.fireRate;
    }

    public void ShootOrder()
    {
        
        if (Time.time >= _nextFireTime)
        {
            _nextFireTime = Time.time + 1 / _currentFireRate;
            if (stat.isRevUp)
            {
                _currentFireRate = Mathf.Lerp(stat.minFireRate, stat.fireRate, stat.revUpCurve.Evaluate(_timeHeld / stat.maxRevTime));
            }
            _fire();
        }

        _timeHeld += Time.deltaTime;
        _timeHeld = Mathf.Clamp(_timeHeld, 0, stat.maxRevTime);
    }
    public void ResetTimeHeld()
    {
        _timeHeld = 0;
        _currentFireRate = stat.fireRate;
    }
    private void _fire()
    {
        if (stat.hasSpread && !stat.isBurst) _spreadShot();
        else if (stat.isBurst) StartCoroutine(_burstShot());
        else _singleFire();
        // After all said and done!
        
        _impulse();
        PlayerStats.instance.TotalShotsTaken++;
    }
    
    // Spread shot logic
    private void _spreadShot() { 
        float startAngle = -stat.pelletSpreadAngle * (stat.numOfPellets - 1) / 2f;
        for (int i = 0; i < stat.numOfPellets; i++)
        {
            Quaternion spreadRotation = Quaternion.Euler(0, 0, startAngle + stat.pelletSpreadAngle * i);
            Instantiate(stat.projectilePrefab, firePoint.position, transform.rotation * spreadRotation);
        }
        if (stat.soundName != null) AudioManager.instance.PlayOnShot(stat.soundName);
        Instantiate(stat.shootParticles, firePoint.position, transform.rotation);
        _impulse();
    }

    //Burst shot logic
    private IEnumerator _burstShot() {
        Debug.Log("Burst!");
        for (int i = 0; i < stat.roundsPerBurst; i++)
        {   
                if(!isEquiped) break;
                if(stat.hasSpread) _spreadShot();
                else _singleFire();
                yield return new WaitForSecondsRealtime(stat.burstDelay);
        }
        yield break;
    }

    private void _singleFire() {
        if (stat.soundName != null) AudioManager.instance.PlayOnShot(stat.soundName);
        Instantiate(stat.projectilePrefab, firePoint.position, transform.rotation);
        Instantiate(stat.shootParticles, firePoint.position, transform.rotation);
        _impulse();
    }



    // Camera Recoil
    private void _impulse()
    {
        _impulseSource.GenerateImpulse((-transform.up).normalized * stat.impluseForce);
    }

}
