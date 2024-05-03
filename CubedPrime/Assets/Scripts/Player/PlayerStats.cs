using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int startingHealth = 4;
    public static int Health;
    
    public int points = 0;
    
    //TODO: Add UI for Health, Points, ect...
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
     private void Start()
    {
        Health = startingHealth;
    }
     
    public static PlayerStats instance;

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
    
}
