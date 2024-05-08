using System;
using UnityEngine;


public class WeaponNew : MonoBehaviour
{
    
    public Vector3 equipOffset;
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float fireRate = 1;
    public bool isRevUp = false;
    public float maxRevTime = 2f;
    public AnimationCurve revUpCurve;
    public bool isEquipped = false;
    public GameObject shootParticles;
    public bool isBurst; 
    public int roundsPerBurst;
    public bool isMagic;
    public bool isTracking;
    private float _timeHeld = 0;
    private float _nextFireTime;
    public string soundName;
    //Magazine/Reload?
    //Particle System for ShootFunction?

    private void Update()
    {
        if (!isEquipped) return;
        if (Time.time < _nextFireTime) return;

        if (Input.GetButton("Fire1"))
        {
            if (isRevUp) handleRevUp();
            else
            {
                Shoot();
                _nextFireTime = Time.time + 1f / fireRate;
                // Debug.Log($"Shooting without rev up. Next Fire Time: {_nextFireTime:F2}");
            }
        }
        else
        {
            if(_timeHeld <= 0) return;
            // if (_timeHeld > 0) Debug.Log("Fire button released, resetting _timeHeld.");
            _timeHeld -= Time.deltaTime; // Reset time held when the fire button is released
        }
    }

    public void Shoot()
    {
        if(isMagic) Instantiate(shootParticles, firePoint.position, firePoint.rotation, transform);
        else Instantiate(shootParticles, firePoint.position, firePoint.rotation);
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        if(soundName != null) {
            AudioManager.instance.PlayOnShot(soundName);
        }
    }

    void handleRevUp() {
        if (_timeHeld < 1) _timeHeld += Time.deltaTime * (20 / maxRevTime);
        float curveValue = revUpCurve.Evaluate(_timeHeld / maxRevTime);
        Shoot();
        _nextFireTime = Time.time + (1f / (fireRate * curveValue));
        // Debug.Log($"Shooting! Time held: {_timeHeld:F2}, Curve Value: {curveValue:F2}, Next Fire Time: {_nextFireTime:F2}");
    }
}
