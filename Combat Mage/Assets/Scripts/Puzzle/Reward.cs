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
    static int GreenPuzzleSol;
   
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
        //Checks if both planes are in the respective colors.If yes, then the Eather stone will appired
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
        GreenPuzzleSol += 1;
        if (GreenPuzzleSol==3)
        {
            //Ative o game Object
            Telport.GetComponent<Animator>().SetTrigger("Open");
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
