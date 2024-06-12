using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCube : MonoBehaviour
{
    public GameObject startCube;
    private void OnTriggerStay(Collider other)
    {
        if(other.transform.gameObject.CompareTag("Grid"))
        { startCube = other.transform.gameObject; }
    }
}
