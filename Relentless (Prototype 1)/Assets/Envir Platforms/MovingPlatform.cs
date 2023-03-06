using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Rigidbody2D platform;
    public Transform start;
    public Transform end;
    public float speed = 7f;
    private int direction = 1;

    private void OnDrawGizmos()
    {
        if(platform != null && start != null && end != null)
        {
            Gizmos.DrawLine(platform.transform.position, start.transform.position);
            Gizmos.DrawLine(platform.transform.position, end.transform.position);
        }
    }

    private void FixedUpdate()
    {
        Vector2 target = MoveTarget();
        //platform.MovePosition(Vector2.MoveTowards(platform.position, target, speed * Time.deltaTime));
        //float distance = (target - platform.position).magnitude;
        Vector2 distance = (target - platform.position);

        if (distance.magnitude <= 0.1f)
        {
            direction *= -1;
        }else
        {
            platform.velocity = distance * speed * Time.deltaTime;
        }
    }

    private Vector2 MoveTarget()
    {
        if(direction ==1)
        {
            return start.position;
        }else
        {
            return end.position;
        }
    }
}

