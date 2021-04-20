using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVitalsGUI : HUD_DisplayerBehaviour
{
    [SerializeField]
    private Image _HealthBar = null;

    [SerializeField]
    private Image _ManaBar = null;

    public override void OnPostAttachment()
    {
        Player.Health.AddChangeListener(OnChanged_Health);
        Player.Mana.AddChangeListener(OnChanged_Mana);
    }

    private void OnChanged_Health(float health)
    {
        _HealthBar.fillAmount = health / 100f;
    }

    private void OnChanged_Mana(float mana)
    {
        _ManaBar.fillAmount = mana / 100f;
    }
}
