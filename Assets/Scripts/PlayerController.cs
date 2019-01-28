﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float maxMoveSpeed;
    float moveSpeedX;
    float moveSpeedZ;
    Vector3 velocity;
    float gravity = -20;

    public GameObject myCam;
    public float mouseSensX;
    public float mouseSensY;
    private Vector3 rotateValueX;
    private Vector3 rotateValueY;

    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
        GetComponent<Animator>().Play("Open Eyes");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        CameraRotation();
    }

    public void Movement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0)
        {
            gameObject.GetComponent<Rigidbody>().velocity = transform.forward * moveZ * maxMoveSpeed;
            gameObject.GetComponent<Rigidbody>().velocity += transform.right * moveX * maxMoveSpeed;
        }
    }

    public void Jumping()
    {

    }

    public void CameraRotation()
    {
        float mouseSpeedX = Input.GetAxis("Mouse Y") * mouseSensY;
        float mouseSpeedY = Input.GetAxis("Mouse X") * mouseSensX;
        rotateValueX = new Vector3(mouseSpeedX * -1, 0, 0);
        myCam.transform.eulerAngles = myCam.transform.eulerAngles + rotateValueX;
        rotateValueY = new Vector3(0, mouseSpeedY * +1, 0);
        transform.eulerAngles = transform.eulerAngles + rotateValueY;
    }
}
