using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audioscript : MonoBehaviour
{
    private static Audioscript instance;
    [SerializeField] private int index = 1;
    
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
