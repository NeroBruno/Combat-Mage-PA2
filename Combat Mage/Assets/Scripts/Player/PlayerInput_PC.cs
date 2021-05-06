using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles player input and feeds it to the other components
/// </summary>
public class PlayerInput_PC : PlayerComponent
{
    private void Update()
    {
        if (!Player.Pause.Active && Player.ViewLocked.Is(false))
        {
            //Movement
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Player.MoveInput.Set(moveInput);

            //Look
            Player.LookInput.Set(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")));
            Player.WantsToInteract.Set(Input.GetButton("Interact"));

            //Jump
            if (Input.GetButtonDown("Jump"))
                Player.Jump.TryStart();

            if (Input.GetButtonDown("Dash"))
                Player.Dash.TryStart();

            // Spell Binding
            // FIRE
            if (Input.GetButtonDown("FireRune") || Input.GetButton("FireRune"))
            {
                if (Input.GetButtonDown("AttackSpell"))
                    Player.CurrentAttackElement.Set(DamageType.Fire);
                else if (Input.GetButtonDown("DefenseSpell"))
                    Player.CurrentDefenseElement.Set(DamageType.Fire);
                else if (Input.GetButtonDown("SupportSpell"))
                    Player.CurrentUtilityElement.Set(DamageType.Fire);
            }
            // AIR
            else if (Input.GetButtonDown("AirRune") || Input.GetButton("AirRune"))   
            {
                if (Input.GetButtonDown("AttackSpell"))
                    Player.CurrentAttackElement.Set(DamageType.Air);
                else if (Input.GetButtonDown("DefenseSpell"))
                    Player.CurrentDefenseElement.Set(DamageType.Air);
                else if (Input.GetButtonDown("SupportSpell"))
                    Player.CurrentUtilityElement.Set(DamageType.Air);
            }
            // EARTH
            else if ((Input.GetButtonDown("EarthRune") || Input.GetButton("EarthRune")) && Player.HasEarthRune.Get())  
            {
                if (Input.GetButtonDown("AttackSpell"))
                    Player.CurrentAttackElement.Set(DamageType.Earth);
                else if (Input.GetButtonDown("DefenseSpell"))
                    Player.CurrentDefenseElement.Set(DamageType.Earth);
                else if (Input.GetButtonDown("SupportSpell"))
                    Player.CurrentUtilityElement.Set(DamageType.Earth);
            }
            // WATER
            else if ((Input.GetButtonDown("WaterRune") || Input.GetButton("WaterRune")) && Player.HasWaterRune.Get())  
            {
                if (Input.GetButtonDown("AttackSpell"))
                    Player.CurrentAttackElement.Set(DamageType.Water);
                else if (Input.GetButtonDown("DefenseSpell"))
                    Player.CurrentDefenseElement.Set(DamageType.Water);
                else if (Input.GetButtonDown("SupportSpell"))
                    Player.CurrentUtilityElement.Set(DamageType.Water);
            }

            // more stuff

            // Take Damage test
            if (Input.GetKeyDown(KeyCode.I))
            {
                HealthEventData damage = new HealthEventData(-100, DamageType.Fire);
                Player.ChangeHealth.Try(damage);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                HealthEventData damage = new HealthEventData(-20, DamageType.Earth, sourcetest);
                Player.ChangeHealth.Try(damage);
            }

            // Suicide damage testing
            if (Input.GetKeyDown(KeyCode.K))
            {
                HealthEventData damage = new HealthEventData(-1000f);
                Player.ChangeHealth.Try(damage);
            }

        }
        else
        {
            //Movement
            Player.MoveInput.Set(Vector2.zero);

            //Look
            Player.LookInput.Set(Vector2.zero);
        }
    }

    public LivingEntity sourcetest = null;
}
