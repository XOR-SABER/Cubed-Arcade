using UnityEngine;

public class CameraFollowUp : MonoBehaviour
{
    public bool followOnX;
    public bool folloxOnY;

    public float yOffset;
    public float xOffset;

    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        var pos = transform.position;
        var position = player.transform.position;
        pos.x = position.x + xOffset;
        pos.y = position.y + yOffset;
        if (!followOnX)
        {
            pos.x = xOffset;
        }

        if (!folloxOnY)
        {
            pos.y = yOffset;
        }
        transform.position = pos;
    }
}
