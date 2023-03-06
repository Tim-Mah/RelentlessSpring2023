using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    // x, y, z. Offset y, so that the player is lower
    private Vector3 camPositionOffset = new Vector3(0f, -4f, -3f);
    private float smoothTime = 0.1f;
    private Vector3 velocity = Vector3.zero;

    public Transform player;
    public Rigidbody2D playerRB2;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(player.position.x, 0, -3f) + camPositionOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            }
}
