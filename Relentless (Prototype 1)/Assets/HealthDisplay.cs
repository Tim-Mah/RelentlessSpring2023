using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private int playerHealth;
    [SerializeField] private Image[] hearts;

    void Update()
    {
        for(int i = 0; i < hearts.Length; i++) {
            if (i < playerHealth) {
                hearts[i].color = Color.red;
            }
            else {
                hearts[i].color = Color.black;
            }
        }
    }
}
