using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmallPatrol : EnemyAI
{
    public GameObject TurrentRotatePoint;
    public GameObject CircleCastPoint;
    public GameObject CircleCastRotatePoint;
    public LayerMask mask;
    public int ShootingDistance = 30;
    public int CannonDamage = 10;

    public override void MoveSelf()
    {
        var GridPoint = StartPoint.GetComponent<ShowNearbyGrid>().GridPoint;
        if (PossibleEndPoint.Count != 0)
        { PossibleEndPoint.Clear(); }

        foreach (GameObject obj in grid.Grid)
        {
            NeigbourCubes neighbours = obj.GetComponent<NeigbourCubes>();
            var distance = Vector3.Distance(transform.position, obj.transform.position);
            if (neighbours.neighbours.ObstacleFilled || neighbours.neighbours.PlayerPartFilled || neighbours.neighbours.EnemyPartFilled) 
            { continue; }
            else if (distance <= TravelDistance * 10 && !obj.GetComponent<NeigbourCubes>().neighbours.EnemyPartFilled && !obj.GetComponent<NeigbourCubes>().neighbours.PlayerHull)
            {
                obj.GetComponent<NeigbourCubes>().EnemyMoveToValue += (int)distance;
                var rangeFromPlayer = Vector3.Distance(obj.transform.position, Player.transform.position);
                obj.GetComponent<NeigbourCubes>().EnemyMoveToValue -= (int)rangeFromPlayer;
                //grid.ShowGrid(obj);//Debug tool to show all possible locations for this AI to go to
                PossibleEndPoint.Add(obj);
            }
        }

        List<GameObject> temp = PossibleEndPoint.OrderByDescending(x => x.GetComponent<NeigbourCubes>().EnemyMoveToValue).ToList();
        //temp.Reverse();
        EndPoint = temp[0];
        Debug.Log("Enemy is moving towards " + EndPoint.name);

        Route = path.FindPath(GridPoint, EndPoint);
        StartCoroutine(MoveAlongRoute());
    }

    public override void FindTarget()
    {
        bool found = false;
        float zRotate = 0f;
        CircleCastRotatePoint.transform.parent = null;
        CircleCastRotatePoint.transform.position = transform.position;

        for (int i = 0; i < 180; i++)
        {
            CircleCastRotatePoint.transform.localRotation = Quaternion.Euler(i, 0f, 0f);
            for (int j = 0; j < 361; j++)
            {
                CircleCastPoint.transform.localRotation = Quaternion.Euler(0f, j, 0f);
                Vector3 forward = CircleCastPoint.transform.TransformDirection(Vector3.forward * ShootingDistance);
                Debug.DrawRay(CircleCastPoint.transform.position, forward, Color.white);

                if (Physics.Raycast(CircleCastPoint.transform.position, forward, float.MaxValue, mask, QueryTriggerInteraction.Collide))
                {
                    Debug.Log("You've got the player!!");
                    found = true;
                    zRotate = j;
                    break;
                }


                //yield return null;
            }
            if (found)
            { break; }
        }

        if (found)
        { StartCoroutine(RotateShip(CircleCastRotatePoint.transform, zRotate)); }
        //StartCoroutine(Search());
        /*
        float rays = 360f;
        float totalAngle = 360f;
        float delta = totalAngle / rays;
        float xRotate = 0f;
        bool found = false;
        Vector3 pos = CircleCastPoint.transform.position;
        Transform t = CircleCastRotatePoint.transform;
        t.parent = null;
        //t.rotation = transform.rotation;
        for (int i = 0; i < 181; i++)
        {
            CircleCastRotatePoint.transform.localRotation = Quaternion.Euler(i, t.rotation.y, t.rotation.z);
            for (int j = 0; j < rays; j++)
            {
                //yield return null;
                RaycastHit hit;
                var dir = Quaternion.Euler(i, j * delta, 0f) * transform.forward;
                if (Physics.Raycast(pos, dir, out hit, Mathf.Infinity, mask, QueryTriggerInteraction.Collide))
                {
                    if (hit.transform.gameObject.CompareTag("Player") || hit.transform.gameObject.CompareTag("PlayerShipPart"))
                    {
                        Debug.Log("We got a rotation");
                        Debug.Log("Target is " + hit.transform.gameObject.name);
                        xRotate = i;
                        found = true;
                        break;
                    }
                    else
                    { Debug.Log("We hit " + hit.transform.gameObject.name); }
                }
                Debug.DrawRay(pos, dir * ShootingDistance, Color.green);
                //yield return new WaitForSeconds(0.1f);
            }
            if (found)
            { break; }
        }

        //t.parent = transform;
        StartCoroutine(RotateShip(t));*/

        /*
        //Debug.Log("Now searching for the enemy");
        RaycastHit hit;
        bool found = false;
        float xRotate = 0f;
        //float yRotate = 0f;
        CircleCastRotatePoint.transform.parent = null;
        CircleCastRotatePoint.transform.localRotation = transform.rotation;
        for (float i = 0; i < 180; i+= 0.5f)
        {
            //Debug.Log("Loop i");
            CircleCastRotatePoint.transform.rotation = Quaternion.Euler(new Vector3(i, CircleCastRotatePoint.transform.rotation.y, CircleCastRotatePoint.transform.rotation.z));
            for(float j = 0; j < 360; j+= 0.5f)
            {
                //Debug.Log("Loop j");
                CircleCastPoint.transform.rotation = Quaternion.Euler(new Vector3(0f, j, 0f));
                if (Physics.Raycast(CircleCastPoint.transform.position, Vector3.forward, out hit, Mathf.Infinity, mask))
                {
                    if (hit.collider.gameObject.CompareTag("Player") || hit.collider.gameObject.CompareTag("PlayerShipPart"))
                    {
                        Debug.Log("Found the needed rotation for the ship");
                        found = true;
                        Debug.Log("xRotate value should be " + i);
                        xRotate = i;
                        //yRotate = j;
                        break;
                    }
                }
            }
            if(found)
            { xRotate = i; Debug.Log("Leaving loop"); break; }
        }

        Debug.Log("Rotation for the x-axis is " + xRotate.ToString());

        if (Mathf.Abs(xRotate) == 0f || Mathf.Abs(xRotate) == 180f)
        {
            CircleCastRotatePoint.transform.parent = transform;
            if (PossibleTargets.Count != 0)
            { PossibleTargets.Clear(); }

            foreach (GameObject obj in grid.Grid)
            {
                var partcheck = obj.GetComponent<NeigbourCubes>();
                if (partcheck.neighbours.PlayerHull || partcheck.neighbours.PlayerPartFilled)
                { PossibleTargets.Add(obj); grid.ShowGrid(obj); }
            }

            var target = PossibleTargets.OrderByDescending(x => x.GetComponent<NeigbourCubes>().EnemyTargetValue).First();
            Debug.Log("Target location is " + target.name);
            StartCoroutine(MoveTurent(target));
        }
        else
        { StartCoroutine(RotateShip(xRotate)); }*/
    }

    IEnumerator RotateShip(Transform to, float zRotate)
    {
        float x = to.rotation.x;
        Debug.Log("X rotation is: " + x);
        if (!(Mathf.Abs(x) == 0f) || !(Mathf.Abs(x) == 180f))
        {
            float rx = transform.rotation.x;
            float timer = 0f;
            while (timer < TimeToTurn)
            {
                timer += (Time.deltaTime / 2f);
                transform.rotation = Quaternion.Slerp(transform.rotation, to.rotation, timer / TimeToTurn);
                //rx = Mathf.Lerp(rx, x, timer / TimeToTurn);
                //transform.rotation = Quaternion.Euler(rx, 0f, 0f);
                yield return null;
            }
            transform.rotation = to.rotation;
        }
        to.parent = transform;
        to.localRotation = Quaternion.Euler(0f, 0f, 0f);

        if (PossibleTargets.Count != 0)
        { PossibleTargets.Clear(); }

        foreach (GameObject obj in grid.Grid)
        {
            var partcheck = obj.GetComponent<NeigbourCubes>();
            if (partcheck.neighbours.PlayerHull || partcheck.neighbours.PlayerPartFilled)
            { PossibleTargets.Add(obj); /*grid.ShowGrid(obj);*/ }
        }

        var target = PossibleTargets.OrderByDescending(x => x.GetComponent<NeigbourCubes>().EnemyTargetValue).First();
        Debug.Log("Target location is " + target.name);
        StartCoroutine(MoveTurent(zRotate));

    }

    IEnumerator MoveTurent(float zRotate)
    {
        float timer = 0f;
        float degrees = 5f;
        while(timer < TimeToTurn)
        {
            TurrentRotatePoint.transform.Rotate(0, 0, degrees * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        TurrentRotatePoint.transform.localRotation = Quaternion.Euler(0f, 0f, zRotate);

        TurrentRotatePoint.GetComponent<EnemyFireCannon>().CheckFire();
    }

    IEnumerator MoveTurent(GameObject target)
    {
        
        Quaternion from = TurrentRotatePoint.transform.localRotation;
        Quaternion to = TurrentRotatePoint.transform.localRotation;

        Vector3 targetDirection = target.transform.position - transform.position;
        Vector3 forward = TurrentRotatePoint.transform.up;//This comes from the blender file being Blender

        float angle = Vector3.SignedAngle(forward, targetDirection, Vector3.up);
        //float angle = Vector3.Angle(forward, targetDirection);
        Debug.Log("Total value to rotate is " + angle);
        if(angle > -7.1f && angle < 7.1f)
        { angle = 0f; }

        to *= Quaternion.Euler(Vector3.forward * angle);
        float timer = 0f;
        while (timer < TimeToTurn)
        {
            TurrentRotatePoint.transform.localRotation = Quaternion.Slerp(from, to, timer / TimeToTurn);
            timer += Time.deltaTime;
            yield return null;
        }

        TurrentRotatePoint.transform.localRotation = to;
        TurrentRotatePoint.GetComponent<EnemyFireCannon>().CheckFire();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = CircleCastPoint.transform.TransformDirection(Vector3.forward * ShootingDistance);
        //Gizmos.DrawRay(CircleCastPoint.transform.position, direction);
    }
}
