using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpOffOneWayPlatform : MonoBehaviour
{
    private bool platformFall = false;
    private GameObject currentOneWayPlatform;
    [SerializeField] private CapsuleCollider2D playerCollider;

    // Update is called once per frame
    void Update()
    {
        if(platformFall)
        {
            if(currentOneWayPlatform != null && playerCollider != null)
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
        if(collision.gameObject.CompareTag("OneWayPlatform") || collision.gameObject.CompareTag("oneWayRidable"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform") || collision.gameObject.CompareTag("oneWayRidable"))
        {
            currentOneWayPlatform = null;
        }

    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        int ground = currentOneWayPlatform.layer;
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
