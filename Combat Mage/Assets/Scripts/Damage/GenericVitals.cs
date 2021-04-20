using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DamageResistance
{
    [SerializeField]
    [Range(0f, 1f)]
    private float _GenericResistance = 0.1f;

    [SerializeField]
    [Range(0f, 1f)]
    private float _FireResistance = 0.1f;

    [SerializeField]
    [Range(0f, 1f)]
    private float _AirResistance = 0.1f;

    [SerializeField]
    [Range(0f, 1f)]
    private float _EarthResistance = 0.1f;

    [SerializeField]
    [Range(0f, 1f)]
    private float _WaterResistance = 0.1f;

    public float GetDamageResistance(HealthEventData damageData)
    {
        if (damageData.DamageType == DamageType.Generic)
            return _GenericResistance;
        else if (damageData.DamageType == DamageType.Fire)
            return _FireResistance;
        else if (damageData.DamageType == DamageType.Air)
            return _AirResistance;
        else if (damageData.DamageType == DamageType.Earth)
            return _EarthResistance;
        else if (damageData.DamageType == DamageType.Water)
            return _WaterResistance;

        return 0f;
    }

    public void SetFireDefenseSpellResistance()
    {
        _FireResistance = 1f;
        _AirResistance = 0.1f;
        _EarthResistance = 0.1f;
        _WaterResistance = 0.1f;
    }

    public void SetAirDefenseSpellResistance()
    {
        _FireResistance = 0.1f;
        _AirResistance = 1f;
        _EarthResistance = 0.1f;
        _WaterResistance = 0.1f;
    }

    public void SetEarthDefenseSpellResistance()
    {
        _FireResistance = 0.1f;
        _AirResistance = 0.1f;
        _EarthResistance = 1f;
        _WaterResistance = 0.1f;
    }

    public void SetWaterDefenseSpellResistance()
    {
        _FireResistance = 0.1f;
        _AirResistance = 0.1f;
        _EarthResistance = 0.1f;
        _WaterResistance = 1f;
    }

    public void SetNormalResistances()
    {
        _FireResistance = 0.1f;
        _AirResistance = 0.1f;
        _EarthResistance = 0.1f;
        _WaterResistance = 0.1f;
    }
}

[Serializable]
public class StatRegenData
{
    public bool CanRegenerate { get { return _Enabled && !IsPaused; } }

    public bool Enabled { get { return _Enabled; } }

    public bool IsPaused { get { return Time.time < _NextRegenTime; } }

    public float RegenDelta { get { return _Speed * Time.deltaTime; } }
    
    [SerializeField]
    private bool _Enabled = true;

    [SerializeField]
    private float _Pause = 2f;

    [SerializeField]
    private float _Speed = 10f;

    private float _NextRegenTime;

    public void Pause()
    {
        _NextRegenTime = Time.time + _Pause;
    }
}

public class GenericVitals : LivingEntityComponent
{
    [SerializeField]
    private DamageResistance _DamageResistance = null;

    [SerializeField]
    [Tooltip("The health to start with")]
    private float _MaxHealth = 100f;

    [SerializeField]
    private StatRegenData _HealthRegeneration = null;

    [SerializeField]
    protected AudioSource _AudioSource = null;

    protected float _HealthDelta;

    protected virtual void Start()
    {
        Entity.ChangeHealth.SetTryer(Try_ChangeHealth);

        SetOriginalMaxHealth();
    }

    protected virtual void Update()
    {
        if(_HealthRegeneration.CanRegenerate && Entity.Health.Get() < 100f && Entity.Health.Get() > 0f)
        {
            var data = new HealthEventData(_HealthRegeneration.RegenDelta);
            Entity.ChangeHealth.Try(data);
        }
    }

    protected virtual bool Try_ChangeHealth(HealthEventData healthEventData)
    {
        if (Entity.Health.Get() == 0f)
            return false;
        if (healthEventData.Delta > 0f && Entity.Health.Get() == 100f)
            return false;

        float healthDelta = healthEventData.Delta;

        if (healthDelta < 0f)
            healthDelta *= (1f - _DamageResistance.GetDamageResistance(healthEventData));

        float newHealth = Mathf.Clamp(Entity.Health.Get() + healthDelta, 0f, 100f);
        Entity.Health.Set(newHealth);

        if (healthDelta < 0f)
            _HealthRegeneration.Pause();

        return true;
    }

    private void SetOriginalMaxHealth()
    {
        Entity.Health.Set(_MaxHealth);
    }

    public void SetFireResistance()
    {
        _DamageResistance.SetFireDefenseSpellResistance();
    }

    public void SetAirResistance()
    {
        _DamageResistance.SetAirDefenseSpellResistance();
    }

    public void SetEarthResistance()
    {
        _DamageResistance.SetEarthDefenseSpellResistance();
    }

    public void SetWaterResistance()
    {
        _DamageResistance.SetWaterDefenseSpellResistance();
    }

    public void SetNormalResistance()
    {
        _DamageResistance.SetNormalResistances();
    }
}
