using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obscatle : MonoBehaviour
{
    public LayerMask layermask;
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Grid"))
        {
            var point = other.GetComponent<NeigbourCubes>();
            if (!point.neighbours.ObstacleFilled)
            { other.GetComponent<NeigbourCubes>().HasObstacle(); }
        }
    }

    public void ColliderCheck()
    {
        Collider[] hitCollider = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, layermask);

        foreach(Collider hit in hitCollider) 
        {
            //Debug.Log("In contact with " + hit.name);
            if(hit.gameObject.GetComponent<NeigbourCubes>())
            { hit.gameObject.GetComponent<NeigbourCubes>().HasObstacle(); }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
