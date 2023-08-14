using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class RopeDart : IWeapon
{

    [SerializeField] protected GameObject DartPrefab;

    public InputHandlerSO InputSO;
    public PlayerStateSO PlayerState;



    public Rigidbody HeldBody;

    public Dart CurrDart;

    public float DartHitDistance;
    public float CurrentDistance;

    public bool DartIsFired;

    public bool DartInTarget;

    public AudioSource Audio;

    public UnityEvent<GameObject> DartSpawned;
    public UnityEvent DartThrow;
    public UnityEvent DartRetrieve;
    public UnityEvent DartDespawn;

    [Header("VisualSettings")]
    public Transform ThrowPoint;
    public float VisualLagTime;
    public AnimationCurve VisualLagCurve;

    [Header("Pull Settings")]
    public float PullForce;
    public bool ReplaceVelocity;
    public AnimationCurve UpwardsPullCurve;

    [Header("Hang Settings")]
    public float RopeTensionForce = 50;
    public float RopeTensionDamper = 10;

    [Header("Dart Throw Settings")]
    public float ThrowForce;
    [Min(0)]
    public float ReturnTime;
    public float ReturnRotationStrength;
    public float MaxThrowDistance;


    public override bool CanAttack { get { return Time.time - LastAttack > FireRate && !DartIsFired; } }

    private void Awake()
    {
        if (!CurrDart)
        {
            CurrDart = Instantiate(DartPrefab).GetComponent<Dart>();
        }
        CurrDart.transform.SetParent(null);
        CurrDart.gameObject.SetActive(false);
        
        DartSpawned?.Invoke(CurrDart.gameObject);
    }

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
    void Start()
    {
        CurrDart.HitTarget += DartHitTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentSlot == WeaponSlot.Holstered)
        {
            return;
        }
        
        if (CurrDart.ActiveThrow)
        {
            MoveVisualsToObject();

            CurrentDistance = Vector3.Distance(transform.position, CurrDart.transform.position);
            if (!DartInTarget && CurrentDistance > MaxThrowDistance)
            {
                RetractDart();
            }
            else if (DartInTarget && CurrentDistance > MaxThrowDistance * 2f)
            {
                RetractDart();
            }
            else if (PlayerState.Crouching)
            {
                RetractDart();
            }
        }
        
    }

    private void FixedUpdate()
    {

        if (CurrentSlot == WeaponSlot.Holstered)
        {
            return;
        }

        if (DartInTarget && CurrentDistance > DartHitDistance)
        {
            ApplyTension();
            PlayerState.Swinging = true;
            Audio.volume = 0.5f;
        }
        else
        {
            PlayerState.Swinging = false;
            Audio.volume = 0;
        }
    }

    protected void MoveVisualsToObject()
    {
        if (CurrDart.VisualObject.position == Vector3.zero)
        {
            return;
        }

        CurrDart.VisualObject.position = Vector3.LerpUnclamped(CurrDart.VisualObject.position, CurrDart.transform.position, VisualLagCurve.Evaluate((Time.time - LastAttack) / VisualLagTime));
    }

    protected override void PerformAction()
    {
        StopAllCoroutines();
        LastAttack = Time.time;


        CurrDart.gameObject.SetActive(true);
        CurrDart.transform.SetPositionAndRotation(transform.position, transform.rotation);
        CurrDart.RB.isKinematic = false;
        CurrDart.RB.velocity = Vector3.zero;
        CurrDart.RB.angularVelocity = Vector3.zero;

        
        Debug.Log("DartPos: " + CurrDart.transform.position + " \nDesiredPos: " + transform.position);
        

        
        CurrDart.ActiveThrow = true;
        
        CurrDart.RB.AddForce(transform.forward * ThrowForce, ForceMode.Impulse);

        DartThrow?.Invoke();

        CurrDart.VisualObject.position = ThrowPoint.position;

        DartIsFired = true;
    }

    protected void Pull()
    {
        PlayerState.Grounded = false;
        Vector3 pullDir = (CurrDart.transform.position - transform.position).normalized;
        pullDir = Vector3.Lerp(pullDir, Vector3.up, UpwardsPullCurve.Evaluate(Vector3.Dot(pullDir, Vector3.up)));
        var velDot = Vector3.Dot(pullDir, HeldBody.velocity);
        if (velDot <= PullForce * 2)
        {
            if (velDot < 0 && ReplaceVelocity)
            {
                HeldBody.velocity = Vector3.zero;
            }
            HeldBody.AddForce(pullDir * (PullForce + PullForce * (CurrentDistance / MaxThrowDistance) / 2), ForceMode.Impulse);
        }
    }

    protected void ApplyTension()
    {
        var tensionDir = (transform.position - CurrDart.transform.position).normalized;

        var tensionDot = Vector3.Dot(HeldBody.velocity, tensionDir);

        float x = DartHitDistance - CurrentDistance;

        float springForce = (x * RopeTensionForce) - (tensionDot * RopeTensionDamper);

        //HeldBody.velocity = (HeldBody.velocity - tensionDot * tensionDir);
        //HeldBody.position += tensionDir * (DartHitDistance - CurrentDistance);
        HeldBody.AddForce(tensionDir * springForce);
    }

    protected void RetractDart()
    {
        DartInTarget = false;
        CurrDart.RB.isKinematic = false;
        CurrDart.RB.velocity = Vector3.zero;

        CurrDart.ActiveThrow = false;

        DartRetrieve?.Invoke();

        if (ReturnTime == 0)
        {
            StartCoroutine(DeactiveateDart(0));
            return;
        }

        var returnVelX = (transform.position.x - CurrDart.transform.position.x) / ReturnTime;
        var returnVelZ = (transform.position.z - CurrDart.transform.position.z) / ReturnTime;
        var returnVelY = (transform.position.y + 2 - CurrDart.transform.position.y) / ReturnTime + (1 / 2 * Physics.gravity.y / Time.deltaTime * ReturnTime * ReturnTime);

        CurrDart.RB.velocity = new Vector3(returnVelX, returnVelY, returnVelZ);
        CurrDart.RB.angularVelocity = Random.insideUnitSphere * ReturnRotationStrength;

        StartCoroutine(DeactiveateDart(ReturnTime));
    }

    protected IEnumerator DeactiveateDart(float time)
    {
        yield return new WaitForSeconds(time);
        DartIsFired = false;
        CurrDart.gameObject.SetActive(false);
        DartDespawn?.Invoke();
    }

    public void DartHitTarget()
    {
        DartInTarget = true;
        DartHitDistance = CurrentDistance * 1.1f;
    }

    private void OnPrimary(InputAction.CallbackContext context)
    {
        if(CurrentSlot != WeaponSlot.Primary)
        {
            return;
        }
        //Debug.Log("PrimaryStarted: " + context.started);
        if (context.started && !DartIsFired)
        {
            PerformAction();
        }
        else if (context.canceled && CurrDart.ActiveThrow)
        {
            if (DartInTarget)
            {
                Pull();
            }
            RetractDart();
        }
    }

    private void OnSecondary(InputAction.CallbackContext context)
    {
        if (CurrentSlot != WeaponSlot.Secondary)
        {
            return;
        }
        //Debug.Log("SecondaryStarted: " + context.started);\
        if (context.started && !DartIsFired)
        {
            PerformAction();
        }
        else if (context.canceled && CurrDart.ActiveThrow)
        {
            if (DartInTarget)
            {
                Pull();
            }
            RetractDart();
        }
    }

}
