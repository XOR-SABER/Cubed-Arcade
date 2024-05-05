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
    private bool _isPlayerInRange;
    public LayerMask layerMask; 
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.GetComponent<Transform>();
        else Debug.LogError("Please set the player object with the 'Player' Tag");
    }

     void Update()
    {
        if (player == null) return; 
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, range, Vector2.right, 0f, layerMask);  
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
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + 270, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);

        if (Vector3.Distance(transform.position, playerTransform.position) <= range && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }
        
    public void Shoot()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        AudioManager.instance.PlayOnShot("ShortShot");

    }
    public void turretDeath() {
        Instantiate(ExplosionFX, transform.position, transform.rotation);
    }
}

