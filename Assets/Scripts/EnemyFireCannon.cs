using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireCannon : MonoBehaviour
{
    public float distance = 30f;
    public LayerMask mask;
    public GameObject Projectile;
    public GameObject point;
    public float speed = 100f;
    public GameObject us;
    public GameObject EnemyCam;
    private InitiativeTracker tracker;

    private void Start()
    {
        tracker = InitiativeTracker.instance;
    }

    public void CheckFire()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, distance, mask);
        if (hit.transform != null && hit.transform.gameObject.CompareTag("Player"))
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
        Gizmos.color = Color.green;
        Vector3 direction = transform.TransformDirection(Vector3.up) * distance;
        Gizmos.DrawRay(transform.position, direction);
    }
}
