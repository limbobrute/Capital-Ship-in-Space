using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMissileCarrier : EnemyAI
{
    /*public GameObject Player;
    public GameObject GridCollider;*/
    public GameObject MissileLaunchSite;
    public GameObject Missile;
    //public GameObject View;
    //public int TravelDistance = 0;
    public int MissileTargetingDistance = 30;
    //public float speed = 6f;
    //public float RotateStep = 6f;
    //public float TimeToTurn = 3f;
    //private GameObject EndPoint;
    //private Pathfinding path;
    private List<GameObject> MissileRoute = new List<GameObject>();
    private MissilePathfinding Missilepath;
    //private ShowNearbyGrid StartPoint;
    //private List<GameObject> PossibleEndPoint = new List<GameObject>();
    //private List<GameObject> Route = new List<GameObject>();
    //private List<GameObject> PossibleTargets = new List<GameObject>();
    //private CubeGrid grid;
    private InitiativeTracker tracker;
    
    private void Awake()
    {
        //path = new Pathfinding();
        //StartPoint = GetComponentInChildren<ShowNearbyGrid>();
        Missilepath = new MissilePathfinding();
        //grid = CubeGrid.instance;
        tracker = InitiativeTracker.instance;
    }

    /*private void Update()
    {
        if (Route.Count > 0)
        {
            MoveAlongRoute();
        }
    }*/

    public override void MoveSelf()
    {
        RaycastHit hit;
        var GridPoint = StartPoint.GetComponent<ShowNearbyGrid>().GridPoint;
        if (PossibleEndPoint.Count != 0)
        { PossibleEndPoint.Clear(); }

        foreach (GameObject obj in grid.Grid)
        {
            var distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance <= TravelDistance * 10 && obj != GridPoint)
            {
                obj.GetComponent<NeigbourCubes>().EnemyMoveToValue += (int)distance;
                var rangeFromPlayer = Vector3.Distance(obj.transform.position, Player.transform.position) / MissileTargetingDistance;
                obj.GetComponent<NeigbourCubes>().EnemyMoveToValue += (int)rangeFromPlayer;
                Physics.Raycast(obj.transform.position, (Player.transform.position - obj.transform.position), out hit, Mathf.Infinity);
                if(hit.collider.gameObject.CompareTag("Player"))
                { obj.GetComponent<NeigbourCubes>().EnemyMoveToValue -= 10000; }
                //grid.ShowGrid(obj);//Debug tool to show all possible locations for this AI to go to
                PossibleEndPoint.Add(obj);
            }
        }

        EndPoint = PossibleEndPoint.OrderByDescending(x => x.GetComponent<NeigbourCubes>().EnemyMoveToValue).First();
        Debug.Log("Enemy is moving towards " + EndPoint.name);

        Route = path.FindPath(GridPoint, EndPoint);
        StartCoroutine(MoveAlongRoute());
    }

    /*private void MoveAlongRoute()
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
            FindTarget();
        }
    }*/

    public override void FindTarget()
    {
        //var prefab = Instantiate(Missile, MissileLaunchSite.transform.position, Quaternion.Euler(Vector3.up));
        foreach (GameObject obj in grid.Grid)
        {
            var partcheck = obj.GetComponent<NeigbourCubes>();
            if (partcheck.neighbours.PlayerHull)
            { EndPoint = obj; break; }

            
        }
        MissileRoute = Missilepath.FindPath(GridCollider.GetComponent<ShowNearbyGrid>().GridPoint, EndPoint);
        if ((MissileRoute.Count + 1) > MissileTargetingDistance)
        {
            View.SetActive(false);
            tracker.NextTurn(this.gameObject);
        }
        else
        {
            var prefab = Instantiate(Missile, MissileLaunchSite.transform.position, Quaternion.Euler(Vector3.forward));
            var MB = prefab.GetComponent<MissileBehavior>();
            MB.TargetTag = "Player";
            MB.CameraEnable(this.gameObject);
            MB.Route = MissileRoute;
            View.SetActive(false);
        }
    }
}
