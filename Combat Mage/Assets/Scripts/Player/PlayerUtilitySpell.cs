using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerUtilitySpell : PlayerComponent
{
    //Fire Spell - Sticky Firebomb that explodes and deals damage in a radius after a few seconds
    public GameObject Firespell;

    //Air Spell - AOE Push, FUS RO DAH
    public GameObject Airspell;

    //Earth Spell - Guiding Wisp, Spawns a wisp that shows the way to the next objective
    public GameObject Earthspell;

    //Water Spell - Healing Water, Heals the player for some time
    public GameObject Waterspell;

    private GameObject _CurrentSpell;

    public Transform firePoint;
    public Camera cam;
    public float projectileSpeed = 30f;
    public float fireRate = 4;
    public float arcRange = 1;

    private Vector3 destination;
    private float timeToFire;

    private void Awake()
    {
        
    }

    private void Start()
    {
        Player.SpellUtility.AddListener(StartUtility);
        Player.SpellUtility.SetTryer(TryUtility);
        Player.CurrentUtilityElement.AddChangeListener(ChangeElement);
        _CurrentSpell = Firespell;
    }

    private bool TryUtility()
    {
        if (Player.Mana.Get() >= 15)
            return true;
        else
            return false;
    }

    private void Update()
    {
        if (!Input.GetButton("FireRune") && !Input.GetButton("AirRune") && !Input.GetButton("EarthRune") && !Input.GetButton("WaterRune"))
        {
            //// Utility SPELL
            if (Input.GetButtonDown("SupportSpell") && Time.time >= timeToFire)
            {
                Player.SpellUtility.Try();
            }
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void StartUtility()
    {
        timeToFire = Time.time + 1 / fireRate;
        ShootProjectile();
    }

    private void ChangeElement(DamageType obj)
    {
        // Set the base color of the shield spell
        // Eventually will choose what element attack spell to use
        if (obj == DamageType.Fire)
        {
            _CurrentSpell = Firespell;
        }
        else if (obj == DamageType.Air)
        {
            _CurrentSpell = Airspell;
        }
        else if (obj == DamageType.Earth)
        {
            _CurrentSpell = Earthspell;
        }
        else if (obj == DamageType.Water)
        {
            _CurrentSpell = Waterspell;
        }
    }

    private void ShootProjectile()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
            destination = hit.point;
        else
            destination = ray.GetPoint(1000);

        InstantiateProjectile(firePoint);
    }

    private void InstantiateProjectile(Transform firePoint)
    {
        var projectileObj = Instantiate(_CurrentSpell, firePoint.position, Quaternion.identity);

        // Logic for each spell
        if (Player.CurrentAttackElement.Get() == DamageType.Fire)
        {
            projectileObj.GetComponent<Rigidbody>().velocity = (destination - firePoint.position).normalized * projectileSpeed;
        }
        else if (Player.CurrentAttackElement.Get() == DamageType.Air)
        {
            projectileObj.GetComponent<Rigidbody>().velocity = (destination - firePoint.position).normalized * projectileSpeed;

            iTween.PunchPosition(projectileObj, new Vector3(Random.Range(arcRange, arcRange), Random.Range(arcRange, arcRange), 0), Random.Range(0.5f, 4.5f));
        }
        else if (Player.CurrentAttackElement.Get() == DamageType.Earth)
        {
            projectileObj.GetComponent<Rigidbody>().velocity = (destination - firePoint.position).normalized * projectileSpeed;
        }
        else if (Player.CurrentAttackElement.Get() == DamageType.Water)
        {
            projectileObj.GetComponent<Rigidbody>().velocity = (destination - firePoint.position).normalized * projectileSpeed;
            iTween.PunchPosition(projectileObj, new Vector3(Random.Range(arcRange, arcRange), Random.Range(arcRange, arcRange), 0), Random.Range(0.5f, 3.5f));
        }

    }
}
