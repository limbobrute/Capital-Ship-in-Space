using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentHealth : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int TargetValue;

    public HealthBar healthBar;

    private void Start()
    {
        healthBar.SetMaxHealth(health);
    }


    public void DamageComponent(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
    }

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
