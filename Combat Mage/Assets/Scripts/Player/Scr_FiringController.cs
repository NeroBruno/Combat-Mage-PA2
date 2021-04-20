using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_FiringController : MonoBehaviour
{

    public float shootforce, upwardforce,Firerate,CooldownU,CooldownP;
    public bool allowButtonHold;
    public bool AllowFiringInvoke,AllowUtilInvoke,AllowDefenseInvoke;
    public GameObject Projectile;
    public Camera fpsCam;
    public Transform exitPoint;
    bool shooting, ReadyToShoot,ReadyToCastU,ReadyTocastP; 

    // Start is called before the first frame update
    void awake(){
            
    }

    private void Start() {
        ReadyToShoot = true;
        AllowFiringInvoke = true;

        ReadyTocastP = true;
        AllowDefenseInvoke = true;

        ReadyToCastU = true;
        AllowUtilInvoke = true;
    }
    
    // Update is called once per frame
    void Update()
    {     
      Myinput();   
    }

    void shoot()
    {
            ReadyToShoot = false;
            
            Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
            RaycastHit hit; 

            Vector3 TargetPoint;
            if(Physics.Raycast(ray, out hit)){
                TargetPoint = hit.point;
            }else{
                TargetPoint = ray.GetPoint(75);
            }

            Vector3 FiringDirection = TargetPoint - exitPoint.position;
            GameObject CurrProjectile = Instantiate(Projectile,exitPoint.position,Quaternion.identity);
            CurrProjectile.transform.forward = FiringDirection.normalized;

            CurrProjectile.GetComponent<Rigidbody>().AddForce(FiringDirection * shootforce,ForceMode.Impulse);

            if(AllowFiringInvoke){
                Invoke("ResetShot",Firerate);
                AllowFiringInvoke = false;
            }           
    }

    void ResetShot()
    {
        ReadyToShoot = true;
        AllowFiringInvoke = true;
    }

    void ResetUtil()
    {
        ReadyToCastU = true;
        AllowUtilInvoke = true;
    }

    void ResetDefense()
    {
        ReadyTocastP = true;
        AllowDefenseInvoke = true;
    }


    void Myinput(){
       //Projectile
        if(allowButtonHold){
            shooting = Input.GetKey(KeyCode.Mouse0);
        }else{ 
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

        Debug.Log(shooting);
        Debug.Log(ReadyToShoot);
        if(ReadyToShoot && shooting){
               
                shoot();
        }

        //spells
    }
}
