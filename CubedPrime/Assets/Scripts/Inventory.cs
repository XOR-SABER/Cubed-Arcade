using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory i;

    public static int money;
    
    public static GameObject hat;
    public static GameObject weapon;
    public static GameObject skin;

    private void Awake()
    {
        if (i is not null)
        {
           Destroy(gameObject); 
        }

        i = this;
        DontDestroyOnLoad(gameObject);
    }
}
