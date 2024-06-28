using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class GunStats : ScriptableObject
{
    // Weapon info
    [Header("Weapon Info")]
    [Tooltip("Weapon name to refrence in the Cube-pedia")]
    public string weaponName = "New Gun";
    [Tooltip("Weapon description to refrence in the Cube-pedia")]
    public string weaponDescription = "Stub to add";

    // Appearance
    [Header("Appearance")]
    [Tooltip("The gun sprite to use in game, and refrence in the Cube-pedia")]
    public Sprite gunSprite;
    [Tooltip("The point where the bullet spawns in the sprite")]
    public Vector3 firePoint;
    [Tooltip("Toggle dual sprite mode, if the gun has a different holstered sprite.")]
    public bool isDualSprite;
    [Tooltip("Holstered sprite when dual sprite is enabled.")]
    public Sprite equipSprite;

    // Audio
    [Header("Audio")]
    [Tooltip("Name of the sound that plays when fired.")]
    public string soundName;

    // Bullets
    [Header("Bullet Prefab")]
    [Tooltip("Bullet that the gun shoots.")]
    public GameObject projectilePrefab;

    // Particles
    [Header("Particles")]
    [Tooltip("THe partcles that is emitted when the gun is shot.")]
    public GameObject shootParticles;

    // Recoil
    [Header("Impluse / Recoil")]
    [Range (0.0f, 5f)]
    [Tooltip("Impluse Force is the force given to the camera to simulate Recoil")]
    public float impluseForce = 1;

    // Fire rate
    [Header("Fire Rate")]
    [Tooltip("Rate of Fire.")]
    [Range (0.0f, 100.0f)]
    public float fireRate = 1;

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
    
}
