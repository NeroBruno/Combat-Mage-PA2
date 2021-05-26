using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Let's use this Dialogue Class as an Object that we can pass into our DialogueManager whenever we want to start a new Dialogue
// If we want a Class like this to show in the Inspector and we are able to change it, we need to mark it as Serializable
[System.Serializable]
public class Dialogue
{
    public string name; // this is our Crow (or any other pet/NPC name)

    [TextArea(3, 10)] // minimum amount of lines used for a Dialogue is 3 and maximum is 10
    public string[] sentences; // array of strings which will be our sentences
}
