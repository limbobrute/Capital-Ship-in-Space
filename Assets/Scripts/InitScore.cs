using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitScore : MonoBehaviour
{
    public int initiative = 0;
    private InitiativeTracker tracker;

    private void Start()
    {
        tracker = InitiativeTracker.instance;
        initiative = Random.Range(1, 20);
        tracker.AddToInit(this.gameObject);
    }
}
