using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audioscript : MonoBehaviour
{
    private static Audioscript instance;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
    }
}
