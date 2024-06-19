using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class InitiativeTracker : MonoBehaviour
{
    public GameObject PlayerGhost;
    public GameObject Background;
    public GameObject LoadingText;
    public GameObject VictoryText;
    public GameObject DefeatText;
    public static InitiativeTracker instance;
    public List<GameObject> initiative = new List<GameObject>();

    private void Awake()
    {
        if (instance != null && instance != this)
        { Destroy(this); }
        else
        { instance = this; }
    }

    public void AddToInit(GameObject Entity)
    {
        initiative.Add(Entity);
    }

    public void RemoveFromInit(GameObject Entity)
    {
        initiative.Remove(Entity);
        if (Entity.name == "Player")
        {
            Debug.Log("GAME OVER");
            PlayerGhost.SetActive(false);
            DefeatText.SetActive(true);
        }

        if(initiative.Count == 1 && initiative[0].name == "Player")
        { Debug.Log("YOU WIN!!"); VictoryText.SetActive(true); }
    }

    public void OrderInit()
    {
        initiative = initiative.OrderByDescending((x => x.GetComponent<InitScore>().initiative)).ToList();
        Debug.Log("Current number of turns:" + initiative.Count);
    }

    public void StartGame()
    {
        Background.SetActive(false);
        LoadingText.SetActive(false);
        if (!initiative[0].GetComponent<EnemyAI>())
        { PlayerGhost.SetActive(true); Camera.main.gameObject.SetActive(true); }
        else if (initiative[0].GetComponent<SmallPatrol>())
        {
            initiative[0].GetComponent<SmallPatrol>().MoveSelf();
            initiative[0].GetComponent<SmallPatrol>().View.SetActive(true);
            Camera.main.gameObject.SetActive(false);
        }
        else
        { 
            initiative[0].GetComponent<EnemyMissileCarrier>().MoveSelf();
            initiative[0].GetComponent<EnemyMissileCarrier>().View.SetActive(true);
            Camera.main.gameObject.SetActive(false); 
        }
    }

    public void NextTurn(GameObject Entity)
    {
        Debug.Log("Turn that just ended belongs to " + Entity.name);
        int index = initiative.IndexOf(Entity);
        Debug.Log("Turn that ended was at index " + index);
        index += 1;
        Debug.Log("Move to turn " + index + " in the tracker");
        if (index == initiative.Count)
        { index = 0; }

        if (initiative[index].GetComponent<SmallPatrol>())
        { 
            var Enemy = initiative[index].GetComponent<SmallPatrol>();
            Enemy.View.SetActive(true);
            Enemy.MoveSelf(); 
        }
        else if(initiative[index].GetComponent<EnemyMissileCarrier>())
        {
            var Enemy = initiative[index].GetComponent<EnemyMissileCarrier>();
            Enemy.View.SetActive(true);
            Enemy.MoveSelf();
        }
        else
        { 
            PlayerGhost.SetActive(true);
            PlayerGhost.GetComponent<PlayerCameraMove>().NewTurn();
        }
    }
}
