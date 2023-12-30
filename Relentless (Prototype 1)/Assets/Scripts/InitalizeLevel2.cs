using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitalizeLevel2 : MonoBehaviour

{
    public bool dashdisabled = false;
    private GameObject player;
    private PlayerMovement playerMovement;

    private GameObject panel;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();

        panel = GameObject.FindWithTag("Panel");
        image = panel.GetComponent<Image>();
        image.color = Color.clear;

        if (dashdisabled) { playerMovement.disableDash(); }
        
    }

}
