using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehavior : MonoBehaviour
{
    public List<GameObject> Route = new List<GameObject>();
    public string TargetTag;
    public float speed = 20f;
    public int damage = 0;
    public GameObject Cam;
    private GameObject Carrier;
    private float time = 0f;
    private InitiativeTracker tracker;

    private void Start()
    {
        tracker = InitiativeTracker.instance;
        StartCoroutine(MoveAlongRoute());
    }

    // Update is called once per frame
    /*void Update()
    {
        if (Route.Count > 0)
        {
            MoveAlongRoute();
            rotateTowards(Route[0].transform.position);
        }
    }*/

    public void CameraEnable(GameObject obj)
    {
        Cam.SetActive(true);
        Carrier = obj;
    }

    /*private void MoveAlongRoute()
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
    }*/

    IEnumerator MoveAlongRoute()
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
            Debug.Log("We have failed to hit the target. Try again");
            if(Cam.activeSelf == true)
            { tracker.NextTurn(Carrier); }
            Destroy(this.gameObject);
        }
    }

    void rotateTowards(Vector3 to)
    {
        Quaternion lookRotation = Quaternion.LookRotation((to - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("We have hit " + other.gameObject.name);
        if(other.gameObject.CompareTag(TargetTag))
        {
            if(TargetTag == "Player" || TargetTag == "PlayerShipPart")
            {
                other.transform.gameObject.GetComponent<ComponentHealth>().DamageComponent(damage);
                tracker.NextTurn(Carrier);
                Destroy(gameObject);
            }
            else if (TargetTag == "Enemy")
            {
                other.GetComponent<DamageTaker>().Damage(damage);
                Destroy(gameObject);
            }
        }
        else
        { Destroy(gameObject); }
    }
}
