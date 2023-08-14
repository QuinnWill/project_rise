using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Res"))
        {
            var respawn = other.GetComponent<RespawnObject>();
            if (respawn)
            {
                respawn.Respawn();
            }
        }
    }
}
