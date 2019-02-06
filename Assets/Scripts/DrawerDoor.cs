using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerDoor: MonoBehaviour {

    PlayerController playerController;
    RaycastShooting raycastShooting;
    Camera playerCam;

    public LayerMask door;
    public Animator doorMoving;
    public bool doorOpen;
    public bool doorWait = true;
    public float doorTime = 2f;

    void Start()
    {
        raycastShooting = GameObject.FindGameObjectWithTag("Player").GetComponent<RaycastShooting>();
        playerCam = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();

        doorOpen = true;
    }

    private void FixedUpdate()
    {
        RaycastCheck();
    }

    public void RaycastCheck()
    {
        if (Input.GetButtonDown("Fire1") && doorWait)
        {            
            Raycasting();
        }
    }

    public void Raycasting()
    {
        float x = Screen.width / 2;
        float y = Screen.height / 2;

        Ray ray = playerCam.ScreenPointToRay(new Vector2(x, y));
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastShooting.shootingDistance, door))
        {
            if (doorOpen)
                StartCoroutine(WaitForDoorOpen(doorTime));

            else if (!doorOpen)
                StartCoroutine(WaitForDoorClose(doorTime));
        }
    }

    IEnumerator WaitForDoorOpen(float time)
    {
        doorMoving.Play("OpenDoor");
        doorWait = false;
        this.gameObject.tag = "Untagged";
        yield return new WaitForSeconds(time);
        doorOpen = false;
        doorWait = true;
        this.gameObject.tag = "Activateable";
    }
    IEnumerator WaitForDoorClose(float time)
    {
        doorMoving.Play("CloseDoor");
        doorWait = false;
        this.gameObject.tag = "Untagged";
        yield return new WaitForSeconds(time);
        doorOpen = true;
        doorWait = true;
        this.gameObject.tag = "Activateable";
    }
}
