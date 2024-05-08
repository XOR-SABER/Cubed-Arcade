using System;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    
    public Vector3 equipOffset;
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float fireRate = 1;
    public float maxFireRateRevUpWeapon = 30;
    public bool isRevUp = false;
    public float maxRevTime = 2f;
    public AnimationCurve revUpCurve;
    public GameObject shootParticles;
    public bool isBurst; 
    public int roundsPerBurst;
    public bool isMagic;
    public bool isTracking;
    private float _nextFireTime;
    public string soundName;
    
    private float _timeHeld = 0;
    
    
    private double nextFire;

    public void ShootOrder()
    {
        
        if (Time.time > nextFire)
        {
            nextFire = Time.time + 1 / fireRate;
            if (isRevUp)
            {
                fireRate = Mathf.Lerp(3, maxFireRateRevUpWeapon, revUpCurve.Evaluate(_timeHeld / maxRevTime));
            }
            Shoot();
        }

        Debug.Log(_timeHeld);
        _timeHeld += Time.deltaTime;
        _timeHeld = Mathf.Clamp(_timeHeld, 0, maxRevTime);
    }

    public void ResetTimeHeld()
    {
        _timeHeld = 0;
    }
    

    private void Shoot()
    {
        if(isMagic)
        {
            Instantiate(shootParticles, firePoint.position, firePoint.rotation, transform);
        }
        else
        {
            Instantiate(shootParticles, firePoint.position, firePoint.rotation);
        }

        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        if (soundName != null)
        {
            AudioManager.instance.PlayOnShot(soundName);
        }
    }
}