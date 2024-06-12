using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentHealth : MonoBehaviour
{
    public int health;
    [SerializeField] private int TargetValue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Grid"))
        {
            var cube = other.GetComponent<NeigbourCubes>();
            cube.EnemyTargetValue = TargetValue;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Grid"))
        {
            var cube = other.GetComponent<NeigbourCubes>();
            Vector3 Center = transform.position;
            Vector3 CubeCenter = other.transform.position;
            float percentage = Vector3.Distance(Center, CubeCenter) / 100f;
            cube.EnemyTargetValue += percentage;
        }
    }
}
