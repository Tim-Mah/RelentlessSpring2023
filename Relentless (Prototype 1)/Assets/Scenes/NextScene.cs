using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public int targetSceneIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            // Singe makes one scene loaded
            SceneManager.LoadScene(targetSceneIndex, LoadSceneMode.Single);
        }
    }
}
