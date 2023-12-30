using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private GameObject player;
    private PlayerMovement playerMovement;
    private HealthController healthController;

    public bool respawned = true;

    [SerializeField] GameObject[] fallingPlats;
    private Animator animator;
    [SerializeField] float respawnTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        healthController = player.GetComponent<HealthController>();

        GameObject fadeImage = GameObject.FindWithTag("FadeImage");
        animator = fadeImage.GetComponent<Animator>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            respawn();
        }
    }

    public void respawn()
    {
        StartCoroutine(respawnAnim());
        
        
        foreach(GameObject i in fallingPlats)
        {
            FallingPlatform fallScript = i.GetComponent<FallingPlatform>();
            fallScript.respawn();
        }
    }

    IEnumerator respawnAnim()
    {
        if(!(animator is null))
        {
            SpriteRenderer sprite = player.GetComponent<SpriteRenderer>();
            respawned = false;
            sprite.color = new Color(1, 1, 1, 0);
            animator.SetBool("FadeIn", false);
            
            
            yield return new WaitForSeconds(respawnTime);
            playerMovement.transform.position = playerMovement.checkpoint;
            healthController.Start();
            sprite.color = new Color(1, 1, 1, 1);
            animator.SetBool("FadeIn", true);

            respawned = true;
        }
        else
        {
            yield return new WaitForSeconds(respawnTime);
            playerMovement.transform.position = playerMovement.checkpoint;
            healthController.Start();
        }
        

    }
}
