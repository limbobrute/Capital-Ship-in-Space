using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerSpaceShip : MonoBehaviour
{
    public GameObject SpaceShip;
    public GameObject GridCollider;
    public PlayerCameraMove ghostObject;
    public GameObject[] EngineEffects = new GameObject[3];
    public Transform GhostCam;
    public GameObject MoveCam;
    public float speed = 6f;
    public float MoveTime = 2f;
    public float RotateTime = 2f;
    private Pathfinding path;
    private List<GameObject> Route = new List<GameObject>();
    private void Start()
    {
        path = new Pathfinding();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var endCube = GridCollider.GetComponent<ShowNearbyGrid>().GridPoint;
            var startCube = SpaceShip.GetComponentInChildren<GrabCube>().startCube;
            Route = path.FindPath(startCube, endCube);
            //Debug.Log("Route length: " + Route.Count);
            foreach(var obj in EngineEffects)
            { obj.SetActive(true); obj.GetComponent<ParticleSystem>().Play(); }
        }
        
        if(Route.Count > 0)
        {
            GhostCam.gameObject.SetActive(false);
            MoveCam.SetActive(true);
            MoveAlongRoute();
        }
    }

    private void MoveAlongRoute()
    {
        var targetRotation = Quaternion.LookRotation(Route[0].transform.position - SpaceShip.transform.position);
        ghostObject.isMoving = true;
        var step = speed * Time.deltaTime;
        SpaceShip.transform.position = Vector3.MoveTowards(SpaceShip.transform.position, Route[0].transform.position, step);
        SpaceShip.transform.rotation = Quaternion.Slerp(SpaceShip.transform.rotation, targetRotation, step);
        if(Vector3.Distance(SpaceShip.transform.position, Route[0].transform.position) < .05f)
        { SpaceShip.transform.position = Route[0].transform.position; Route.RemoveAt(0); }
        if(Route.Count == 0)
        {   
            var angle = transform.rotation.eulerAngles;
            StartCoroutine(MatchGhost(angle));
        }
    }

    private IEnumerator MatchGhost(Vector3 angle)
    {
        float timer = 0f;
        while (timer <= RotateTime)
        {
            SpaceShip.transform.rotation = Quaternion.Lerp(SpaceShip.transform.rotation, Quaternion.Euler(angle), timer / RotateTime);
            SpaceShip.transform.position = Vector3.Lerp(SpaceShip.transform.position, transform.position, timer / RotateTime);
            timer += Time.deltaTime;
            yield return null;
        }
        SpaceShip.transform.rotation = transform.rotation;
        SpaceShip.transform.position = transform.position;
        foreach (var obj in EngineEffects)
        { obj.SetActive(false); }
        ghostObject.HideSelf();
        StopAllCoroutines();
    }
}
