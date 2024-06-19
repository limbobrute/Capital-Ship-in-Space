using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject Player;
    public GameObject GridCollider;
    public GameObject View;
    public int TravelDistance = 0;
    public float speed = 6f;
    public float RotateStep = 6f;
    public float TimeToTurn = 3f;
    [HideInInspector] public CubeGrid grid;
    [HideInInspector] public ShowNearbyGrid StartPoint;
    [HideInInspector] public GameObject EndPoint;
    [HideInInspector] public Pathfinding path;
    [HideInInspector]public List<GameObject> PossibleEndPoint = new List<GameObject>();
    public List<GameObject> Route = new List<GameObject>();
    [HideInInspector]public List<GameObject> PossibleTargets = new List<GameObject>();

    public void Start()
    {
        path = new Pathfinding();
        StartPoint = GetComponentInChildren<ShowNearbyGrid>();
        grid = CubeGrid.instance;
    }

    public virtual void MoveSelf()
    { }

    /*public void MoveAlongRoute()
    {
        var targetRotation = Quaternion.LookRotation(Route[0].transform.position - transform.position);
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Route[0].transform.position, step);
        transform.position = Vector3.MoveTowards(transform.position, Route[0].transform.position, step);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
        if (Vector3.Distance(transform.position, Route[0].transform.position) < .05f)
        { transform.position = Route[0].transform.position; Route.RemoveAt(0); }

        if (Route.Count == 0)
        {
            //StartCoroutine(MoveTurent());
            FindTarget();
        }
    }*/

    public virtual void FindTarget()
    { }

    public IEnumerator MoveAlongRoute()
    {
        float step = 0f;
        while (Mathf.Abs(transform.position.x - Route[0].transform.position.x) > 0.1f)
        {
            rotateTowards(Route[0].transform.position);
            step += Time.deltaTime;
            float x = Mathf.Lerp(transform.position.x, Route[0].transform.position.x, step/speed);
            float y = Mathf.Lerp(transform.position.y, Route[0].transform.position.y, step / speed);
            float z = Mathf.Lerp(transform.position.z, Route[0].transform.position.z, step / speed);
            transform.position = new Vector3(x, y, z);
            yield return null;
        }
        
        transform.position = Route[0].transform.position;
        Route.RemoveAt(0);
        if (Route.Count > 0)
        { StartCoroutine(MoveAlongRoute()); }
        else
        { FindTarget(); }
    }

    void rotateTowards(Vector3 to)
    {
        Quaternion lookRotation = Quaternion.LookRotation((to - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
    }
    /*
    public GameObject Player;
    public GameObject GridCollider;
    public GameObject TurrentRotatePoint;
    public GameObject View;
    public int TravelDistance = 0;
    public int ShootingDistance = 30;
    public float speed = 6f;
    public float RotateStep = 6f;
    public float TimeToTurn = 3f;
    private CubeGrid grid;
    private GameObject EndPoint;
    private Pathfinding path;
    private ShowNearbyGrid StartPoint;
    private List<GameObject> PossibleEndPoint = new List<GameObject>();
    private List<GameObject> Route = new List<GameObject>();
    private List<GameObject> PossibleTargets = new List<GameObject>();


    private void Start()
    {
        path = new Pathfinding();
        StartPoint = GetComponentInChildren<ShowNearbyGrid>();
        grid = CubeGrid.instance;
    }

    private void Update()
    {
        if (Route.Count > 0)
        {
            MoveAlongRoute();
        }
    }

    public void MoveSelf()
    {
        var GridPoint = StartPoint.GetComponent<ShowNearbyGrid>().GridPoint;
        if(PossibleEndPoint.Count != 0)
        { PossibleEndPoint.Clear(); }

        foreach(GameObject obj in grid.Grid)
        {
            var distance = Vector3.Distance(transform.position, obj.transform.position);
            if ((distance <= TravelDistance*10 && obj != GridPoint) && !obj.GetComponent<NeigbourCubes>().neighbours.EnemyPartFilled && !obj.GetComponent<NeigbourCubes>().neighbours.PlayerHull)
            {
                obj.GetComponent<NeigbourCubes>().EnemyMoveToValue += (int)distance;
                var rangeFromPlayer = Vector3.Distance(obj.transform.position, Player.transform.position) / ShootingDistance;
                obj.GetComponent<NeigbourCubes>().EnemyMoveToValue += (int)rangeFromPlayer;
                var yValue = Mathf.Abs(obj.transform.position.y - Player.transform.position.y);
                obj.GetComponent<NeigbourCubes>().EnemyMoveToValue -= (int)yValue;
                //grid.ShowGrid(obj);//Debug tool to show all possible locations for this AI to go to
                PossibleEndPoint.Add(obj);
            }
        }

        EndPoint = PossibleEndPoint.OrderByDescending(x => x.GetComponent<NeigbourCubes>().EnemyMoveToValue).First();
        Debug.Log("Enemy is moving towards " + EndPoint.name);

        Route = path.FindPath(GridPoint, EndPoint);
    }

    private void MoveAlongRoute()
    {
        var targetRotation = Quaternion.LookRotation(Route[0].transform.position - transform.position);
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Route[0].transform.position, step);
        transform.position = Vector3.MoveTowards(transform.position, Route[0].transform.position, step);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
        if (Vector3.Distance(transform.position, Route[0].transform.position) < .05f)
        { transform.position = Route[0].transform.position; Route.RemoveAt(0); }

        if(Route.Count == 0)
        {
            //StartCoroutine(MoveTurent());
            FindTarget();
        }
    }

    private void FindTarget()
    {
        if(PossibleTargets.Count != 0)
        { PossibleTargets.Clear(); }

        foreach (GameObject obj in grid.Grid)
        {
            var partcheck = obj.GetComponent<NeigbourCubes>();
            if(partcheck.neighbours.PlayerHull || partcheck.neighbours.PlayerPartFilled)
            { PossibleTargets.Add(obj); grid.ShowGrid(obj); }
        }

        var target = PossibleTargets.OrderByDescending(x => x.GetComponent<NeigbourCubes>().EnemyTargetValue).First();
        Debug.Log("Target location is " + target.name);
        StartCoroutine(MoveTurent(target));
    }

    IEnumerator MoveTurent(GameObject target)
    {
        Quaternion from = TurrentRotatePoint.transform.localRotation;
        Quaternion to = TurrentRotatePoint.transform.localRotation;

        Vector3 targetDirection = target.transform.position - TurrentRotatePoint.transform.position;
        Vector3 forward = TurrentRotatePoint.transform.up;//This comes from the blender file being Blender

        float angle = Vector3.SignedAngle(targetDirection, forward, Vector3.forward);
        Debug.Log("Total value to rotate is " + angle);

        to *= Quaternion.Euler(Vector3.forward * angle);
        float timer = 0f;
        while(timer < TimeToTurn)
        {
            TurrentRotatePoint.transform.localRotation = Quaternion.Slerp(from, to, timer / TimeToTurn);
            timer += Time.deltaTime;
            yield return null;
        }

        TurrentRotatePoint.transform.localRotation = to;
        TurrentRotatePoint.GetComponent<EnemyFireCannon>().CheckFire();
    }*/

}
