using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletListener: MonoBehaviour {

    PlayerController playerController;
    RaycastShooting raycastShooting;
    Camera playerCam;

    public GameObject lidDoor;
    public GameObject toiletPee;
    public GameObject toiletFlush;
    public GameObject toiletWipe;
    public GameObject toiletSoap;
    public GameObject toiletWash;
    public GameObject washWater;
    public ParticleSystem handSoap;
    public AudioSource soapSound;
    public ParticleSystem flushWater;
    public AudioSource flushSound;

    public LayerMask door;
    public Animator doorMoving;
    public bool doorOpen;
    public bool doorWait = true;
    public bool toiletLidOpen = false;
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
        doorMoving.Play("OpenToilet");
        doorWait = false;
        lidDoor.gameObject.tag = "Untagged";
        yield return new WaitForSeconds(time);
        toiletLidOpen = true;
        doorOpen = false;
        doorWait = true;
        lidDoor.gameObject.tag = "Activateable";
    }
    IEnumerator WaitForDoorClose(float time)
    {
        doorMoving.Play("CloseToilet");
        doorWait = false;
        lidDoor.gameObject.tag = "Untagged";
        yield return new WaitForSeconds(time);
        toiletLidOpen = false;
        doorOpen = true;
        doorWait = true;
        lidDoor.gameObject.tag = "Activateable";
    }

    public void ActiveWater(float time)
    {
        StartCoroutine(TurnWaterOn(time));
    }

    IEnumerator TurnWaterOn(float time)
    {
        washWater.SetActive(true);
        yield return new WaitForSeconds(time);
        washWater.SetActive(false);
    }
}
