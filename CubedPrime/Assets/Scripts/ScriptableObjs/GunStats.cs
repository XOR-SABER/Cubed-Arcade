using UnityEngine;
[CreateAssetMenu(fileName = "New Gun stats", menuName = "Gun stats")]

public class GunStats : ScriptableObject {
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
}