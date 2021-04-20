using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : PlayerComponent
{
    public float SensitivityFactor { get; set; }

    public Vector2 LookAngles
    {
        get => _LookAngles;

        set
        {
            _LookAngles = value;
        }
    }

    public Vector2 LastMovement { get; private set; }

    //[Header("General", true)]
    [Header("General")]

    [SerializeField]
    [Tooltip("The camera root which will be rotated up and down on the x axis")]
    private Transform _LookRoot = null;

    [SerializeField]
    private Transform _PlayerRoot = null;

    [SerializeField]
    [Tooltip("The up and down rotation will be inverted if true")]
    private bool _Invert = false;

    // Motion
    [Header("Motion")]

    [SerializeField]
    [Tooltip("The higher it is the faster the camera will rotate")]
    private float _Sensitivity = 5f;

    //[SerializeField]
    //private float _AimSensitivy = 2.5f;

    [SerializeField]
    private float _RollAngle = 10f;

    [SerializeField]
    private float _RollSpeed = 3f;

    // Rotation Limits
    [Header("Rotation Limits")]

    [SerializeField]
    private Vector2 _DefaultLookLimits = new Vector2(-60f, 90f);

    private float _CurrentRollAngle;
    private Vector2 _LookAngles;

    private bool _Loaded;

    public void MoveCamera(float verticalMove, float horizontalMove)
    {
        LookAngles += new Vector2(verticalMove, horizontalMove);
    }

    public void OnLoad()
    {
        _Loaded = true;
    }

    private void Awake()
    {
        SensitivityFactor = 1f;
    }

    private void Start()
    {
        if (!_LookRoot)
        {
            Debug.LogErrorFormat(this, "Assign the look root in inspector", name);
            enabled = false;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (!_Loaded)
            _LookAngles = new Vector2(transform.localEulerAngles.x, _PlayerRoot.localEulerAngles.y);
    }

    private void LateUpdate()
    {
        Vector2 prevLookAngles = _LookAngles;

        if (Player.ViewLocked.Is(false) && Player.Health.Get() > 0f)
        {
            LookAround();
        }

        LastMovement = _LookAngles - prevLookAngles;
    }

    private void LookAround()
    {
        var sensitivity = _Sensitivity;
        sensitivity *= SensitivityFactor;

        _LookAngles.x += Player.LookInput.Get().y * sensitivity * (_Invert ? 1f : -1f);
        _LookAngles.y += Player.LookInput.Get().x * sensitivity;

        _LookAngles.x = ClampAngle(_LookAngles.x, _DefaultLookLimits.x, _DefaultLookLimits.y);

        _CurrentRollAngle = Mathf.Lerp(_CurrentRollAngle, Player.LookInput.Get().x * _RollAngle, Time.deltaTime * _RollSpeed);

        // Apply current vertical rotation to the look root
        _LookRoot.localRotation = Quaternion.Euler(_LookAngles.x, 0f, 0f);
        _PlayerRoot.localRotation = Quaternion.Euler(0f, _LookAngles.y, 0f);

        Entity.LookDirection.Set(_LookRoot.forward);
    }

    /// <summary>
    /// Clamps the given angle between min and max degrees
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle > 360f)
            angle -= 360f;
        else if (angle < -360f)
            angle += 360f;

        return Mathf.Clamp(angle, min, max);
    }
}
