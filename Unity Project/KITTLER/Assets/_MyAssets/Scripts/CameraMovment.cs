using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovment : MonoBehaviour {

    public float mouseSensitivity = 5.0f;        // Mouse rotation sensitivity.
    private float rotationY = 0.0f;

    CharacterController ch;
    public float speed = 5;
    public float currentSpeed = 5f;
    bool speedIncrease;

    public GameObject rectile;

    void Start()
    {
        ch = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update () {
        MovmentAndRotation();
        if (Input.GetMouseButtonDown(1))
        {
            rectile.SetActive(!rectile.activeSelf);
        }
    }

    void MovmentAndRotation()
    {
        speedIncrease = Input.GetKey(KeyCode.LeftShift);

        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY += Input.GetAxis("Mouse Y") * mouseSensitivity;
        //rotationY = Mathf.Clamp(rotationY, -90, 90);
        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0.0f);

        currentSpeed = (speedIncrease) ? speed * 3 : speed;
        float z = Input.GetAxisRaw("Vertical");
        if (z != 0)
        {
            //Vector3 newPos = Vector3.Scale(transform.forward, new Vector3(1f, 0, 1f));
            ch.Move(transform.forward * z * currentSpeed * Time.deltaTime);
        }
        float h = Input.GetAxisRaw("Horizontal");
        if (h != 0)
        {
            //Vector3 newPos = Vector3.Scale(transform.right, new Vector3(1f, 0, 1f));
            ch.Move(transform.right * h * currentSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E)) //up
        {
            ch.Move(transform.up * currentSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            ch.Move(-transform.up * currentSpeed * Time.deltaTime);
        }



          
    }
}
