﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeListener: MonoBehaviour {

    PlayerController playerController;
    RaycastShooting raycastShooting;
    Camera playerCam;

    public GameObject slidingDoor;
    public GameObject showerWater;
    public GameObject washSelf;
    public GameObject drySelf;
    public GameObject putOnClothes;
    public bool showerWaterOn;

    public LayerMask door;
    public Animator doorMoving;
    public bool doorOpen;
    public bool doorWait = true;
    public float doorTime = 2f;

    void Start()
    {
        raycastShooting = GameObject.FindGameObjectWithTag("Player").GetComponent<RaycastShooting>();
        playerCam = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
        doorMoving = this.gameObject.GetComponent<Animator>();
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
        doorMoving.Play("OpenShower");
        doorWait = false;
        slidingDoor.gameObject.tag = "Untagged";
        yield return new WaitForSeconds(time);
        doorOpen = false;
        doorWait = true;
        slidingDoor.gameObject.tag = "Activateable";
    }
    IEnumerator WaitForDoorClose(float time)
    {
        doorMoving.Play("CloseShower");
        doorWait = false;
        slidingDoor.gameObject.tag = "Untagged";
        yield return new WaitForSeconds(time);
        doorOpen = true;
        doorWait = true;
        slidingDoor.gameObject.tag = "Activateable";
    }
}
