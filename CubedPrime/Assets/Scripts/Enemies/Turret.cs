using UnityEngine;

public class Turret : MonoBehaviour
{

    public float fireRate = 1f;
    private float nextFireTime;
    public int range = 15;
    
    
    public GameObject projectilePrefab;

    public Transform firePoint;
    public GameObject player;
    public Transform playerTransform;
    public GameObject ExplosionFX;
    public bool isReinforcer;
    public GameObject reinforcement;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.GetComponent<Transform>();
        }
        
    }

     void Update()
    {
        if (player != null)
        {
            Vector3 direction = playerTransform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle + 270, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);

            if (Vector3.Distance(transform.position, playerTransform.position) <= range && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }
        
    public void Shoot()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        AudioManager.instance.PlayOnShot("ShortShot");
        if (isReinforcer) {
            int randomNumber = Random.Range(0, 5);
            if(randomNumber == 1) Instantiate(reinforcement, transform.position, reinforcement.transform.rotation);
        }

    }
    public void turretDeath() {
        Instantiate(ExplosionFX, transform.position, transform.rotation);
    }
}

