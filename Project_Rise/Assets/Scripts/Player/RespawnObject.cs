using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : MonoBehaviour
{
    public GameObject ObjectToRespawn;

    public Transform RespawnPoint;
    public bool ResetVelocity;


    public void Respawn()
    {
        if (RespawnPoint)
        {
            ObjectToRespawn.transform.position = RespawnPoint.position;
            ObjectToRespawn.transform.rotation = RespawnPoint.rotation;
            if (ResetVelocity)
            {
                var body = ObjectToRespawn.GetComponent<Rigidbody>();
                if (body)
                {
                    body.velocity = Vector3.zero;
                }
            }
        }
        else
        {
            ObjectToRespawn.transform.position = Vector3.zero;
            ObjectToRespawn.transform.rotation = Quaternion.identity;
            if (ResetVelocity)
            {
                var body = ObjectToRespawn.GetComponent<Rigidbody>();
                if (body)
                {
                    body.velocity = Vector3.zero;
                }
            }
        }
    }
}
