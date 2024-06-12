using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTurrent : MonoBehaviour
{
    public float MaxRotate = 50f;
    public float MinRotate = -50f;
    public float RotateStep = 1f;
    public float RotateSpeed = 6f;
    float currentRotate;

    public KeyCode RotateRight = KeyCode.D;
    public KeyCode RotateLeft = KeyCode.A;

    void SetCurrentRotation(float rot)
    {
        currentRotate = Mathf.Clamp(rot, MinRotate, MaxRotate);
        transform.localRotation = Quaternion.Euler(0f, 0f, currentRotate);
    }
    private void Update()
    {
        if(Input.GetKey(RotateRight))
        {
            var newRotate = currentRotate + RotateStep * RotateSpeed * Time.deltaTime;
            SetCurrentRotation(newRotate);
        }
        else if(Input.GetKey(RotateLeft))
        {
            var newRotate = currentRotate + (-RotateStep) * RotateSpeed * Time.deltaTime;
            SetCurrentRotation(newRotate);
        }
    }
}
