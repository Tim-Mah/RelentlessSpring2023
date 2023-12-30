using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    // x, y, z. Offset y, so that the player is lower
    private Vector3 camPositionOffset = new Vector3(0f, 4f, -37f);
    private Vector3 airPositionOffset = new Vector3(0f, 0f, -37f);
    private float smoothTime = 0.1f;
    private Vector3 velocity = Vector3.zero;

    public Transform player;
    public Rigidbody2D playerRB2;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player.position.y >= 5 || player.position.x >= 370)
        {
                Vector3 targetPosition = new Vector3(player.position.x, player.position.y, -4.5f) + airPositionOffset;
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }

        else
        {
            Vector3 targetPosition = new Vector3(player.position.x, 0, -4.5f) + camPositionOffset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        
            }
}
