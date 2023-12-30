using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameObject player;
    private PlayerMovement playerMovement;
    private HealthController healthController;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        healthController = player.GetComponent<HealthController>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log(transform.position);
            playerMovement.checkpoint = (Vector2)(transform.position) + (new Vector2(0,1));
            Debug.Log(playerMovement.checkpoint);
            healthController.Start();
        }
    }
}
