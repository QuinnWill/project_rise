using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{
    public LayerMask EventMask;
    public UnityEvent EnterEvent;
    public UnityEvent ExitEvent;
    public UnityEvent StayEvent;


    private void OnTriggerEnter(Collider other)
    {
        if (EventMask == (EventMask | (1 << other.gameObject.layer)))
        {
            EnterEvent?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (EventMask == (EventMask | (1 << other.gameObject.layer)))
        {
            ExitEvent?.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (EventMask == (EventMask | (1 << other.gameObject.layer)))
        {
            StayEvent?.Invoke();
        }
    }
}
