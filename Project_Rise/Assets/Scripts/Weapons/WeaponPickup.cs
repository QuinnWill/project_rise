using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponPickup : MonoBehaviour
{

    public GameObject WeaponPrefab;

    public UnityEvent pickupEvent;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " Entered");
        if (other.CompareTag("Player"))
        {
            var CurrWeapon = Camera.main.transform.GetComponentInChildren<RopeDart>();
            if (!CurrWeapon)
            {
                CurrWeapon = Instantiate(WeaponPrefab, Camera.main.transform).GetComponent<RopeDart>();
                CurrWeapon.HeldBody = other.attachedRigidbody;
            }
            pickupEvent?.Invoke();
            Destroy(gameObject);
        }
    }
}
