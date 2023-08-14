using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KickWeapon : IWeapon
{

    public InputHandlerSO InputSO;
    public PlayerStateSO PlayerState;
    public Rigidbody HeldBody;

    public LayerMask KickableMask;

    public float KickForce;
    [Range(0,1)]
    public float NormalKickRatio;

    private float _LastKick;


    private void OnEnable()
    {
        InputSO.PrimaryEvent += OnPrimary;
        InputSO.SecondaryEvent += OnSecondary;
    }

    private void OnDisable()
    {
        InputSO.PrimaryEvent -= OnPrimary;
        InputSO.SecondaryEvent -= OnSecondary;
    }

    // Start is called before the first frame update
    void Awake()
    {
        _LastKick = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void PerformAction()
    {
       
        var forceDir = -Camera.main.transform.forward;
        RaycastHit hitInfo;
        bool hit = Physics.SphereCast(transform.position, 0.25f, Camera.main.transform.forward, out hitInfo, 2, KickableMask, QueryTriggerInteraction.Ignore);
        if (hit)
        {
            PlayerState.Grounded = false;
            PlayerState.WallRunning = false;
            _LastKick = Time.time;
            var kickDot = Vector3.Dot(HeldBody.velocity, forceDir);
            forceDir += NormalKickRatio * hitInfo.normal;
            if (kickDot < 0)
            {
                HeldBody.velocity = HeldBody.velocity - kickDot * forceDir;
            }
            HeldBody.AddForce(forceDir * KickForce, ForceMode.Impulse);
        }
    }

    private void OnPrimary(InputAction.CallbackContext context)
    {
        if (CurrentSlot != WeaponSlot.Primary)
        {
            return;
        }

        if (context.started && Time.time - _LastKick > FireRate)
        {
            PerformAction();
        }
    }

    private void OnSecondary(InputAction.CallbackContext context)
    {
        if (CurrentSlot != WeaponSlot.Secondary)
        {
            return;
        }

        if (context.started && Time.time - _LastKick > FireRate)
        {
            PerformAction();
        }
    }
}
