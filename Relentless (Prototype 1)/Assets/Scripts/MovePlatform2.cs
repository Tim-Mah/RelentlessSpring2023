using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform2 : MonoBehaviour
{
    [SerializeField] private bool active = true;

    private float initDistance;
    private float dist;

    [SerializeField] private float speed = 10f;
    private float initSpeed;

    private float accel;
    [SerializeField] private float decelerationPoint = 0.8f;
    private float step;

    [SerializeField] private GameObject[] waypoints;
    private GameObject currWaypoint;
    [SerializeField] private int initWaypointIndex = 0;
    private int i;

    [SerializeField] private float PauseAtWaypointTime = 1f;
    private float currTime;
    private bool startTime = false;

    private void Start()
    {
        initSpeed = speed;
        initDistance = Vector2.Distance(waypoints[initWaypointIndex].transform.position, waypoints[initWaypointIndex+1].transform.position);
        dist = initDistance * (1 - decelerationPoint);
        accel = (speed * speed) / (-2 * dist);
        currWaypoint = waypoints[initWaypointIndex];
        currTime = PauseAtWaypointTime;

        i = initWaypointIndex;
    }
    void Update()
    {
        if (active)
            {
            float currDistance = Vector2.Distance(currWaypoint.transform.position, transform.position);
            if (currDistance < dist && speed > 0f)
            {
                speed += accel * Time.deltaTime;
                step = speed * Time.deltaTime;
                if (speed > 0f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, currWaypoint.transform.position, step);
                }
                else
                {
                    speed = 0f;
                    i++;
                    if (i == waypoints.Length)
                    {
                        i = 0;
                    }
                    currWaypoint = waypoints[i];
                    startTime = true;

                }
            }
            else if (currDistance > .1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, currWaypoint.transform.position, speed * Time.deltaTime);

            }

            if (startTime)
            {
                currTime -= Time.deltaTime;
                if (currTime < 0f)
                {
                    startTime = false;
                    speed = initSpeed;

                    currTime = PauseAtWaypointTime;
                }
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
             collision.transform.SetParent(transform);
            CapsuleCollider2D collisionBox = collision.GetComponent<CapsuleCollider2D>();
            BoxCollider2D platformBox = this.GetComponent<BoxCollider2D>();
            float collisionTransformY = collision.transform.position.y + collisionBox.offset.y;
            float platformTransformY = transform.position.y + platformBox.offset.y;
            if (collision.transform.position.y > transform.position.y)
            {
                activate();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.SetParent(null);
            
        }
    }

    public void activate()
    {
        active = true;
    }
}
