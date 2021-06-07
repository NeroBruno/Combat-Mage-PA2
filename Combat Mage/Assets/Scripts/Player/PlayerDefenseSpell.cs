using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefenseSpell : PlayerComponent
{
    // Player spell stuff and logic

    public GameObject DebugSpellDefend;

    private MeshRenderer _MeshRenderer;
    private Material _Material;

    [SerializeField]
    private List<Texture2D> _Textures = new List<Texture2D>();

    private float _uSpeed = 0f;
    private float _vSpeed = 0f;

    private void Awake()
    {
        //_Material.color = Color.red;
        //_Material.mainTexture = _Textures[0];
        //_Shader = _Material.shader;
    }

    private void Start()
    {
        Player.SpellDefend.AddStartListener(StartDefend);
        Player.SpellDefend.AddStopListener(StopDefend);
        Player.CurrentDefenseElement.AddChangeListener(ChangeElement);

        _Material = Resources.Load<Material>("shieldtest");
        _MeshRenderer = GetComponent<MeshRenderer>();
        Material[] materials = _MeshRenderer.materials;
        // Get the current material applied on the GameObject
        Material oldMaterial = materials[0];
        // Set the new material on the GameObject
        Material[] newMaterials = new Material[] { oldMaterial };

        if (Player.CurrentDefenseElement.Get() == DamageType.Fire)
        {
            _MeshRenderer.material.mainTexture = _Textures[0];
            _MeshRenderer.material.SetColor("_MainColor", Color.red);
            //meshRenderer.material.SetColor("_EmissionColor", Color.red);

        }
        else if (Player.CurrentDefenseElement.Get() == DamageType.Air)
        {
            _MeshRenderer.material.mainTexture = _Textures[1];
            _MeshRenderer.material.SetColor("_MainColor", Color.grey);

        }
        else if (Player.CurrentDefenseElement.Get() == DamageType.Earth)
        {
            _MeshRenderer.material.mainTexture = _Textures[2];
            _MeshRenderer.material.SetColor("_MainColor", Color.green);

        }
        else if (Player.CurrentDefenseElement.Get() == DamageType.Water)
        {
            _MeshRenderer.material.mainTexture = _Textures[3];
            _MeshRenderer.material.SetColor("_MainColor", Color.blue);

        }
        _MeshRenderer.materials = newMaterials;
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
        {
            _MeshRenderer.material.color = Color.red;
            _MeshRenderer.material.mainTexture = _Textures[0];

        }
        else if (obj == DamageType.Air)
        {
            _MeshRenderer.material.color = Color.grey;
            _MeshRenderer.material.mainTexture = _Textures[1];

        }
        else if (obj == DamageType.Earth)
        {
            _MeshRenderer.material.color = Color.green;
            _MeshRenderer.material.mainTexture = _Textures[2];

        }
        else if (obj == DamageType.Water)
        {
            _MeshRenderer.material.color = Color.blue;
            _MeshRenderer.material.mainTexture = _Textures[3];

        }
    }
}
