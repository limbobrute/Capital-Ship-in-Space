using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    public int selectedWeapon = 0;
    public GameObject[] Turrents;
    public RotateTurrent[] Rotater;
    public BarrelAdjuster[] Adjuster;
    public FireTurret[] Shoot;

    // Start is called before the first frame update
    void Start()
    {
        //selectWeapon();    
    }

    // Update is called once per frame
    void Update()
    {
        int pre = selectedWeapon;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKeyDown(KeyCode.W))
        {
            if (selectedWeapon >= Turrents.Length - 1)
            { selectedWeapon = 0; }
            else
            { selectedWeapon++; }
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetKeyDown(KeyCode.S))
        {
            if (selectedWeapon <= 0)
            { selectedWeapon = Turrents.Length - 1; }
            else
            { selectedWeapon--; }
        }

        if(pre != selectedWeapon)
        { selectWeapon(); }
    }

    public void NextWeapon()
    {
        selectedWeapon++;
        if(selectedWeapon >= Turrents.Length)
        { selectedWeapon = 0; }
        selectWeapon();
    }

    public void selectWeapon()
    {
        int i = 0;
        foreach(GameObject weapon in Turrents)
        {
            if(i == selectedWeapon && !Shoot[i].hasFired)
            { 
                weapon.gameObject.SetActive(true); 
                Rotater[i].enabled = true; 
                Adjuster[i].enabled = true; 
                Shoot[i].enabled = true; 
            }
            else 
            { 
                weapon.gameObject.SetActive(false); 
                Rotater[i].enabled = false; 
                Adjuster[i].enabled = false;
                Shoot[i].enabled = false;
            }
            i++;

        }
    }

    public void UnFireWeapons()
    {
        for (int i = 0; i < Turrents.Length; i++)
        { Shoot[i].UnFired(); }
        selectedWeapon = 0;
    }
}
