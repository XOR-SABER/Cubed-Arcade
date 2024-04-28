using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int startingHealth = 4;
    public static int Health;

    public float startingSpeed = 5.0f;
    public static float Speed;

    public int points = 0;
    
    //TODO: Add UI for Health, Points, ect...
    
     private void Start()
    {
        Health = startingHealth;
        Speed = startingSpeed;
    }

    public void TakeDamage(int damageReceived)
    {
        Health -= damageReceived;
        Debug.Log("Damage: " + damageReceived + " Health: " + Health + "/" + startingHealth);
        //Maybe I-frames after?
    }

    public void AddPoints(int amount)
    {
        points += amount;
        Debug.Log("Points: " + points);
    }
    
    //TODO stats for enemies killed, damage dealt
}
