using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float maxMoveSpeed;
    float moveSpeedX;
    float moveSpeedZ;
    Vector3 velocity;
    float gravity = -20;
    RaycastShooting raycastShooting;
    Pause pause;

    public GameObject myCam;
    public float mouseSensX;
    public float mouseSensY;
    float mouseSpeedX;
    float mouseSpeedY;
    private Vector3 rotateValueX;
    private Vector3 rotateValueY;
    public float viewRange;
    public int yInvert = -1;
    private float rotY = 0.0f;
    private float rotX = 0.0f;
    Quaternion localCamRotation;
    Quaternion localPlayRotation;

    // Use this for initialization
    void Start()
    {
        raycastShooting = this.gameObject.GetComponent<RaycastShooting>();
        pause = this.gameObject.GetComponent<Pause>();
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
        if (!pause.paused)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0)
            {
                gameObject.GetComponent<Rigidbody>().velocity = transform.forward * moveZ * maxMoveSpeed;
                gameObject.GetComponent<Rigidbody>().velocity += transform.right * moveX * maxMoveSpeed;
            }
        }
    }

    public void CameraRotation()
    {
        if (raycastShooting.wokenUp && !pause.paused)
        {
            float mouseX = Input.GetAxis("Mouse X") + Input.GetAxis("JoystickRightX");
            float mouseY = Input.GetAxis("Mouse Y") + Input.GetAxis("JoystickRightY");

            rotY += mouseX * mouseSensX;
            rotX += (mouseY * yInvert) * mouseSensY;

            rotX = Mathf.Clamp(rotX, -viewRange, viewRange);

            localCamRotation = Quaternion.Euler(rotX, 0.0f, 0.0f);
            localPlayRotation = Quaternion.Euler(0.0f, rotY, 0.0f);
            myCam.transform.localRotation = localCamRotation;
            transform.localRotation = localPlayRotation;
        }        
    }
}
