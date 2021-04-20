using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(HealthEventData damageData);

    LivingEntity GetEntity();
}
