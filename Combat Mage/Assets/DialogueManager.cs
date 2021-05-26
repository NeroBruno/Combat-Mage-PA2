using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator animator;

    // This variable will keep track of all of the sentences in our Crow Dialogue
    private Queue<string> sentences; // a Queue works in many ways like a list, but a bit more restrictive (FIFO collection)


    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }
    public void StartDialogue (Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);

        nameText.text = dialogue.name;

        // clear any sentences that were there from a previous conversation
        sentences.Clear();

        // Go through all of the strings(Crow texts) in our dialogue.senteces array with a foreach
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence); // here we "Queue up" a sentence
        }

        DisplayNextSentence();
    }

    // make this method public to call it from the Continue button on the Dialogue box
    public void DisplayNextSentence ()
    {
        if (sentences.Count == 0) // if this is true, we have no more sentences so we end conversation (CrowDialogue box dissapears)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue(); // if we have more sentences left to say, we get the next sentence in the queue
        StopAllCoroutines(); // this makes sure that if TypeSentence is already running, it will stop doing so and it starts a new one
        StartCoroutine(TypeSentence(sentence));
    }

    // this animates the letters 1 by 1 when the sentence is being written
    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()) // loop through all the individual characters in the sentence. ToCharArray converts a string into a character array
        {
            dialogueText.text += letter; // add a letter into our dialogueText 1 by 1
            yield return null; // wait a single frame for each letter
        }
    }

    void EndDialogue ()
    {
        animator.SetBool("isOpen", false);
    }
}
