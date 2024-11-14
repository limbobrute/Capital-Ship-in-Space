using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public string TargetType = "";
    private int damage = 0;
    private InitiativeTracker tracker;

    private void Start()
    {
        tracker = InitiativeTracker.instance;
        StartCoroutine(DistanceCheck());
    }

    public void SetDamage(int d)
    {
        damage = d;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("We hit something");
        if(other.transform.gameObject.CompareTag(TargetType))
        {
            if(TargetType == "Player" || TargetType == "PlayerShipPart")
            {
                other.transform.gameObject.GetComponent<ComponentHealth>().DamageComponent(damage);
                Destroy(gameObject);
            }

            else if(TargetType == "Enemy")
            {
                //tracker.RemoveFromInit(other.transform.gameObject);
                //Destroy(other.transform.gameObject);
                other.GetComponent<EnemyAI>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else
        { Destroy(gameObject); }
    }

    IEnumerator DistanceCheck()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
