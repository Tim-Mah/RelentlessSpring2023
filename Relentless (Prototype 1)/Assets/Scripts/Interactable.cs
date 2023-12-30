using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Dialogue dialogue;
    public DialogueManager dialogueManager;
    public StartCutscene startCutscene;
    [SerializeField] public bool isCutscene = false;
    private bool cutsceneStarted = false;

    void Start()
    {

        dialogueManager = FindObjectOfType<DialogueManager>();
        if (!isCutscene)
        {
            StartInteractable();
        }
    }

    public void StartInteractable()
    {
        cutsceneStarted = true;
        if (isCutscene)
        {
            dialogueManager.isGameCutscene = true;
            dialogueManager.startCutscene = this.startCutscene;
        }
        dialogueManager.StartDialogue(dialogue);
    }

    void Update()
    {
        if (cutsceneStarted)
        {
            if (Input.GetKeyDown("space"))
            {
                dialogueManager.DisplayNextSentence();
            }
        }

    }
}
