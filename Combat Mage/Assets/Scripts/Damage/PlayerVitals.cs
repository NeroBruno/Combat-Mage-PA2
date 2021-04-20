using System;
using UnityEngine;

public class PlayerVitals : EntityVitals
{
    public Player Player
    {
        get
        {
            if (!_Player)
                _Player = GetComponent<Player>();
            if (!_Player)
                _Player = GetComponentInParent<Player>();

            return _Player;
        }
    }

    [SerializeField]
    [Range(0f, 300f)]
    private float _ManaDepletionRate = 30f;

    [SerializeField]
    private StatRegenData _ManaRegeneration = null;

    [SerializeField]
    private SoundPlayer _BreathingHeavyAudio = null;

    [SerializeField]
    private float _BreathingHeavyDuration = 11f;

    //[SerializeField]
    //[Range(0f, 100f)]
    //private float _JumpStaminaTake = 15f;

    [SerializeField]
    private SoundPlayer _JumpAudio = null;

    [SerializeField]
    private SoundPlayer _EarRingingAudio = null;

    [SerializeField]
    [Range(0f, 1f)]
    private float _EarRingingAudioVolumeDecrease = 0.5f;

    [SerializeField]
    private float _EarRingVolumeGainSpeed = 0.15f;

    private DamageType _DefenseElement { get => Player.CurrentDefenseElement.Get(); }

    private Player _Player;
    private float _LastHeavyBreathTime;

    protected override void Update()
    {
        base.Update();

        if (_ManaRegeneration.CanRegenerate && Player.Mana.Get() < 100f)
            ModifyMana(_ManaRegeneration.RegenDelta);

        if (Player.SpellDefend.Active)
        {
            _ManaRegeneration.Pause();
            if (_DefenseElement == DamageType.Fire)
                SetFireResistance();
            else if (_DefenseElement == DamageType.Air)
                SetAirResistance();
            else if (_DefenseElement == DamageType.Earth)
                SetEarthResistance();
            else if (_DefenseElement == DamageType.Water)
                SetWaterResistance();

            ModifyMana(-_ManaDepletionRate * Time.deltaTime);
        }
        else
            SetNormalResistance();

        //if (!_StaminaRegeneration.CanRegenerate && Player.Stamina.Is(0f) && Time.time - _LastHeavyBreathTime > _BreathingHeavyDuration)
        //{
        //    _LastHeavyBreathTime = Time.time;
        //    _BreathingHeavyAudio.Play2D();
        //}

        AudioListener.volume = Mathf.MoveTowards(AudioListener.volume, 1f, _EarRingVolumeGainSpeed * Time.deltaTime);
    }

    protected override bool Try_ChangeHealth(HealthEventData healthEventData)
    {
        return base.Try_ChangeHealth(healthEventData);
    }

    protected override void Start()
    {
        Player.SpellDefend.AddStartTryer(() => { _ManaRegeneration.Pause(); return Player.Mana.Get() > 0f; });
        Player.SpellAttack.AddListener(OnSpellAttack);
        Player.SpellUtility.AddListener(OnSpellUtility);
        //Player.Crouch.AddStartListener(OnCrouchStart);
        //Player.Crouch.AddStopListener(OnCrouchEnd);

        //ShakeManager.ShakeEvent.AddListener(OnShakeEvent);
    }

    private void OnSpellUtility()
    {
        _ManaRegeneration.Pause();
        ModifyMana(-15);
    }

    private void OnSpellAttack()
    {
        _ManaRegeneration.Pause();
        ModifyMana(-15);
    }

    private void OnDestroy()
    {
        //ShakeManager.ShakeEvent.RemoveListener(OnShakeEvent);
    }

    //private void OnShakeEvent(ShakeEventData shake)
    //{
    //    if (shake.ShakeType == ShakeType.Explosion)
    //    {
    //        float distToExplosionSqr = (transform.position - shake.Position).sqrMagnitude;
    //        float explosionRadiusSqr = shake.Radius * shake.Radius;

    //        float distanceFactor = 1f - Mathf.Clamp01(distToExplosionSqr / explosionRadiusSqr);

    //        AudioListener.volume = 1f - _EarRingingAudioVolumeDecrease * distanceFactor;

    //        _EarRingingAudio.Play(ItemSelection.Method.RandomExcludeLast, _AudioSource, distanceFactor);
    //    }
    //}

    private void ModifyMana(float delta)
    {
        float mana = Player.Mana.Get() + delta;
        mana = Mathf.Clamp(mana, 0f, 100f);
        Player.Mana.Set(mana);
    }

    //private void OnAttack()
    //{
    //    ModifyMana(-20);
    //    _ManaRegeneration.Pause();
    //}

    //private void OnJump()
    //{
    //    ModifyStamina(-_JumpStaminaTake);
    //    _JumpAudio.Play(ItemSelection.Method.RandomExcludeLast, _AudioSource);

    //    _StaminaRegeneration.Pause();
    //}

    //private void OnCrouchStart()
    //{
    //    _CrouchAudio.Play(ItemSelection.Method.RandomExcludeLast, _AudioSource);
    //}

    //private void OnCrouchEnd()
    //{
    //    _StandUpAudio.Play(ItemSelection.Method.RandomExcludeLast, _AudioSource);
    //}
}
