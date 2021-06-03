using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline : MonoBehaviour
{

    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform pointC;
    private float polateAmond;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        //Creates a Line Spline.It allows to the middle cube(point C) to go point A to B.It creates a Vector 3 with has the posicton of A,B and 
        polateAmond = (polateAmond + Time.deltaTime) % 1;
        
        pointC.position =Vector3.Lerp(pointA.position,pointB.position,polateAmond);
    
    }
}
