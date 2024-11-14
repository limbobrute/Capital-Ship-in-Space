using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    public GameObject missile;
    public GrabCube startCube;
    public GameObject endCube;
    public bool canFire = false;
    public int damage = 0;
    public KeyCode Fire = KeyCode.M;
    private CubeGrid grid;
    private List<GameObject> Route = new List<GameObject>();
    private MissilePathfinding path;

    private void Start()
    {
        grid = CubeGrid.instance;
        path = new MissilePathfinding();
    }

    private void Update()
    {
        if(canFire && Input.GetKeyDown(Fire))
        {
            canFire = false;
            var prefab = Instantiate(missile, transform.position, Quaternion.Euler(Vector3.forward));
            foreach(GameObject obj in grid.Grid)
            {
                if(obj.GetComponent<NeigbourCubes>().neighbours.EnemyPartFilled)
                {
                    endCube = obj;
                    break;
                }
            }
            Route = path.FindPath(startCube.startCube, endCube);
            prefab.GetComponent<MissileBehavior>().TargetTag = "Enemy";
            prefab.GetComponent<MissileBehavior>().Route = Route;
        }
    }
}
