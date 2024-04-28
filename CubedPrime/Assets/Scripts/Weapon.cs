using System;
using UnityEngine;


    public class Weapon : MonoBehaviour
    {
        public float fireRate = 1; 
        //Magazine/Reload?
        //Particle System for ShootFunction?
        private float nextFireTime;

        
        public Transform firePoint;
        public GameObject projectilePrefab;
        public bool isEquipped = false;

        private void Update()
        {
            if (Input.GetButton("Fire1") && Time.time >= nextFireTime && isEquipped)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
            
        }
        
        public void Shoot()
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
    }
