using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class ParticleWindSpeed : MonoBehaviour
{

    private VisualEffect _VFX;
    public Rigidbody RB;

    public float _baseSpeed = 10;
    public float _maxSpeed = 20;

    public AnimationCurve WindCurve;

    private void Awake()
    {
        _VFX = GetComponent<VisualEffect>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentVelocity = RB.velocity;
        var velDot = Mathf.Abs(Vector3.Dot(Camera.main.transform.forward, currentVelocity));
        float targetZoom = WindCurve.Evaluate((velDot - _baseSpeed) / _maxSpeed);
        _VFX.SetFloat("SpawnRate", Mathf.Lerp(_VFX.GetFloat("SpawnRate"), targetZoom, 5 * Time.deltaTime));
    }
}
