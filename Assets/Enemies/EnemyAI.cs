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
    [HideInInspector] public InitiativeTracker tracker;
    [HideInInspector] public List<GameObject> PossibleEndPoint = new List<GameObject>();
    public List<GameObject> Route = new List<GameObject>();
    [HideInInspector] public List<GameObject> PossibleTargets = new List<GameObject>();

    public void Start()
    {
        path = new Pathfinding();
        StartPoint = GetComponentInChildren<ShowNearbyGrid>();
        grid = CubeGrid.instance;
        tracker = InitiativeTracker.instance;
    }

    public virtual void MoveSelf()
    { }

    public virtual void FindTarget()
    { }

    public IEnumerator MoveAlongRoute()
    {
        float step = 0f;
        while (Mathf.Abs(transform.position.x - Route[0].transform.position.x) > 0.1f)
        {
            rotateTowards(Route[0].transform.position);
            step += Time.deltaTime;
            float x = Mathf.Lerp(transform.position.x, Route[0].transform.position.x, step / speed);
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
        {
            FindTarget(); 
        }
    }

    void rotateTowards(Vector3 to)
    {
        Quaternion lookRotation = Quaternion.LookRotation((to - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
    }

}
