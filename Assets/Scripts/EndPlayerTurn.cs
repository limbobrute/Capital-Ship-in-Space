using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlayerTurn : MonoBehaviour
{
    public GameObject player;
    public GameObject Cam;
    private int GunsFired = 0;
    private InitiativeTracker tracker;

    private void Start()
    {
        tracker = InitiativeTracker.instance;
    }

    public void Fired()
    {
        GunsFired++;
        if(GunsFired == 5)
        {
            GunsFired = 0;
            StartCoroutine(DelayNextTurn());
        }
    }

    private IEnumerator DelayNextTurn()
    {
        Cam.SetActive(true);
        yield return new WaitForSeconds(2f);
        Cam.SetActive(false);
        tracker.NextTurn(player);
    }
}
