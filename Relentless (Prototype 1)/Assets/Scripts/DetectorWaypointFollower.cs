using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorWaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject waypoint;
    private bool active;
    private float initDistance;
    private float dist;

    [SerializeField] private float speed = 10f;
    private float accel;
    [SerializeField] private float decelerationPoint = 0.8f;
    private float step;

    private void Start()
    {
        initDistance = Vector2.Distance(waypoint.transform.position, transform.position);
        dist = initDistance * (1-decelerationPoint);
        accel = (speed * speed) / (-2 * dist);
        active = false;
        
        
    }
    void Update()
    {
        if (active)
        {
            float currDistance = Vector2.Distance(waypoint.transform.position, transform.position);
            if (currDistance < dist && speed > 0f)
            {
                speed += accel * Time.deltaTime; 
                step = speed * Time.deltaTime; 
                if (speed > 0f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, waypoint.transform.position, step);
                }
                else
                {
                    speed = 0f;
                }
                
            } 
            else if (currDistance > .1f)
            {   
                transform.position = Vector2.MoveTowards(transform.position, waypoint.transform.position, speed * Time.deltaTime);

            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.SetParent(transform);
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
