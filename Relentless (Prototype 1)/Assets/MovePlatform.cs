using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{

    private int direction = 1;
    [SerializeField] private float speed = 1f;
    public Rigidbody2D platform;
    public Transform start;
    public Transform end;
    private static Vector2 moveTarget;
    private bool hitTrigger = false;

    private void Start()
    {
        moveTarget = new Vector2(start.position.x, start.position.y) - new Vector2(platform.position.x, platform.position.y);
        platform.velocity = moveTarget * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MovingPlatformPosts") && !hitTrigger)
        {
            hitTrigger = true;
            //print("Entered trigger");
            direction *= -1;
            if (direction == 1)
            {
                moveTarget = new Vector2(start.position.x, start.position.y) - new Vector2(platform.position.x, platform.position.y);
            }
            else
            {
                moveTarget = new Vector2(end.position.x, end.position.y) - new Vector2(platform.position.x, platform.position.y);
            }
            platform.velocity = moveTarget * speed;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("MovingPlatformPosts"))
        {
            hitTrigger = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (platform != null && start != null && end != null)
        {
            Gizmos.DrawLine(platform.transform.position, start.transform.position);
            Gizmos.DrawLine(platform.transform.position, end.transform.position);
        }
    }
}
