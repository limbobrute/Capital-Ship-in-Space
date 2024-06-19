using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obscatle : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Grid"))
        {
            var point = other.GetComponent<NeigbourCubes>();
            if (!point.neighbours.ObstacleFilled)
            { other.GetComponent<NeigbourCubes>().HasObstacle(); }
        }
    }
}
