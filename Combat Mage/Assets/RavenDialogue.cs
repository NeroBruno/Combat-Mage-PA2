using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Call this function RavenDialogue to Trigger a dialogue anywhere, like when a Player gets inside a certain radius, or when he discovers an object, starts the game or dies

public class RavenDialogue : MonoBehaviour
{

    public Dialogue dialogue;

    public void TriggerDialogue ()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
