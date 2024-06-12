using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelAdjuster : MonoBehaviour
{
    public GameObject[] BarrelRotater;
    public GameObject[] indicator;
    public float MinRotate = 0f;
    public float MaxRotate = 30f;
    public float RotateStep = 1f;
    public float RotateSpeed = 6f;
    public int selectedBarrel = 0;

    public KeyCode IncrementBarrel = KeyCode.LeftArrow;
    public KeyCode DecrementBarrel = KeyCode.RightArrow;
    public KeyCode RotateUp = KeyCode.UpArrow;
    public KeyCode RotateDown = KeyCode.DownArrow;

    private float currentRotate;

    void SetCurrentRotation(float rot, GameObject barrel)
    {
        currentRotate = Mathf.Clamp(rot, MinRotate, MaxRotate);
        barrel.transform.localRotation = Quaternion.Euler(currentRotate, 0f, 0f);
    }

    private void Update()
    {
        
        int pre = selectedBarrel;
        if (Input.GetKeyDown(DecrementBarrel))
        {
            if (selectedBarrel >= BarrelRotater.Length - 1)
            { selectedBarrel = 0; }
            else
            { selectedBarrel++; }
        }
        if (Input.GetKeyDown(IncrementBarrel))
        {
            if (selectedBarrel <= 0)
            { selectedBarrel = BarrelRotater.Length - 1; }
            else
            { selectedBarrel--; }
        }
        if(pre != selectedBarrel)
        {
            for(int i = 0; i < BarrelRotater.Length; i++)
            {
                if(selectedBarrel != i)
                { indicator[i].SetActive(false);}
                else
                { indicator[i].SetActive(true); }
            }
        }

        RotateBarrel(selectedBarrel);
    }

    void RotateBarrel(int barrel)
    {
        var b = BarrelRotater[barrel];
        currentRotate = b.transform.localEulerAngles.x;
        //Debug.Log("currentRotate on " + gameObject.name + " after grabbing the current barrel's local X rotation is " + currentRotate);
        if (Input.GetKey(RotateDown))
        {
            var newRotate = currentRotate + RotateStep * RotateSpeed * Time.deltaTime;
            //Debug.Log("Raw value to rotate to on " + gameObject.name + " is " + newRotate);
            SetCurrentRotation(newRotate, b);
        }
        else if (Input.GetKey(RotateUp))
        {
            var newRotate = currentRotate + (-RotateStep) * RotateSpeed * Time.deltaTime;
            //Debug.Log("Raw value to rotate to on " + gameObject.name + " is " + newRotate);
            SetCurrentRotation(newRotate, b);
        }
    }

}
