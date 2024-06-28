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

    // Audio
    [Header("Audio")]
    [Tooltip("Name of the sound that plays when fired.")]
    public string soundName;


    // Particles
    [Header("Particles")]
    [Tooltip("THe partcles that is emitted when the gun is shot.")]
    public GameObject shootParticles;


    // Bullet
    [Header("Bullet Prefab")]
    [Tooltip("Bullet that the gun shoots.")]
    public GameObject projectilePrefab;


    // Fire rate
    [Header("Fire Rate")]
    [Tooltip("Rate of Fire.")]
    [Range (0.0f, 100.0f)]
    public float fireRate = 1;

    // Recoil
    [Header("Impluse / Recoil")]
    [Range (0.0f, 5f)]
    [Tooltip("Impluse Force is the force given to the camera to simulate Recoil")]
    public float impluseForce = 1;


    // Rev up Mechanic.. 
    [Header("Rev up / spin-up mechanic")]
    [Tooltip("Toggle the Rev-up mechanic.")]
    public bool isRevUp = false;

    [Tooltip("The minimum fire rate in the Rev-Up.")]
    [Range (1.0f, 100.0f)]
    public float minFireRate = 3f;

    [Tooltip("Time which it takes to Rev-up.")]
    [Range (0.1f, 10.0f)]
    public float maxRevTime = 2f;

    [Tooltip("It controls the rate of growth from Rev-up.")]
    public AnimationCurve revUpCurve;


    // Burst fire mechanic
    [Header("Burst fire mechanic")]
    [Tooltip("Toggle the burst fire mechanic.")]
    public bool isBurst; 

    [Tooltip("Burst delay is the delay between shots in a burst")]
    [Range (0.0f, 5f)]
    public float burstDelay;

    [Tooltip("Number of rounds per burst.")]
    [Range(1,32)]
    public int roundsPerBurst;


    // Spread mechanic
    [Header("Spread mechanic")]
    [Tooltip("Toggle the spread fire mechanic.")]
    public bool hasSpread; 

    [Tooltip("Number of spread per burst.")]
    [Range(1,32)]
    public int numOfPellets = 5;

    [Tooltip("Angle of spread")]
    [Range(0,360)]
    public int pelletSpreadAngle = 15;

    // Charge mechanic
    [Header("Charge mechanic")]
    [Tooltip("Toggle the charge mechanic.")]
    public bool isCharge;

    [Tooltip("The time it took to charge a shot.")]
    public float chargeTime;


    // Lazer mechanic
    [Header("Lazer mechanic")]
    [Tooltip("Toggle lazer mechanic")]
    public bool isLazer; 


    // Cooldown mechanic
    [Header("Cooldown time")]
    [Tooltip("The time it takes to cooldown.")]
    [Range (0.0f, 5f)]
    public float cooldownTime;
    
    [Tooltip("Number of shots it takes until cooldown")]
    [Range(0, 1000)]
    public int numOfShotsPreCool;

    
    //  ------------------ Private ------------------
    [HideInInspector]
    public bool isEquiped;
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
        _currentFireRate = fireRate;
    }
    public void ShootOrder()
    {
        
        if (Time.time >= _nextFireTime)
        {
            _nextFireTime = Time.time + 1 / _currentFireRate;
            if (isRevUp)
            {
                _currentFireRate = Mathf.Lerp(minFireRate, fireRate, revUpCurve.Evaluate(_timeHeld / maxRevTime));
            }
            _fire();
        }

        _timeHeld += Time.deltaTime;
        _timeHeld = Mathf.Clamp(_timeHeld, 0, maxRevTime);
    }
    public void ResetTimeHeld()
    {
        _timeHeld = 0;
    }
    private void _fire()
    {
        if (hasSpread && !isBurst) _spreadShot();
        else if (isBurst) StartCoroutine(_burstShot());
        else _singleFire();
        // After all said and done!
        
        _impulse();
        PlayerStats.instance.TotalShotsTaken++;
    }
    
    // Spread shot logic
    private void _spreadShot() { 
        float startAngle = -pelletSpreadAngle * (numOfPellets - 1) / 2f;
        for (int i = 0; i < numOfPellets; i++)
        {
            Quaternion spreadRotation = Quaternion.Euler(0, 0, startAngle + pelletSpreadAngle * i);
            Instantiate(projectilePrefab, firePoint.position, transform.rotation * spreadRotation);
        }
        if (soundName != null) AudioManager.instance.PlayOnShot(soundName);
        Instantiate(shootParticles, firePoint.position, transform.rotation);
        _impulse();
    }

    //Burst shot logic
    private IEnumerator _burstShot() {
        Debug.Log("Burst!");
        for (int i = 0; i < roundsPerBurst; i++)
        {   
                if(!isEquiped) break;
                if(hasSpread) _spreadShot();
                else _singleFire();
                yield return new WaitForSecondsRealtime(burstDelay);
        }
        yield break;
    }

    private void _singleFire() {
        if (soundName != null) AudioManager.instance.PlayOnShot(soundName);
        Instantiate(projectilePrefab, firePoint.position, transform.rotation);
        Instantiate(shootParticles, firePoint.position, transform.rotation);
        _impulse();
    }



    // Camera Recoil
    private void _impulse()
    {
        _impulseSource.GenerateImpulse((-transform.up).normalized * impluseForce);
    }

}
