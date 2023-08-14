using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IWeapon : MonoBehaviour
{

    public WeaponSlot CurrentSlot;
    public float FireRate;
    protected float LastAttack;
    public virtual bool CanAttack { get { return Time.time - LastAttack > FireRate; } }

    protected abstract void PerformAction();

}

public enum WeaponSlot
{ 
    Primary,
    Secondary,
    Holstered
}
