using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTurret : MonoBehaviour
{
    //public KeyCode Fire = KeyCode.Mouse0;
    public EndPlayerTurn AddOne;
    public GameObject[] FirePoint;
    public GameObject Projectile;
    public WeaponSwap NextWeapon;
    public int damage = 0;
    public float speed = 100f;
    [HideInInspector]public bool hasFired = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && !hasFired)
        {
            foreach(GameObject point in FirePoint)
            {
                var shot = Instantiate(Projectile, point.transform.position, Quaternion.identity);
                shot.GetComponent<ProjectileScript>().TargetType = "Enemy";
                shot.GetComponent<Rigidbody>().AddForce(point.transform.TransformDirection(Vector3.forward) * speed);
                shot.GetComponent<ProjectileScript>().SetDamage(damage);
            }
            hasFired = true;
        }

        if(hasFired)
        {
            GetComponent<RotateTurrent>().enabled = false;
            GetComponent<BarrelAdjuster>().enabled = false;
            AddOne.Fired();
            NextWeapon.NextWeapon();
            enabled = false;
        }
    }

    public void UnFired()
    {
        hasFired = false;
    }
}
