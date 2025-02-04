using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;


public class BallTurrentFire : MonoBehaviour
{
    public GameObject BallJoint;
    public GameObject[] FirePoints;
    public GameObject Projectile;
    public int damage = 0;
    public float ProjectileSpeed = 100f;
    public float RotateSpeed = 0f;
    public float radius = 0f;
    public float depth = 0f;
    public float angle = 0f;
    public float Delay = .5f;
    public LayerMask mask;

    public bool isFiring = false;
    private GameObject Target;
    private Physics physics;
    int FirePointPosition = 0;
    float timer = 0f;

    private void FixedUpdate()
    {
        RaycastHit[] coneHits = physics.ConeCastAll(transform.position, radius, transform.forward, depth, angle, mask);

        if(coneHits.Length > 0 && coneHits[0].collider.gameObject.CompareTag("Missile") && !isFiring)
        {
            Debug.Log("Found a missile!!");
            Target = coneHits[0].collider.gameObject;
            isFiring = true;
        }

        if(coneHits.Length == 0)
        {
            //Debug.Log("No missiles to be found. Stop firing.");
            Target = null;
            isFiring = false;
        }

        if (Target != null)
        {
            Quaternion rotation = Quaternion.LookRotation(Target.transform.position - BallJoint.transform.position);
            BallJoint.transform.rotation = Quaternion.RotateTowards(BallJoint.transform.rotation, rotation, Time.deltaTime * RotateSpeed);

            timer += Time.deltaTime;

            Debug.Log("Wait for cool down.");
            if (timer >= Delay)
            {
                Debug.Log("FIRE!!");
                timer = 0f;
                GameObject projectile = Instantiate(Projectile, FirePoints[FirePointPosition].transform.position, Quaternion.identity);

                var shot = projectile.GetComponent<ProjectileScript>();
                shot.TargetType = "Missile";
                projectile.GetComponent<Rigidbody>().AddForce(FirePoints[FirePointPosition].transform.TransformDirection(Vector3.forward) * ProjectileSpeed);
                shot.SetDamage(damage);

                FirePointPosition++;
                if (FirePointPosition == FirePoints.Length)
                { FirePointPosition = 0; }
            }

        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * depth;
        Gizmos.DrawRay(transform.position, direction);
    }

}
