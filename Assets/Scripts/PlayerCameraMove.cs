using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMove : MonoBehaviour
{
    #region VARIABLES
    public bool isMoving = false;
    public float speed = 6f;
    public float SmoothTime = 0.1f;
    public float RotateStep = 1f;
    public GameObject StartTurrent;
    public Transform GhostCam;
    public Transform Cam;
    public WeaponSwap swap;
    [Header("Inputs")]
    public KeyCode Forward = KeyCode.W;
    public KeyCode Backward = KeyCode.S;
    public KeyCode Up = KeyCode.Space;
    public KeyCode Down = KeyCode.LeftShift;
    public KeyCode Left = KeyCode.A;
    public KeyCode Right = KeyCode.D;
    public KeyCode PitchUp = KeyCode.UpArrow;
    public KeyCode PitchDown = KeyCode.DownArrow;
    public KeyCode YawLeft = KeyCode.Q;
    public KeyCode YawRight = KeyCode.E;
    public KeyCode RollLeft = KeyCode.LeftArrow;
    public KeyCode RollRight = KeyCode.RightArrow;
    public KeyCode RotationReset = KeyCode.R;
    public KeyCode CameraSwap = KeyCode.Tab;
    #endregion

    void Update()
    {
        if (!isMoving)
        {
            if (Input.GetKey(Forward))
            { transform.Translate(Vector3.forward * speed * Time.deltaTime); }
            else if (Input.GetKey(Backward))
            { transform.Translate(Vector3.back * speed * Time.deltaTime); }

            if (Input.GetKey(Left))
            { transform.Translate(Vector3.left * speed * Time.deltaTime); }
            else if (Input.GetKey(Right))
            { transform.Translate(Vector3.right * speed * Time.deltaTime); }

            if (Input.GetKey(Up))
            { transform.Translate(Vector3.up * speed * Time.deltaTime); }
            else if (Input.GetKey(Down))
            { transform.Translate(Vector3.down * speed * Time.deltaTime); }

            if (Input.GetKey(YawRight))
            { transform.Rotate(new Vector3(0f, RotateStep, 0f)); }
            else if (Input.GetKey(YawLeft))
            { transform.Rotate(new Vector3(0f, -RotateStep, 0f)); }

            if (Input.GetKey(PitchUp))
            { transform.Rotate(new Vector3(-RotateStep, 0f, 0f)); }
            else if (Input.GetKey(PitchDown))
            { transform.Rotate(new Vector3(RotateStep, 0f, 0f)); }

            if (Input.GetKey(RollLeft))
            { transform.Rotate(new Vector3(0f, 0f, RotateStep)); }
            else if (Input.GetKey(RollRight))
            { transform.Rotate(new Vector3(0f, 0f, -RotateStep)); }

            if (Input.GetKey(RotationReset))
            { StartCoroutine(ResetRotation()); }
        }
    }

    public void HideSelf()
    {
        StartTurrent.SetActive(true);
        swap.enabled = true;
        swap.selectWeapon();
        Cam.gameObject.SetActive(false);
        transform.gameObject.SetActive(false);
    }

    public void NewTurn()
    {
        isMoving = false;
        StartTurrent.SetActive(false);
        swap.UnFireWeapons();
        swap.enabled = false;
        GhostCam.gameObject.SetActive(true);
    }

    private IEnumerator ResetRotation()
    {
        float timer = 0f;
        while(timer <= 2f)
        {
            transform.localRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), timer / 2f);
            timer += Time.deltaTime;
            yield return null;
        }
        StopCoroutine(ResetRotation());
    }
}
