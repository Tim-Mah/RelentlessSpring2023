using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 1f;
    public float destroyDelay = 2f;
    private Vector2 initialPos;

    public Rigidbody2D rb2;

    private void Start()
    {
        initialPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb2.bodyType = RigidbodyType2D.Dynamic;
        
    }

    public void respawn()
    {
        StopCoroutine(Fall());
        rb2.velocity = new Vector2(0, 0);
        rb2.bodyType = RigidbodyType2D.Kinematic;
        transform.position = initialPos;
    }
}

