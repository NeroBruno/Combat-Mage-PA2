using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    public bool Red;
    public bool Blue;
    public GameObject Eather;

    // Start is called before the first frame update
    void Start()
    {
        Red = false;
        Blue = false;
    }

    public void RedPuzzle()
    {
        Red = true;
        if(Red==true && Blue==true)
        {
            //Ative o game Object
            Eather.SetActive(true);
        }
    }

    public void BluePuzzle()
    {
        Blue = true;
        if (Red == true && Blue == true)
        {
            //Ative o game Object
            Eather.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
