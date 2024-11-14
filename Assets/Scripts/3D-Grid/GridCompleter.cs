using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCompleter : MonoBehaviour
{
    public bool Finished = false;
    public enum direction { X, Y, Z};
    public direction Direction;
    public CubeGrid grid;
    private int Distance;
    public GameObject Cube;
    public GameObject GridManager;
    private List<GameObject> Points = new List<GameObject>();

    private void Start()
    {
        Cube = GameObject.Find("(0, 0, 0)");
        grid = GameObject.Find("Grid").GetComponent<CubeGrid>();
        Distance = grid.Distance;
        GridManager = GameObject.Find("Grid");
    }
    public void BuildGrid()
    {
        if (!Finished)
        {
            StartCoroutine(GridFinish());
            Finished = true;
        }   
    }

    public void Add()
    {
        foreach(GameObject obj in Points)
        { grid.AddObj(obj); }
    }

    private IEnumerator GridFinish()
    {
        for (int i = 0; i < grid.GridSize; i++)
        {
            switch (Direction)
            {
                case direction.X:
                    var prefabRight = Instantiate(Cube, new Vector3(transform.position.x, transform.position.y, transform.position.z + (Distance * (i + 1))), Quaternion.identity);
                    var prefabLeft = Instantiate(Cube, new Vector3(transform.position.x, transform.position.y, transform.position.z - (Distance * (i + 1))), Quaternion.identity);
                    grid.Naming(prefabRight);
                    grid.Naming(prefabLeft);
                    prefabRight.transform.parent = GridManager.transform;
                    prefabLeft.transform.parent = GridManager.transform;
                    prefabRight.AddComponent<GridFiller>();
                    prefabLeft.AddComponent<GridFiller>();
                    grid.AddObj(prefabRight);
                    grid.AddObj(prefabLeft);
                    break;

                case direction.Y:
                    var prefabUp = Instantiate(Cube, new Vector3(transform.position.x + (Distance * (i + 1)), transform.position.y, transform.position.z), Quaternion.identity);
                    var prefabDown = Instantiate(Cube, new Vector3(transform.position.x - (Distance * (i + 1)), transform.position.y, transform.position.z), Quaternion.identity);
                    grid.Naming(prefabUp);
                    grid.Naming(prefabDown);
                    prefabUp.transform.parent = GridManager.transform;
                    prefabDown.transform.parent = GridManager.transform;
                    grid.AddObj(prefabUp);
                    grid.AddObj(prefabDown);
                    break;

                case direction.Z:
                    var prefabForward = Instantiate(Cube, new Vector3(transform.position.x, transform.position.y + (Distance * (i + 1)), transform.position.z), Quaternion.identity);
                    var prefabBackward = Instantiate(Cube, new Vector3(transform.position.x, transform.position.y - (Distance * (i + 1)), transform.position.z), Quaternion.identity);
                    grid.Naming(prefabForward);
                    grid.Naming(prefabBackward);
                    prefabForward.transform.parent = GridManager.transform;
                    prefabBackward.transform.parent = GridManager.transform;
                    grid.AddObj(prefabForward);
                    grid.AddObj(prefabBackward);
                    break;
            }
            yield return new WaitForSeconds(.1f);
        }
    }
}
