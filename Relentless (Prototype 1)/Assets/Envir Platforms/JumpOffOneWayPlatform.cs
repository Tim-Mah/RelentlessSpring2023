using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpOffOneWayPlatform : MonoBehaviour
{
    private bool platformFall = false;
    private GameObject currentOneWayPlatform;
    [SerializeField] private BoxCollider2D playerCollider;

    // Update is called once per frame
    void Update()
    {
        if(platformFall)
        {
            if(currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }

    public void PlatformFall(InputAction.CallbackContext context)
    {
        platformFall = context.performed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }

    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        int ground = currentOneWayPlatform.layer;
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
