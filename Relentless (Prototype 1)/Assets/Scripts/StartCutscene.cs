using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCutscene : MonoBehaviour
{
    [SerializeField] private Animator camAnimator;
    [SerializeField] public Interactable interactable;

    private GameObject player;
    private GameObject panel;
    private PlayerMovement playerMovement;
    private Image image;

    [SerializeField] private string cutsceneplusindex;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        panel = GameObject.FindWithTag("Panel");
        playerMovement = player.GetComponent<PlayerMovement>();
        image = panel.GetComponent<Image>();
        playerMovement.cutsceneActive = false;

        interactable.startCutscene = this;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            image.color = new Color(1, 1, 1, 0.5f);
            camAnimator.SetBool(cutsceneplusindex, true);
            playerMovement.cutsceneActive = true;
            interactable.StartInteractable();

        }
    }

    public void StopCutscene()
    {
        camAnimator.SetBool(cutsceneplusindex, false);
        playerMovement.cutsceneActive = false;
        image.color = Color.clear;
        playerMovement.cutsceneActive = false;
        Destroy(gameObject);
    }
}
