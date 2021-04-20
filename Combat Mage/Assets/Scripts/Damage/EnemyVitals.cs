using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVitals : EntityVitals
{
    public GameObject ObjectEnemy;
    public Enemy Enemy
    {
        get
        {
            if (!_Enemy)
                _Enemy = GetComponent<Enemy>();
            if (!_Enemy)
                _Enemy = GetComponentInParent<Enemy>();

            return _Enemy;
        }
    }

    private Enemy _Enemy;

    protected override void Update()
    {
        base.Update();

        if (Enemy.Health.Get() == 0f)
            Destroy(ObjectEnemy);

        //if (_ManaRegeneration.CanRegenerate && Player.Mana.Get() < 100f)
        //    ModifyMana(_ManaRegeneration.RegenDelta);

        //if (Player.SpellDefend.Active)
        //{
        //    _ManaRegeneration.Pause();
        //    if (_DefenseElement == DamageType.Fire)
        //        SetFireResistance();
        //    else if (_DefenseElement == DamageType.Air)
        //        SetAirResistance();
        //    else if (_DefenseElement == DamageType.Earth)
        //        SetEarthResistance();
        //    else if (_DefenseElement == DamageType.Water)
        //        SetWaterResistance();

        //    ModifyMana(-_ManaDepletionRate * Time.deltaTime);
        //}
        //else
        //    SetNormalResistance();

        //if (!_StaminaRegeneration.CanRegenerate && Player.Stamina.Is(0f) && Time.time - _LastHeavyBreathTime > _BreathingHeavyDuration)
        //{
        //    _LastHeavyBreathTime = Time.time;
        //    _BreathingHeavyAudio.Play2D();
        //}

        //AudioListener.volume = Mathf.MoveTowards(AudioListener.volume, 1f, _EarRingVolumeGainSpeed * Time.deltaTime);
    }

    protected override bool Try_ChangeHealth(HealthEventData healthEventData)
    {
        return base.Try_ChangeHealth(healthEventData);
    }

    protected override void Start()
    {
        //Player.SpellDefend.AddStartTryer(() => { _ManaRegeneration.Pause(); return Player.Mana.Get() > 0f; });
        //Player.SpellAttack.AddListener(OnSpellAttack);
        //Player.Crouch.AddStartListener(OnCrouchStart);
        //Player.Crouch.AddStopListener(OnCrouchEnd);

        //ShakeManager.ShakeEvent.AddListener(OnShakeEvent);
    }

    private void OnDestroy()
    {
        //ShakeManager.ShakeEvent.RemoveListener(OnShakeEvent);
    }
}
