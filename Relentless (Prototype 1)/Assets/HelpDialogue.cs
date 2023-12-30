using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpDialogue : MonoBehaviour
{
    bool isDialogue;
    public Dialogue dialogue;
    public Interactable interactable;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Dialogue")) {
            isDialogue = true;
        }
        else {
            isDialogue = false;
        }
    }

    void Update() {
        if(isDialogue) {
            FindObjectOfType<HelpManager>().StartDialogue(dialogue);
        }
    }
}
