using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Bullet stats", menuName = "Bullet stats")]
public class BulletStats : ScriptableObject {
    // Speed
    [Header("Speed")]
    [Tooltip("Speed in which the bullet moves.")]
    public float speed = 50f;

    // Pierces
    [Header ("Piercing")]
    [Tooltip("certain amount of times with one being only one collison allowed.")]
    public int numberOfPierces = 1;

    // Lifetime
    [Header("Lifetime")]
    [Tooltip("All bullets have a lifespan before they are automatically culled.")]
    public float bulletLifeTime = 10f;

    // Explosive
    [Header("Explosive")]
    [Tooltip("Enables explosions to be spawned on bullet death.")]
    public bool isExplosive; 
    [Tooltip("The object that spawns on bullet death.")]
    public GameObject explosionOBJ;

    // Bouncy
    [Header("Bounce")]
    [Tooltip("Enables bounce mechanic")]
    public bool isBouncy;
    [Tooltip("Toggle if the bullet can only pierce enemies.")]
    public bool isOnlyEnemyPiercing;
    [Tooltip("Furthest Angle offset that the bullet bounce from.")]
    public float bounceOffset = 0.25f;
    [Tooltip("Chance to bounce")]
    [Range(0.00f, 1f)]
    public float bounceChance; 

    // Tracking
    [Header("Tracking")]
    [Tooltip("Enables bullet tracking WIP")]
    public bool isTracking; 
    [Range(0.01f, 1000.0f)]
    public float trackingStrength;

    // Damage
    [Header("Damage")]
    [Tooltip("Damage done to the player")]
    public int playerDamage;
    [Tooltip("Damage done to enemies")]
    public int enemyDamage;
    
    // Particles
    [Header("Particles")]
    [Tooltip("Particles that spawn when the bullet hits a bulletSolid.")]
    public GameObject wallParticles;
    [Tooltip("Particles that spawn when the bullet hits a entity.")]
    public GameObject bloodParticles;
}   
