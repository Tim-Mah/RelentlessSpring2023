using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectAndActivateMovable : MonoBehaviour
{
    [SerializeField] private GameObject activateObject;
    private DetectorWaypointFollower detectorWaypointFollower;
    private AudioSource detectSound;
    

    private void Start()
    {
        Debug.Log("start");
        detectSound = GetComponent<AudioSource>();
        detectorWaypointFollower = activateObject.GetComponent<DetectorWaypointFollower>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {          
            detectorWaypointFollower.activate();
            Debug.Log("detected");
        }
        ;
    }


}
