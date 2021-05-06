using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneInteraction : MonoBehaviour
{
    public PlayerInput_PC input;
    public enum typeenum
    {
        Water,
        Fire,
        Eather,
        Air
    }

    public typeenum elemental;
 

    public void InterativeRune()
    {
       switch (elemental)
            {
            case typeenum.Water:
                input.hasWaterRune = true;
                break;
                
        }
    }
}
