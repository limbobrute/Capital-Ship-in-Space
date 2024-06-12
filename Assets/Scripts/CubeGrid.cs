using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGrid : MonoBehaviour
{
    public static CubeGrid instance;
    public GameObject OriginPrefab;
    public GameObject BuilderPrefab;
    public int GridSize = 5;
    public int Distance = 2;
    public List<GameObject> Grid = new List<GameObject>();
    public List<NeigbourCubes.Neighbours> neighbours = new List<NeigbourCubes.Neighbours>();
    private bool FindNeighbours = false;
    private int TotalCount = 0;
    private List<GameObject> Axis = new List<GameObject>();
    private InitiativeTracker tracker;

    private void Awake()
    {
        if(instance != null && instance != this)
        { Destroy(this); }
        else
        { instance = this; }
    }

    void Start()
    {
        tracker = InitiativeTracker.instance;
        var prefab = Instantiate(OriginPrefab, transform.position, Quaternion.identity);
        prefab.name = "(0, 0, 0)";
        prefab.transform.parent = this.transform;
        prefab.GetComponent<MeshRenderer>().enabled = false;
        Grid.Add(prefab);
        StartCoroutine(BuildAxis());
        TotalCount = ((GridSize * 2) + 1) * ((GridSize * 2) + 1) * ((GridSize * 2) + 1);
    }

    public void AddObj(GameObject obj)
    {
        Grid.Add(obj);
    }

    public void ShowGrid(GameObject obj)
    {
        foreach(GameObject o in Grid)
        {
            if(obj == o)
            { obj.GetComponent<MeshRenderer>().enabled = true; break; }
        }
    }

    public void HideGrid(GameObject obj)
    {
        foreach (GameObject o in Grid)
        {
            if (obj == o)
            { obj.GetComponent<MeshRenderer>().enabled = false; break; }
        }
    }

    public void Naming(GameObject obj)
    {
            string x = obj.transform.position.x.ToString();
            string y = obj.transform.position.y.ToString();
            string z = obj.transform.position.z.ToString();
            obj.name = "(" + x + ", " + y + ", " + z + ")";
    }

    private void Update()
    {
        if(TotalCount == Grid.Count && !FindNeighbours)
        {
            FindNeighbours = true;
            tracker.OrderInit();
            StartCoroutine(NeigbhourSearch());
        }
    }

    private IEnumerator NeigbhourSearch()
    {
        foreach(GameObject obj in Grid)
        {
            neighbours.Add(obj.GetComponent<NeigbourCubes>().FindNeigbours());
            yield return null;
        }
        tracker.StartGame();
        StopAllCoroutines();
    }

    private IEnumerator BuildAxis()
    {
        for (int i = 0; i < GridSize; i++)
        {
            var Forward = Instantiate(BuilderPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + (Distance * (i + 1))), Quaternion.identity);
            var Backward = Instantiate(BuilderPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z - (Distance * (i + 1))), Quaternion.identity);
            var Up = Instantiate(BuilderPrefab, new Vector3(transform.position.x, transform.position.y + (Distance * (i + 1)), transform.position.z), Quaternion.identity);
            var Down = Instantiate(BuilderPrefab, new Vector3(transform.position.x, transform.position.y - (Distance * (i + 1)), transform.position.z), Quaternion.identity);
            var Right = Instantiate(BuilderPrefab, new Vector3(transform.position.x + (Distance * (i + 1)), transform.position.y, transform.position.z), Quaternion.identity);
            var Left = Instantiate(BuilderPrefab, new Vector3(transform.position.x - (Distance * (i + 1)), transform.position.y, transform.position.z), Quaternion.identity);

            Forward.transform.parent = this.transform;
            Backward.transform.parent = this.transform;
            Forward.GetComponent<GridCompleter>().Direction = GridCompleter.direction.Z;
            Backward.GetComponent<GridCompleter>().Direction = GridCompleter.direction.Z;

            Up.transform.parent = this.transform;
            Down.transform.parent = this.transform;
            Up.GetComponent<GridCompleter>().Direction = GridCompleter.direction.Y;
            Down.GetComponent<GridCompleter>().Direction = GridCompleter.direction.Y;

            Right.transform.parent = this.transform;
            Left.transform.parent = this.transform;
            Right.GetComponent<GridCompleter>().Direction = GridCompleter.direction.X;
            Left.GetComponent<GridCompleter>().Direction = GridCompleter.direction.X;
            Axis.Add(Up);
            Axis.Add(Down);
            Axis.Add(Forward);
            Axis.Add(Backward);
            Axis.Add(Left);
            Axis.Add(Right);
            Grid.Add(Up);
            Grid.Add(Down);
            Grid.Add(Forward);
            Grid.Add(Backward);
            Grid.Add(Left);
            Grid.Add(Right);
            Naming(Up);
            Naming(Down);
            Naming(Forward);
            Naming(Backward);
            Naming(Right);
            Naming(Left);
            Up.GetComponent<MeshRenderer>().enabled = false;
            Down.GetComponent<MeshRenderer>().enabled = false;
            Forward.GetComponent<MeshRenderer>().enabled = false;
            Backward.GetComponent<MeshRenderer>().enabled = false;
            Right.GetComponent<MeshRenderer>().enabled = false;
            Left.GetComponent<MeshRenderer>().enabled = false;
            yield return new WaitForSeconds(.1f);
        }
        StartCoroutine(BuildGrid());
        StopCoroutine(BuildAxis());
    }

    private IEnumerator BuildGrid()
    {
        foreach(GameObject obj in Axis)
        {
            if (obj.GetComponent<GridCompleter>() != null)
            { obj.GetComponent<GridCompleter>().BuildGrid(); }
            yield return new WaitForSeconds(.1f);
        }
    }
}
