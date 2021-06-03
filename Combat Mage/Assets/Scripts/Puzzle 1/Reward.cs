using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    public bool Red;
    public bool Blue;
    public bool Green1;
    public bool Green2;
    public bool Green3;
   
    public GameObject Eather;
    public GameObject Telport;
    // Start is called before the first frame update
    void Start()
    {
        Red = false;
        Blue = false;
        Green1 = false;
        Green2 = false;
        Green3 = false;
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


    public void GreenPuzzle()
    {
        Green1 = true;
        Green2 = true;
        Green3 = true;
        if (Green1 == true)
        {
            //Ative o game Object
            Telport.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
