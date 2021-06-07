using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorToolkit;

public class Sumoner_scr : MonoBehaviour
{
    [SerializeField] RangeSensor sans;
    bool detected;
    [SerializeField] float sTime;
    [SerializeField] Transform sLocation;
    [SerializeField] GameObject PrefabToSpawn;
    float cTime;

    // Spider Egg (Texture and animation)

    [SerializeField] EggController spiderEgg;

    // Start is called before the first frame update
    void Start()
    {
        spiderEgg = GetComponentInChildren<EggController>();
        cTime = 0f;
        detected = false;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (!detected)
        { 
           
            var det = sans.GetNearest();
            if(det != null)
            {
                detected = true;
                spiderEgg.Destroy();
                Debug.Log("hello");
            }
        }
        else {

            if (cTime > sTime)
            {
                cTime = 0;
                detected = false;
                Spawn();
            }
            else {
                cTime += Time.deltaTime;
            }
        
        }
    }

    void Spawn()
    {
        //GameObject prefab;
        //prefab = PrefabToSpawn;
        Instantiate(PrefabToSpawn, sLocation);
        Debug.Log("Trying to spawn");
    }

}
