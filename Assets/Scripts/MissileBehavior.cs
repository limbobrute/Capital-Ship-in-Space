using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehavior : MonoBehaviour
{
    public List<GameObject> Route = new List<GameObject>();
    public string TargetTag;
    public float speed = 20f;
    public GameObject Cam;
    private GameObject Carrier;
    private float time = 0f;
    private InitiativeTracker tracker;

    private void Start()
    {
        tracker = InitiativeTracker.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Route.Count > 0)
        {
            MoveAlongRoute();
            rotateTowards(Route[0].transform.position);
        }
    }

    public void CameraEnable(GameObject obj)
    {
        Cam.SetActive(true);
        Carrier = obj;
    }

    private void MoveAlongRoute()
    {
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Route[0].transform.position, step);

        if (Vector3.Distance(transform.position, Route[0].transform.position) < .05f)
        { 
          transform.position = Route[0].transform.position;
          Route.RemoveAt(0);
          time = 0f;
        }
        if (Route.Count == 0)
        {
            if (Cam.activeSelf == true)
            {
                tracker.NextTurn(Carrier);
            }
            Destroy(gameObject);
        }
    }

    void rotateTowards(Vector3 to)
    {
        Quaternion lookRotation = Quaternion.LookRotation((to - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(TargetTag))
        {
            other.transform.gameObject.SetActive(false);
            tracker.RemoveFromInit(other.GetComponent<InitRemover>().InitObject);
            //Debug.Log("BOOM!!");
            if(Cam.activeSelf == true)
            {
                tracker.NextTurn(Carrier);
            }
            Destroy(gameObject);
        }
    }
}
