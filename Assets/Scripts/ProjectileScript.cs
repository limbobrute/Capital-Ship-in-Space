using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public string TargetType = "";
    private InitiativeTracker tracker;
    private void Start()
    {
        tracker = InitiativeTracker.instance;
        StartCoroutine(DistanceCheck());
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("We hit something");
        if(other.transform.gameObject.CompareTag(TargetType))
        {
            other.transform.gameObject.SetActive(false);
            var temp = other.GetComponent<InitRemover>();
            temp.cube.GetComponent<NeigbourCubes>().EmptyPoint();
            //Debug.Log("Got them!!");
            Destroy(gameObject);
            tracker.RemoveFromInit(temp.InitObject);
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
