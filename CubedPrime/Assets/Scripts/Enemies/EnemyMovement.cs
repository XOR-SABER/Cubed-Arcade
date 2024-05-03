
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public GameObject player;
    public Transform playerTransform;
    public int distanceFromPlayer;
    NavMeshAgent agent;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.GetComponent<Transform>();
        } 
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        float idealDistance = distanceFromPlayer;

        if (distanceToPlayer < idealDistance) 
        {
            Vector3 directionToPlayer = transform.position - playerTransform.position;
            Vector3 newPosition = playerTransform.position + directionToPlayer.normalized * (idealDistance - 0.5f);
            agent.SetDestination(newPosition);
        }
        else
        {
            agent.SetDestination(playerTransform.position);
        }
    }   
}
