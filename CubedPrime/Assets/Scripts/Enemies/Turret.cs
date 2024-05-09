using UnityEngine;

public class Turret : MonoBehaviour
{

    public float fireRate = 1f;
    public int range = 15;
    public GameObject mainObject;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public GameObject player;
    public Transform playerTransform;
    public GameObject ExplosionFX;
    public LayerMask playerLayerMask; 
    public LayerMask enviromentLayerMask;
    public bool isMissleTurret;
    public bool isShotgunTurret;
    public int numOfShotgunPellets = 3;
    public int shotGunPelletSpreadAngle = 15;
    private float _nextFireTime;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.GetComponent<Transform>();
        else Debug.LogError("Please set the player object with the 'Player' Tag");
    }

     void Update()
    {
        if (player == null) return; 
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, range, Vector2.right, 0f, playerLayerMask);  
        if (hit.collider != null) {
            handleTurret();
        }
        
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Setting the color of the Gizmo to red
        Gizmos.DrawWireSphere(transform.position, range); // Drawing a wireframe sphere at the transform's position
    }

    void handleTurret() {
        Vector3 direction = playerTransform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Vector3.Distance(transform.position, playerTransform.position), enviromentLayerMask);
        if (hit.collider != null) return;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + 270, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);

        if (Vector3.Distance(transform.position, playerTransform.position) <= range && Time.time >= _nextFireTime)
        {
            if(isShotgunTurret) shotgunShoot();
            else Shoot();
            _nextFireTime = Time.time + 1f / fireRate;
        }
    }
        
    public void Shoot()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        if(isMissleTurret) AudioManager.instance.PlayOnShot("MissleLaunch");
        else AudioManager.instance.PlayOnShot("ShortShot");

    }
    public void shotgunShoot()
    {
        float startAngle = -shotGunPelletSpreadAngle * (numOfShotgunPellets - 1) / 2f;
    
        for (int i = 0; i < numOfShotgunPellets; i++)
        {
            Quaternion spreadRotation = Quaternion.Euler(0, 0, startAngle + shotGunPelletSpreadAngle * i);
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation * spreadRotation);
        }
        AudioManager.instance.PlayOnShot("ShortShot");

    }
    public void turretDeath() {
        Instantiate(ExplosionFX, transform.position, transform.rotation);
    }
}

