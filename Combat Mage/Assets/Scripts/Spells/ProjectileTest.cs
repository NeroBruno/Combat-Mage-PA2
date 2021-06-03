using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTest : MonoBehaviour
{
    //public Player Player;

    private bool isCollided;

    //private float _Force = 55f;

    private void OnCollisionEnter(Collision collision)
    {
        // NOT WORKING THIS WAY
        //if (collision.gameObject.tag == "Enemy")
        //{
        //    var damageable = collision.gameObject.gameObject.GetComponent<IDamageable>();

        //    if (damageable != null)
        //    {
        //        var damageData = new HealthEventData(-25f, Player.CurrentAttackElement.Get(), transform.position, (collision.gameObject.transform.position - transform.position).normalized, _Force, Vector3.zero, Player);
        //        damageable.TakeDamage(damageData);
        //    }

        //    Destroy(gameObject);
        //}
        if (collision.gameObject.tag == "Minion")
        {
            collision.gameObject.GetComponent<AI_Health_src>().takeDamage(25);
        }
        if (collision.gameObject.tag != "Projectile" && collision.gameObject.tag != "Player" && !isCollided)
        {
            Destroy(gameObject);
        }
    }

    //public Transform firePoint;
    //public Camera cam;
    //public float projectileSpeed = 30f;
    //public float fireRate = 4;
    //public float arcRange = 1;

    //private GameObject destination;


    //private void ShootProjectile()
    //{
    //    Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit))
    //        destination = hit.transform.gameObject;
    //    else
    //        destination = ray.GetPoint(1000);

    //    InstantiateProjectile(firePoint);
    //}

    //private void InstantiateProjectile(Transform firePoint)
    //{
    //    var projectileObj = Instantiate(_CurrentProjectile, firePoint.position, Quaternion.identity);

    //    // Logic for each spell
    //    if (Player.CurrentAttackElement.Get() == DamageType.Fire)
    //    {
    //        projectileObj.GetComponent<Rigidbody>().velocity = (destination - firePoint.position).normalized * projectileSpeed;
    //    }
    //    else if (Player.CurrentAttackElement.Get() == DamageType.Air)
    //    {
    //        projectileObj.GetComponent<Rigidbody>().velocity = (destination - firePoint.position).normalized * projectileSpeed;

    //        iTween.PunchPosition(projectileObj, new Vector3(Random.Range(arcRange, arcRange), Random.Range(arcRange, arcRange), 0), Random.Range(0.5f, 4.5f));
    //    }
    //    else if (Player.CurrentAttackElement.Get() == DamageType.Earth)
    //    {
    //        projectileObj.GetComponent<Rigidbody>().velocity = (destination - firePoint.position).normalized * projectileSpeed;
    //    }
    //    else if (Player.CurrentAttackElement.Get() == DamageType.Water)
    //    {
    //        projectileObj.GetComponent<Rigidbody>().velocity = (destination - firePoint.position).normalized * projectileSpeed;
    //        iTween.PunchPosition(projectileObj, new Vector3(Random.Range(arcRange, arcRange), Random.Range(arcRange, arcRange), 0), Random.Range(0.5f, 3.5f));
    //    }
    //}
}
