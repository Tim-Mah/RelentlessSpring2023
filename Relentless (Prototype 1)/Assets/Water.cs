using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public float waterSpeed = 0.1f;
    public float waterHeight = 2.0f;

    private Rigidbody2D playerRidgidbody; 

    // Start is called before the first frame update
    void Start()
    {
        playerRidgidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRidgidbody.position.y < transform.position.y + waterHeight) {
            playerRidgidbody.AddForce(Vector2.up * waterSpeed * Time.fixedDeltaTime);
        }
    }
}
