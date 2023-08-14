using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController3D : MonoBehaviour
{

    [Header("Refereces")]
    [SerializeField] private Rigidbody _RB;

    [SerializeField] private InputHandlerSO _InputSO;

    [SerializeField] private PlayerStateSO _PlayerState;

    private RaycastHit _GroundRayHit;

    private bool doJump;

    [Header("Physics")]
    public LayerMask GroundMask;
    public Vector3 GravityDir = Vector3.down;
    
    public float GravityForce;
    [Min(0)]
    public float GravityScale = 1;
    public float ColliderHeight;
    public float RideHeight;
    [SerializeField] private float _RideSpringStrength;
    [SerializeField] private float _RideSpringDamper;

    [Header("Locomotion")]
    public float MaxSpeed;
    [Range(0,1)]
    public float CrouchSpeedRatio;
    public float CrouchSpeed { get { return MaxSpeed * CrouchSpeedRatio; } }
    public float Acceleration;
    public float MaxAccelerationForce;

    [SerializeField] private List<AnimationCurve> AccelerationCurves = new List<AnimationCurve>();
    [SerializeField] private List<bool> UseAsMinAcceleration = new List<bool>();

    [Header("Wall Run Settings")]
    public float WallCheckDist;
    [Range(0, 1)]
    public float WallNormalRange;
    [Range(0, 1)]
    public float WallJumpRatio;
    public float MinVerticalSpeed;

    [Header("Jumping")]
    [SerializeField] private float _maxJumpHeight = 2f;
    private float jumpForce;
    [Range(0, 1)]
    [SerializeField] private float NormalJumpPercent;
   
    [Header("Public Values")]
    public Vector3 Normal;
    public Vector3 WallNormal;
    public Vector2 InputDir;
    public bool Crouching;
    public bool Sliding;

    private void OnEnable()
    {
        _InputSO.MoveEvent += OnMoveEvent;
        _InputSO.JumpEvent += OnJumpEvent;
        _InputSO.CrouchEvent += OnCrouchEvent;
    }

    private void OnDisable()
    {
        _InputSO.MoveEvent -= OnMoveEvent;
        _InputSO.JumpEvent -= OnJumpEvent;
        _InputSO.CrouchEvent -= OnCrouchEvent;
    }


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (!_RB)
        {
            _RB = GetComponent<Rigidbody>();
        }

        jumpForce = Mathf.Sqrt(-2 * _maxJumpHeight * -GravityForce);

    }

    // Update is called once per frame
    void Update()
    {
        CheckWallRun();
        LookForGround();
        UpdatePlayerState();
        CheckGravityScale();
    }

    private void FixedUpdate()
    {
        Jump();
        
        Gravity();
        GroundForce();
        Move();

    }

    #region PhysicsHelpers
    private void LookForGround()
    {
        
        float checkDist = _PlayerState.Grounded ? (ColliderHeight / 2) + RideHeight : RideHeight;

        bool hit = Physics.Raycast(transform.position, GravityDir, out _GroundRayHit, checkDist, GroundMask, QueryTriggerInteraction.Ignore);
        Debug.DrawLine(transform.position, transform.position + GravityDir * checkDist, Color.red);

        if (hit)
        {
            if (_PlayerState.Grounded || Vector3.Dot(_RB.velocity, GravityDir) >= 0)
            {
                Normal = _GroundRayHit.normal;
                WallNormal = Vector3.up;
                _PlayerState.Grounded = true;
            }
            else
            {
                Normal = -GravityDir;
            }
        }
        else
        {
            _PlayerState.Grounded = false;
            Normal = -GravityDir;
        }
    }

    private void CheckWallRun()
    {
        if (_PlayerState.WallRunning)
        {
            if (_PlayerState.Grounded)
            {
                _PlayerState.WallRunning = false;
                GravityScale = 1f;
                Debug.Log("grounded so stopped wall running");
                return;
            }
            if (_RB.velocity.sqrMagnitude < 5 * 5)
            {
                _PlayerState.WallRunning = false;
                GravityScale = 1f;
                Debug.Log("not running fast enough");
                return;
            }
            else if (_RB.velocity.y < MinVerticalSpeed)
            {
                _PlayerState.WallRunning = false;
                GravityScale = 1f;
                Debug.Log("Falling too fast");
                return;
            }
            else
            {
                RaycastHit hitInfo;
                if (!Physics.Raycast(transform.position, -WallNormal, out hitInfo, WallCheckDist + 0.7f))
                {
                    _PlayerState.WallRunning = false;
                    GravityScale = 1f;
                    Debug.Log("no longer finding wall");
                    return;
                }
                else
                {
                    WallNormal = hitInfo.normal;
                    _RB.velocity = Vector3.ProjectOnPlane(_RB.velocity, WallNormal);
                    _RB.AddForce(-WallNormal * GravityForce * 2);
                    return;
                }
            }
        }


        if (!_PlayerState.Grounded && ( _RB.velocity.x != 0 || _RB.velocity.z != 0))
        {
            LookForWall();
        }
    }

    private void LookForWall()
    {
        if (_RB.velocity.y < MinVerticalSpeed)
        {
            return;
        }

        RaycastHit hitInfo;
        if (Physics.CapsuleCast(transform.position + (Vector3.up * 0.8f), transform.position - (Vector3.up * 0.8f), 0.5f, _RB.velocity, out hitInfo, _RB.velocity.magnitude * Time.deltaTime + 0.05f, GroundMask, QueryTriggerInteraction.Ignore))
        {
            SetWallRun(hitInfo);
            return;
        }

        if (Physics.Raycast(transform.position, Vector3.Cross(_RB.velocity.normalized, Vector3.down), out hitInfo, 0.85f, GroundMask, QueryTriggerInteraction.Ignore))
        {
            SetWallRun(hitInfo);
            return;
        }

        if (Physics.Raycast(transform.position, Vector3.Cross(_RB.velocity.normalized, Vector3.up), out hitInfo, 0.85f, GroundMask, QueryTriggerInteraction.Ignore))
        {
            SetWallRun(hitInfo);
            return;
        }
    }

    private void SetWallRun(RaycastHit hitInfo)
    {

        if (Vector3.Dot(WallNormal, hitInfo.normal) > 0.9f)
        {
            return;
        }

        if (Mathf.Abs(hitInfo.normal.y) <= 0.5f)
        {
            _PlayerState.WallRunning = true;
            WallNormal = hitInfo.normal;

            //_RB.MovePosition(_RB.position + (hitInfo.distance) * raycastDir);

            _RB.velocity = new Vector3(_RB.velocity.x, Mathf.Max(_RB.velocity.y * 0.75f, -0.5f), _RB.velocity.z);
            _RB.velocity = Vector3.ProjectOnPlane(_RB.velocity, WallNormal).normalized * _RB.velocity.magnitude;
            //_RB.AddForce(-WallNormal * GravityForce, ForceMode.Impulse);
        }
    }

    private void CheckGravityScale()
    {
        if (_PlayerState.Grounded)
        {
            GravityScale = 1;
        }
        else if (_PlayerState.Swinging)
        {
            GravityScale = 1;
        }
        else if (_PlayerState.WallRunning)
        {
            if (_RB.velocity.y < 0)
            {
                GravityScale = 0.3f;
            }
            else
            {
                GravityScale = 1.5f;
            }
        }
        else if (_RB.velocity.y < 0)
        {
            GravityScale = 1.5f;
        }
    }

    private void Gravity()
    {
        _RB.AddForce(GravityDir * GravityForce * GravityScale);
    }

    private void GroundForce()
    {
        if (_PlayerState.Swinging)
        {
            return;
        }
        if (_RB.velocity.sqrMagnitude > (MaxSpeed * MaxSpeed) * 1.1f)
        {
            return;
        }
        if (!_PlayerState.Grounded && _RB.velocity.y > 0)
        {
            return;
        }
        if (_PlayerState.Grounded)
        {
            Vector3 vel = _RB.velocity;

            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = _GroundRayHit.rigidbody;
            if (hitBody)
            {
                otherVel = hitBody.velocity;
            }

            float rayDirVel = Vector3.Dot(-Normal, vel);
            float otherDirVel = Vector3.Dot(-Normal, otherVel);

            float relVel = rayDirVel - otherDirVel;

            float x = _GroundRayHit.distance - RideHeight;

            float springForce = (x * _RideSpringStrength) - (relVel * _RideSpringDamper);

            //Debug.Log("SpringDistance: " + x);

            //Debug.DrawLine(transform.position, transform.position + (GravityDir * springForce), Color.yellow);

            _RB.AddForce(GravityDir * springForce);

            if (hitBody)
            {
                hitBody.AddForceAtPosition(GravityDir * -springForce, _GroundRayHit.point);
            }

            if (_PlayerState.Sliding)
            {
                if (Normal.y < 0.98f)
                {
                    var forceDir = Vector3.ProjectOnPlane(Vector3.down, Normal);
                    _RB.AddForce(forceDir * 20);
                }

            }
        }
        
    }
    #endregion

    private void Move()
    {
        Vector3 HorizontalVel = Vector3.ProjectOnPlane(_RB.velocity, Normal);

        Vector3 DesiredDir = new Vector3(InputDir.x, 0, InputDir.y);

        Vector3 CamForward = Camera.main.transform.forward;
        CamForward.y = 0;

        DesiredDir = Quaternion.FromToRotation(Vector3.forward, CamForward) * DesiredDir;

        DesiredDir = Quaternion.FromToRotation(Vector3.up, Normal) * DesiredDir;

        float velDot = Vector3.Dot(HorizontalVel.normalized, DesiredDir.normalized);
        float accel = Acceleration * AccelerationCurves[(int)_PlayerState.CurrentState].Evaluate(velDot);
        float speed = Crouching ? CrouchSpeed : MaxSpeed;

        Debug.DrawRay(transform.position, DesiredDir * 2, Color.blue);

        Vector3 GoalVel = Vector3.MoveTowards(HorizontalVel, DesiredDir * speed, accel * Time.fixedDeltaTime);
        if (_PlayerState.CurrentState == PlayerMoveState.Airborne && _RB.velocity.sqrMagnitude > speed * speed)
        {
            GoalVel = HorizontalVel * velDot + GoalVel * (1 - velDot);
        }
        else if (UseAsMinAcceleration[(int)_PlayerState.CurrentState] || _PlayerState.Sliding)
        {
            if (HorizontalVel.sqrMagnitude >= speed * speed)
            {
                GoalVel = HorizontalVel * velDot + GoalVel * (1 - velDot);
            }
            else
            {
                GoalVel = HorizontalVel + DesiredDir * accel * Time.fixedDeltaTime;
            }
        }

        Vector3 neededAccel = (GoalVel - HorizontalVel) / Time.fixedDeltaTime;

        neededAccel = Vector3.ClampMagnitude(neededAccel, MaxAccelerationForce);

        _RB.AddForce(neededAccel * _RB.mass);
    }

    private void Jump()
    {
        if (doJump)
        {
            if (_PlayerState.Grounded)
            {
                doJump = false;
                _PlayerState.Grounded = false;
                _RB.velocity = _RB.velocity - Vector3.Dot(_RB.velocity, GravityDir) * GravityDir;
                Vector3 jumpDir = Normal * NormalJumpPercent + Vector3.up * (1 - NormalJumpPercent);
                _RB.AddForce(jumpDir * jumpForce, ForceMode.Impulse);
            }
            else if (_PlayerState.WallRunning)
            {
                doJump = false;
                _PlayerState.WallRunning = false;
                GravityScale = 1f;
                var jumpDir = Vector3.Lerp(-GravityDir, WallNormal, WallJumpRatio).normalized;
                _RB.velocity = _RB.velocity - Vector3.Dot(_RB.velocity, jumpDir) * jumpDir;
                _RB.AddForce(jumpDir * jumpForce * 1.5f, ForceMode.Impulse);
                _RB.AddForce(Camera.main.transform.forward * jumpForce * 0.25f, ForceMode.Impulse);
            }
        }
    }

    private IEnumerator CancelJumpInput(float time)
    {
        yield return new WaitForSeconds(time);
        doJump = false;
    }

    private void UpdatePlayerState()
    {
        _PlayerState.Crouching = Crouching;
        _PlayerState.Sliding = Crouching && _RB.velocity.magnitude > CrouchSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*foreach (ContactPoint contact in collision.contacts)
        {
            if (Mathf.Abs(contact.normal.y) <= 0.3f && _RB.velocity.y > MinVerticalSpeed)
            {

                _RB.velocity = new Vector3(_RB.velocity.x, Mathf.Max(_RB.velocity.y * 0.75f, -0.5f), _RB.velocity.z);
                _RB.velocity = Vector3.ProjectOnPlane(_RB.velocity, WallNormal);
                _PlayerState.WallRunning = true;
                WallNormal = contact.normal;


            }
        }*/
    }

    private void OnMoveEvent(InputAction.CallbackContext context)
    {
        InputDir = context.ReadValue<Vector2>();
    }

    private void OnJumpEvent(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            doJump = true;
            StartCoroutine(CancelJumpInput(0.3f));
        }
        else if (context.canceled && _RB.velocity.y > 0)
        {
            GravityScale = 1.5f;
        }
    }

    private void OnCrouchEvent(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Crouching = true;
        }
        else if (context.canceled)
        {
            Crouching = false;
        }
    }
}
