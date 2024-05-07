using UnityEngine;

public class explosiveBarrel : MonoBehaviour
{
    public GameObject explosiveFx; 
    public float particleRadius;
    public float damageRadius;

    public void destroyBarrel() {
        GameObject explosion = Instantiate(explosiveFx, transform.position, transform.rotation);
        ExplosionObj exp = explosion.GetComponent<ExplosionObj>();
        exp.target_tags = "All";
        exp.radius = particleRadius;
        exp.damageRadius = damageRadius;
        Destroy(gameObject);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; // Setting the color of the Gizmo to red
        Gizmos.DrawWireSphere(transform.position, particleRadius); // Drawing a wireframe sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
