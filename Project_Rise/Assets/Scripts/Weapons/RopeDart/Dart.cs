using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Dart : MonoBehaviour
{

    public Rigidbody RB;

    public ParticleSystem particles;

    public Transform VisualObject;

    public AudioSource HitClip;

    public event UnityAction HitTarget;

    public bool ActiveThrow;

    private Vector3 _previousVelocity;

    private void Awake()
    {
        if (!RB)
        {
            RB = GetComponent<Rigidbody>();
        }
        _previousVelocity = Vector3.zero;

    }


    private void OnDisable()
    {
        particles.Stop();
        HitClip.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (ActiveThrow)
        {
            if (_previousVelocity != Vector3.zero && !RB.isKinematic)
            {
                transform.forward = _previousVelocity;
            }
        }
        _previousVelocity = RB.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ActiveThrow)
        {
            RB.isKinematic = true;
            Debug.Log(collision.contacts[0].otherCollider.name);

            transform.forward = Vector3.Lerp(transform.forward, -collision.contacts[0].normal, 0.2f);

            if (particles && particles.isStopped)
            {
                particles.transform.forward = collision.contacts[0].normal;
                particles.Play();
            }
            if (HitClip)
            {
                HitClip.Play();
            }

            HitTarget?.Invoke();
        }
    }
}
