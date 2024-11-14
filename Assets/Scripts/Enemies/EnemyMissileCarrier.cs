using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMissileCarrier : EnemyAI
{
    public GameObject MissileLaunchSite;
    public GameObject Missile;
    public int MissileTargetingDistance = 30;
    public int MissileDamage = 0;
    private List<GameObject> MissileRoute = new List<GameObject>();
    private MissilePathfinding Missilepath;
    
    private void Awake()
    {
        Missilepath = new MissilePathfinding();
    }

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

    public override void FindTarget()
    {
        Debug.Log("Now searching for the player");
        foreach (GameObject obj in grid.Grid)
        {
            var partcheck = obj.GetComponent<NeigbourCubes>();
            if (partcheck.neighbours.PlayerHull)
            { EndPoint = obj; break; }

            
        }
        MissileRoute = Missilepath.FindPath(GridCollider.GetComponent<ShowNearbyGrid>().GridPoint, EndPoint);
        if ((MissileRoute.Count + 1) > MissileTargetingDistance)
        {
            Debug.Log("Too far, please hold");
            View.SetActive(false);
            tracker.NextTurn(this.gameObject);
        }
        else
        {
            Debug.Log("Launching missile");
            var prefab = Instantiate(Missile, MissileLaunchSite.transform.position, Quaternion.Euler(Vector3.forward));
            prefab.name = "EnemyMissile";
            var MB = prefab.GetComponent<MissileBehavior>();
            MB.damage = MissileDamage;
            MB.TargetTag = "Player";
            MB.CameraEnable(this.gameObject);
            MB.Route = MissileRoute;
            View.SetActive(false);
        }
    }
}
