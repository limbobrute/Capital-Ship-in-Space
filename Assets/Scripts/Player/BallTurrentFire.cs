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
    int FirePointPosition = 0;
    public float ProjectileSpeed = 100f;
    public float RotateSpeed = 0f;
    public float radius = 0f;
    public float depth = 0f;
    public float angle = 0f;
    public float MaxDelay = .5f;
    float timer = 0f;
    public LayerMask mask;

    public bool isFiring = false;
    private GameObject Target;
    private Physics physics;

    private void FixedUpdate()
    {
        RaycastHit[] coneHits = physics.ConeCastAll(transform.position, radius, transform.forward, depth, angle, mask);

        if(coneHits.Length > 0 && coneHits[0].collider.gameObject.CompareTag("Missile") && !isFiring)
        {
            float delay = Random.Range(0, MaxDelay);
            Debug.Log("Found a missile!!");
            //Fire(coneHits[0].collider.gameObject);
            Target = coneHits[0].collider.gameObject;
            isFiring = true;
            StartCoroutine(Fire(delay));
        }

        if(coneHits.Length == 0)
        {
            Debug.Log("No missiles to be found. Stop firing.");
            StopCoroutine(Fire(0f));
            Target = null;
            isFiring = false;
        }

        coneHits = null;

    }

    IEnumerator Fire(float startdelay)
    {
        //float delay = 0f;
        while (isFiring)
        {
            timer += Time.deltaTime;
            Quaternion rotation = Quaternion.LookRotation(Target.transform.position - BallJoint.transform.position);
            BallJoint.transform.rotation = Quaternion.RotateTowards(BallJoint.transform.rotation, rotation, Time.deltaTime * RotateSpeed);

            Debug.Log("Wait for cool down.");
            if (timer >= startdelay)
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
                startdelay = Random.Range(0, MaxDelay);
            }

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * depth;
        Gizmos.DrawRay(transform.position, direction);
    }

}
