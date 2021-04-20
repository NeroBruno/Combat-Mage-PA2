using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefenseSpell : PlayerComponent
{
    // Player spell stuff and logic

    public GameObject DebugSpellDefend;

    [SerializeField]
    private Material _Color;

    private void Awake()
    {
        _Color.color = Color.red;
    }

    private void Start()
    {
        Player.SpellDefend.AddStartListener(StartDefend);
        Player.SpellDefend.AddStopListener(StopDefend);
        Player.CurrentDefenseElement.AddChangeListener(ChangeElement);
    }

    private void Update()
    {
        if (!Input.GetButton("FireRune") && !Input.GetButton("AirRune") && !Input.GetButton("EarthRune") && !Input.GetButton("WaterRune"))
        {
            // DEFENSE SPELL
            if (Player.Mana.Get() >= 8 && Input.GetButton("DefenseSpell") && (!Input.GetButton("AttackSpell") || !Input.GetButtonDown("AttackSpell")))
            {
                Player.SpellDefend.TryStart();
            }
            else if (Player.Mana.Get() == 0 || !Input.GetButton("DefenseSpell") && (!Input.GetButton("AttackSpell") || !Input.GetButtonDown("AttackSpell")))
            {
                Player.SpellDefend.ForceStop();
            }
            else if (Input.GetButtonUp("DefenseSpell"))
                Player.SpellDefend.ForceStop();
        }
        else if (Input.GetButton("FireRune") || Input.GetButton("AirRune") || Input.GetButton("EarthRune") || Input.GetButton("WaterRune") && Player.SpellDefend.Active)
        {
            Player.SpellDefend.ForceStop();
        }
    }
    private void StartDefend()
    {
        // Activate the shield
        DebugSpellDefend.SetActive(true);
    }

    private void StopDefend()
    {
        // Deactivate the shield
        DebugSpellDefend.SetActive(false);
    }

    private void ChangeElement(DamageType obj)
    {
        // Set the base color of the shield spell
        // Eventually will choose what element defense spell to use
        if (obj == DamageType.Fire)
            _Color.color = Color.red;
        else if (obj == DamageType.Air)
            _Color.color = Color.grey;
        else if (obj == DamageType.Earth)
            _Color.color = Color.green;
        else if (obj == DamageType.Water)
            _Color.color = Color.blue;
    }
}
