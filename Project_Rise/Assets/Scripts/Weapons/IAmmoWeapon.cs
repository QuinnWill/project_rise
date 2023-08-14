using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAmmoWeapon : IWeapon
{
    public bool UseAmmo;
    public int MaxAmmo;
    public int Ammo;
}
