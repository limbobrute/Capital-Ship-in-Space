using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowNearbyGrid : MonoBehaviour
{
    public CubeGrid grid;
    public GameObject GridPoint;
    public InitRemover remover;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Grid"))
        {
            //Debug.Log("Grid part made contact");
            grid.ShowGrid(other.gameObject);
            GridPoint = other.gameObject;
            remover.cube = GridPoint;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Grid"))
        {
            //Debug.Log("Leaving Grid part");
            grid.HideGrid(other.gameObject);
        }
    }

    public void RemoveFromGrid()
    {
        GridPoint.GetComponent<NeigbourCubes>().EmptyPoint();
    }
}
