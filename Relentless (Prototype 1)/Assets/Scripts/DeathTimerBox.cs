using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTimerBox : MonoBehaviour
{
    [SerializeField] private float SecondsToDie;
    private float TimeLeft;
    private bool active;

    [SerializeField] private GameObject player;
    private PlayerMovement playerMovement;
    SpriteRenderer sprite;
    private float ColorTick;
    private float TickCount;
    GameObject respawn;
    Respawn respawncode;

    // Start is called before the first frame update
    void Start()
    {
        respawn = GameObject.FindGameObjectWithTag("Respawn");
        respawncode = respawn.GetComponent<Respawn>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.disableDash();

        active = false;
        TimeLeft = SecondsToDie;
        sprite = player.GetComponent<SpriteRenderer>();
        ColorTick = (float)(0.01 * SecondsToDie);
        TickCount = 1;
        sprite.color = new Color(1, 1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!respawncode.respawned)
        {
            sprite.color = new Color(1, 1, 1, 0);
        }
        else if (active)
        {
            if(TimeLeft > 0f)
            {
                if(TimeLeft< (SecondsToDie - (ColorTick * TickCount)))
                {
                    float ColorDiff = 1 - ((ColorTick * TickCount)/ SecondsToDie);
                    sprite.color = new Color(1, ColorDiff, ColorDiff, 1);
                    TickCount++;
                }
                TimeLeft -= Time.deltaTime;
                Debug.Log(TimeLeft + "" + TickCount);
            }
            else
            {
                Debug.Log("DEAD");
                sprite.color = new Color(0, 0, 0, 0);
                TimeLeft = 0f;      
                respawncode.respawn();
                TickCount = 0;
                TimeLeft = SecondsToDie;
            }
        }
        else
        {
            if(TimeLeft < SecondsToDie)
            {
                if (TimeLeft > (SecondsToDie - (ColorTick * TickCount)))
                {
                    float ColorDiff = 1 - ((ColorTick * TickCount) / SecondsToDie);
                    sprite.color = new Color(1, ColorDiff, ColorDiff, 1);
                    TickCount -= 2;
                }

                TimeLeft += (Time.deltaTime * 2);
                Debug.Log(TimeLeft);

            }
            else
            {
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerMovement.enableDash();
            active = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerMovement.disableDash();
            active = false;
        }
    }
}
