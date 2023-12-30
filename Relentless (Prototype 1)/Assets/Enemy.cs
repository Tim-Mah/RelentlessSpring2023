using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isStand;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            isStand = true;
        }

       if(isStand) {
        
       }
    }
}
