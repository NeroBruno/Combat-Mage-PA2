using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerComponent
{
    public bool IsGrounded { get => _Controller.isGrounded; }

    public Vector3 Velocity { get => _Controller.velocity; }

    public Vector3 SurfaceNormal { get; private set; }

    public float SlopeLimit { get => _Controller.slopeLimit; }

    public float DefaultHeight { get; private set; }

    [SerializeField]
    private CharacterController _Controller = null;

    [SerializeField]
    private LayerMask _ObstacleCheckMask = ~0;

    [Header("Core Movement")]
    
    [SerializeField]
    [Range(0f, 20f)]
    private float _Acceleration = 5f;

    [SerializeField]
    [Range(0f, 20f)]
    private float _Damping = 8f;

    [SerializeField]
    [Range(0f, 1f)]
    private float _AirborneControl = 0.15f;

    [SerializeField]
    [Range(0f, 3f)]
    private float _StepLength = 1.2f;

    [SerializeField]
    [Range(0f, 10f)]
    private float _ForwardSpeed = 2.5f;

    [SerializeField]
    [Range(0f, 10f)]
    private float _BackSpeed = 2.5f;

    [SerializeField]
    [Range(0f, 10f)]
    private float _SideSpeed = 2.5f;
    
    [SerializeField]
    private AnimationCurve _SlopeSpeedMult = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));

    [SerializeField]
    private float _AntiBumpFactor = 1f;

    //Jumping

    [SerializeField]
    private bool _EnableJumping = true;

    //Dodging

    [SerializeField]
    private bool _EnableDodging = true;

    [SerializeField]
    private float _DodgeDistance = 3f;

    [SerializeField]
    private float _DodgeTimer = 0.3f;

    private float _LastDodgeTime;

    [SerializeField]
    [Range(0f, 3f)]
    private float _JumpHeight = 1f;

    [SerializeField]
    [Range(0f, 1.5f)]
    private float _JumpTimer = 0.3f;

    //Sliding
    [SerializeField]
    private bool _EnableSliding = false;

    [SerializeField]
    [Range(20f, 90f)]
    private float _SlideTreeshold = 32f;

    [SerializeField]
    [Range(0f, 50f)]
    private float _SlideSpeed = 15f;

    //MISC

    [SerializeField]
    [Range(0f, 100f)]
    private float _Gravity = 20f;

    private Vector3 _DesiredVelocityLocal;
    private Vector3 _SlideVelocity;

    private CollisionFlags _CollisionFlags;
    private bool _PreviouslyGrounded;
    private float _LastLandTime;

    private float _DistMovedSinceLastCycleEnded;
    private float _CurrentStepLength;

    private void Awake()
    {
        DefaultHeight = _Controller.height;

        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position + transform.up, -transform.up, out hitInfo, 3f, ~0, QueryTriggerInteraction.Ignore))
            transform.position = hitInfo.point + Vector3.up * 0.08f;
    }

    private void Start()
    {
        Player.IsGrounded.AddChangeListener(OnGroundingStateChanged);
        Player.Jump.AddStartTryer(TryJump);
        Player.Dash.AddStartTryer(TryDash);
        Player.Death.AddListener(OnDeath);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        Vector3 translation = Vector3.zero;

        if (IsGrounded)
        {
            translation = transform.TransformVector(_DesiredVelocityLocal) * deltaTime;

            if (!Player.Jump.Active)
                translation.y = -_AntiBumpFactor;
        }
        else
            translation = transform.TransformVector(_DesiredVelocityLocal * deltaTime);

        _CollisionFlags = _Controller.Move(translation);

        if ((_CollisionFlags & CollisionFlags.Below) == CollisionFlags.Below && !_PreviouslyGrounded)
        {
            bool wasJumping = Player.Jump.Active;

            if (Player.Jump.Active)
                Player.Jump.ForceStop();

            Player.FallImpact.Send(Mathf.Abs(_DesiredVelocityLocal.y));

            _LastLandTime = Time.time;

            if (wasJumping)
                _DesiredVelocityLocal = Vector3.ClampMagnitude(_DesiredVelocityLocal, 1f);
        }

        Vector3 targetVelocity = CalculateTargetVelocity(Player.MoveInput.Get());

        if (!IsGrounded)
            UpdateAirborneMovement(deltaTime, targetVelocity, ref _DesiredVelocityLocal);
        else if (!Player.Jump.Active)
            UpdateGroundedMovement(deltaTime, targetVelocity, ref _DesiredVelocityLocal);

        Player.IsGrounded.Set(IsGrounded);
        Player.Velocity.Set(Velocity);

        _PreviouslyGrounded = IsGrounded;
    }

    private void UpdateGroundedMovement(float deltaTime, Vector3 targetVelocity, ref Vector3 velocity)
    {
        // Make sure to lower speed when moving on steep surfaces
        float surfaceAngle = Vector3.Angle(Vector3.up, SurfaceNormal);
        targetVelocity *= _SlopeSpeedMult.Evaluate(surfaceAngle / SlopeLimit);

        // Calculate the rate at which the current speed should increase or decrease
        // If the player doesnt press any movement button, use the _Damping value otherwise use _Acceleration
        float targetAccel = targetVelocity.sqrMagnitude > 0f ? _Acceleration : _Damping;

        velocity = Vector3.Lerp(velocity, targetVelocity, targetAccel * deltaTime);

        // If we are moving and not running start the Walk activity
        if (!Player.Walk.Active && targetVelocity.sqrMagnitude > 0.05f /*&& !Player.Run.Active && !Player.Crouch.Active*/)
            Player.Walk.ForceStart();
        // If we are running or not moving stop the Walk activity
        else if (Player.Walk.Active && (targetVelocity.sqrMagnitude < 0.05f /*|| Player.Run.Active || Player.Crouch.Active*/))
            Player.Walk.ForceStop();

        // Leaving this here because some of it could be used as reference for mana or to when we are casting or binding spells (probably slow down speed a bit)
        //if (Player.Run.Active)
        //{
        //    bool wantsToMoveBackwards = Player.MoveInput.Get().y < 0f;
        //    bool runShouldStop = wantsToMoveBackwards || targetVelocity.sqrMagnitude == 0f || Player.Stamina.Is(0f);

        //    if (runShouldStop)
        //        Player.Run.ForceStop();
        //}

        if (_EnableSliding)
        {
            //Sliding
            if (surfaceAngle > _SlideTreeshold && Player.MoveInput.Get().sqrMagnitude == 0f)
            {
                Vector3 slideDirection = (SurfaceNormal + Vector3.down);
                _SlideVelocity += slideDirection * _SlideSpeed * deltaTime;
            }
            else
                _SlideVelocity = Vector3.Lerp(_SlideVelocity, Vector3.zero, deltaTime * 10f);

            velocity += transform.InverseTransformVector(_SlideVelocity);
        }

        // Advance step
        _DistMovedSinceLastCycleEnded += _DesiredVelocityLocal.magnitude * deltaTime;

        // Which step length should be used?
        float targetStepLength = _StepLength;

        //if (Player.Crouch.Active)
        //    targetStepLength = _CrouchStepLength;
        //else if (Player.Run.Active)
        //    targetStepLength = _RunStepLength;

        _CurrentStepLength = Mathf.MoveTowards(_CurrentStepLength, targetStepLength, deltaTime * 0.6f);

        // If the step cycle is complete, reset it, and send a notification
        if (_DistMovedSinceLastCycleEnded > _CurrentStepLength)
        {
            _DistMovedSinceLastCycleEnded -= _CurrentStepLength;
            Player.MoveCycleEnded.Send();
        }

        Player.MoveCycle.Set(_DistMovedSinceLastCycleEnded / _CurrentStepLength);
    }

    private void UpdateAirborneMovement(float deltaTime, Vector3 targetVelocity, ref Vector3 velocity)
    {
        if (_PreviouslyGrounded && !Player.Jump.Active)
            velocity.y = 0f;

        // Modify current velocity by taking into account how well we can change direction when not grounded _AirControl tooltip
        velocity += targetVelocity * _Acceleration * _AirborneControl * deltaTime;

        // Apply gravity
        velocity.y -= _Gravity * deltaTime;
    }

    //private bool TryRun()
    //{
    //    if (!_EnableRunning || Player.Stamina.Get() < 15f)
    //        return false;

    //    bool wantsToMoveBack = Player.MoveInput.Get().y < 0f;

    //    return Player.IsGrounded.Get() && !wantsToMoveBack && !Player.Crouch.Active && !Player.Aim.Active;
    //}

    private bool TryDash()
    {
        bool canDash = _EnableDodging && Time.time > _LastDodgeTime + _DodgeTimer && Player.Mana.Get() >= 20f;

        if (!canDash)
            return false;

        float dodgeSpeed = _DodgeDistance * 2f;
        Vector2 direction = Player.MoveInput.Get().normalized;

        _DesiredVelocityLocal = new Vector3(direction.x * dodgeSpeed, _DesiredVelocityLocal.y, direction.y * dodgeSpeed);

        return true;
    }

    private bool TryJump()
    {
        //If crouched stop crouching first
        //if (Player.Crouch.Active)
        //{
        //    Player.Crouch.TryStop();
        //    return false;
        //}

        bool canJump = _EnableJumping && IsGrounded && /*!Player.Crouch.Active &&*/ Time.time > _LastLandTime + _JumpTimer;

        if (!canJump)
            return false;
        float jumpSpeed = Mathf.Sqrt(2 * _Gravity * _JumpHeight);
        _DesiredVelocityLocal = new Vector3(_DesiredVelocityLocal.x, jumpSpeed, _DesiredVelocityLocal.z);

        return true;
    }

    //private bool TryCrouch()
    //{
    //    bool canCrouch = _EnableCrouching && (Time.time > _NextTimeCanCrouch || _NextTimeCanCrouch == 0f) && Player.IsGrounded.Get() && !Player.Run.Active;

    //    if (canCrouch)
    //    {
    //        SetHeight(_CrouchHeight);
    //        _NextTimeCanCrouch = Time.time + _CrouchDuration;
    //    }

    //    return canCrouch;
    //}

    //private bool TryUncrouch()
    //{
    //    bool obstacleAbove = DoCollisionCheck(true, Mathf.Abs(_CrouchedHeight - _UncrouchedHeight));
    //    bool canStopCrouching = Time.time > _NextTimeCanCrouch && !obstacleAbove;

    //    if (canStopCrouching)
    //    {
    //        SetHeight(DefaultHeight);
    //        _NextTimeCanCrouch = Time.time;
    //    }

    //    return canStopCrouching;
    //}

    private void OnGroundingStateChanged(bool isGrounded)
    {
        if (!isGrounded)
        {
            Player.Walk.ForceStop();
            //Player.Run.ForceStop();
        }
    }

    private Vector3 CalculateTargetVelocity(Vector2 moveInput)
    {
        moveInput = Vector2.ClampMagnitude(moveInput, 1f);

        bool wantsToMove = moveInput.sqrMagnitude > 0f;

        // Calculate the direction in which the player wants to move
        Vector3 targetDirection = (wantsToMove ? new Vector3(moveInput.x, 0f, moveInput.y) : _DesiredVelocityLocal.normalized);

        float desiredSpeed = 0f;

        if (wantsToMove)
        {
            // Set default speed
            desiredSpeed = _ForwardSpeed;

            // If player wants to move sideways
            if (Mathf.Abs(moveInput.x) > 0f)
                desiredSpeed = _SideSpeed;

            // If player wants to move backwards
            if (moveInput.y < 0f)
            {
                desiredSpeed = _BackSpeed;
            }

            // If we are currently running..
            //if (Player.Run.Active)
            //{
            //    // If the player wants to move forward or sideways, apply the run speed multiplier
            //    if (desiredSpeed == _ForwardSpeed || desiredSpeed == _SideSpeed)
            //        desiredSpeed = _RunSpeed;
            //}

            //// If we are crouching
            //if (Player.Crouch.Active)
            //    desiredSpeed *= _CrouchSpeedMult;
        }

        return targetDirection * (desiredSpeed * Player.MovementSpeedFactor.Val);
    }

    private bool DoCollisionCheck(bool checkAbove, float maxDistance, out RaycastHit hitInfo)
    {
        Vector3 rayOrigin = transform.position + (checkAbove ? Vector3.up * _Controller.height : Vector3.zero);
        Vector3 rayDirection = checkAbove ? Vector3.up : Vector3.down;

        return Physics.SphereCast(new Ray(rayOrigin, rayDirection), _Controller.radius, out hitInfo, maxDistance, _ObstacleCheckMask, QueryTriggerInteraction.Ignore);
    }

    private bool DoCollisionCheck(bool checkAbove, float maxDistance)
    {
        Vector3 rayOrigin = transform.position + (checkAbove ? Vector3.up * _Controller.height : Vector3.zero);
        Vector3 rayDirection = checkAbove ? Vector3.up : Vector3.down;

        return Physics.SphereCast(new Ray(rayOrigin, rayDirection), _Controller.radius, maxDistance, _ObstacleCheckMask, QueryTriggerInteraction.Ignore);
    }

    private void SetHeight(float height)
    {
        _Controller.height = height;
        _Controller.center = Vector3.up * height * 0.5f;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        SurfaceNormal = hit.normal;
    }

    private void OnDeath()
    {
        _DesiredVelocityLocal = Vector3.zero;
    }
}
