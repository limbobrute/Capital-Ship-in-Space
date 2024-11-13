using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireCannon : MonoBehaviour
{
    public SmallPatrol thisShip;
    public LayerMask mask;
    public GameObject Projectile;
    public GameObject point;
    public float speed = 100f;
    public GameObject us;
    public GameObject EnemyCam;
    public Transform GunPoint;
    private InitiativeTracker tracker;

    private void Start()
    {
        tracker = InitiativeTracker.instance;
    }

    public void CheckFire()
    {
        float distance = us.GetComponent<SmallPatrol>().ShootingDistance;
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, distance, mask);
        if (hit.transform != null && (hit.transform.gameObject.CompareTag("Player") || hit.transform.gameObject.CompareTag("PlayerShipPart")))
        {
            FireCannon();
        }
        EnemyCam.SetActive(false);
        tracker.NextTurn(us);
    }

    private void FireCannon()
    {
        var shot = Instantiate(Projectile, point.transform.position, Quaternion.identity);
        shot.GetComponent<ProjectileScript>().TargetType = "Player";
        shot.GetComponent<ProjectileScript>().SetDamage(thisShip.CannonDamage);
        shot.GetComponent<Rigidbody>().AddForce(point.transform.TransformDirection(Vector3.forward) * speed);
    }

    public void TestFire()
    {
        var shot = Instantiate(Projectile, point.transform.position, Quaternion.identity);
        shot.GetComponent<ProjectileScript>().TargetType = "Player";
        shot.GetComponent<Rigidbody>().AddForce(point.transform.TransformDirection(Vector3.forward) * speed);
    }

    private void OnDrawGizmos()
    {
        float distance = thisShip.ShootingDistance;
        Gizmos.color = Color.green;
        Vector3 direction = transform.TransformDirection(Vector3.up * distance);
        Gizmos.DrawRay(transform.position, direction);
        RangeCheck.DrawGizmoDisk(GunPoint, distance);
    }
}
