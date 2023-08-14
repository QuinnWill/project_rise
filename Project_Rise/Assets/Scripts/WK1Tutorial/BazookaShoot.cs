using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaShoot : MonoBehaviour
{
    public Transform MuzzlePoint;
    public GameObject Projectile;
    public float ProjectileLifetime = 5.0f;
    public float RateOfFire = 0.33f;
    public float ShootForce;

    float _timeToFire;
    // Start is called before the first frame update
    void Start()
    {
        _timeToFire = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _timeToFire += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && _timeToFire > RateOfFire)
        {
            Fire();
        }
    }

    private void Fire()
    {
        GameObject currentProjectile = Instantiate(Projectile, MuzzlePoint.position, MuzzlePoint.rotation);
        Rigidbody projectileBody = currentProjectile.GetComponent<Rigidbody>();
        projectileBody.AddForce(MuzzlePoint.up * ShootForce, ForceMode.Impulse);
        Destroy(currentProjectile, ProjectileLifetime);
        _timeToFire = 0;
    }
}
