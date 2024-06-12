using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelRayCheck : MonoBehaviour
{
    public float distance = 10f;
    public CubeGrid grid;
    List<GameObject> LastObjects = new List<GameObject>();
    public LayerMask mask;
    private LineRenderer visableLine;

    private void Start()
    {
        visableLine = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        RaycastHit hit;   
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distance, mask);
        if(hit.transform != null)
        {
            Debug.Log("Found an Enemy");
            visableLine.enabled = true;
        }
        else
        { visableLine.enabled = false; }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * distance;
        Gizmos.DrawRay(transform.position, direction);
    }
}
