using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpeed : MonoBehaviour
{

    public AudioSource Source;
    public Rigidbody RB;

    public float _baseSpeed = 10;
    public float _maxSpeed = 20;

    public AnimationCurve VolumeCurve;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentVelocity = RB.velocity;
        float targetZoom = VolumeCurve.Evaluate((currentVelocity.sqrMagnitude - (_baseSpeed * _baseSpeed)) / (_maxSpeed * _maxSpeed));
        Source.volume = Mathf.Lerp(Source.volume, targetZoom, 5 * Time.deltaTime);
    }
}
