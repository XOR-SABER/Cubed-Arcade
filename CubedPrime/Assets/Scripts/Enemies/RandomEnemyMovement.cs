using Unity.VisualScripting;
using UnityEngine;

public class RandomEnemyMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rushSpeed = 10f;
    public float rushDistance = 10f;
    private GameObject player;
    private Transform playerTransform;

    private Vector3 moveDirection;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.GetComponent<Transform>();
        }        
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        moveDirection = new Vector3(randomX, randomY, 0f).normalized * moveSpeed;
    }

    void FixedUpdate()
    {
        if (playerTransform is null)
        {
            return;
        }
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        
        if (distanceToPlayer < rushDistance)
        { 
            moveDirection = (playerTransform.position - transform.position).normalized * rushSpeed;
        }
        else
        {
            moveDirection = moveDirection.normalized * moveSpeed;
        }
        
        transform.position += moveDirection * Time.fixedDeltaTime;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
            // Reflect the movement direction
            ContactPoint2D contact = collision.contacts[0];
            Vector2 reflection = Vector2.Reflect(moveDirection, contact.normal);
            moveDirection = reflection.normalized * moveSpeed;
    }
}