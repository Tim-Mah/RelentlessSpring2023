using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Dialogue dialogue;
    public HelpManager helpManager;

    void Start() {
        FindObjectOfType<HelpManager>().StartDialogue(dialogue);
    }

    void Update() {
        if(Input.GetKeyDown("space")) {
            FindObjectOfType<HelpManager>().DisplayNextSentence();
        }
    }
}
