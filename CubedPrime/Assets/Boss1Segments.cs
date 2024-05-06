using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Segments : MonoBehaviour
{
    public WormBoss mainWorm; 
    public float speed = 10f;
    public bool isTail = false;
    public bool isHead = false;
    public Transform leftTurret;
    public Transform rightTurret;
    public float maxDistance = 100f;
    public LayerMask layerMask;
    public GameObject bulletPrefab;
    public GameObject explosionFX;
    public float fireRate = 4f;
    private float nextFireTime;
    private GameObject _player_ref;
    private Vector2 _dir;
    public void TakeDamage(int damageAmount) {

        _player_ref = GameObject.FindGameObjectWithTag("Player");
        if (_player_ref == null) Debug.LogError("Please set the player object with the 'Player' Tag");
        if(isTail || isHead) mainWorm.TakeDamage(damageAmount * 2);
        else mainWorm.TakeDamage(damageAmount);
    }
    void Update() {
        if(!mainWorm.isAlive()) return;
        _dir = mainWorm.transform.position - transform.position;
        float angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + 270, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
        CheckLineOfSight(leftTurret, -leftTurret.right);
        CheckLineOfSight(rightTurret, rightTurret.right);
    }

    void CheckLineOfSight(Transform turret, Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(turret.position, direction, maxDistance, layerMask);
        if (hit.collider != null && Time.time >= nextFireTime) 
        {
            Shoot(turret);
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(leftTurret.position, leftTurret.position - leftTurret.right * maxDistance);
    //     Gizmos.color = Color.blue;
    //         Gizmos.DrawLine(rightTurret.position, rightTurret.position + rightTurret.right * maxDistance);
    // }
    public void Shoot(Transform turret)
    {
         if (_player_ref == null) {
            Debug.LogError("Player reference is null");
            return;
        }
        if (bulletPrefab == null) {
            Debug.LogError("Bullet prefab is not assigned");
            return;
        }
        Vector3 direction = _player_ref.transform.position - turret.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        Instantiate(bulletPrefab, turret.position, rotation);
        AudioManager.instance.PlayOnShot("ShortShot");

    }

    public void onDeath() {
        GameObject explosion = Instantiate(explosionFX, transform.position, transform.rotation);
        ExplosionObj exp = explosion.GetComponent<ExplosionObj>();
        exp.radius = 35f;
        Destroy(gameObject);
    }
}
