using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastShooting : MonoBehaviour {
    
    public float shootingDistance;
    public Camera playerCam;
    public Animator playerAnim;

    [Header("Wake Up Mission")]
    public LayerMask alarmClock;

    // Use this for initialization
    void Start ()
    {
        playerCam = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        RaycastCheck();
    }

    public void RaycastCheck()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Raycasting();
        }
    }

    public void Raycasting()
    {
        Debug.Log("Trying to Hit");
        float x = Screen.width / 2;
        float y = Screen.height / 2;

        Ray ray = playerCam.ScreenPointToRay(new Vector2(x, y));
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shootingDistance, alarmClock))
        {
            Debug.Log("Hit Alarm");
            playerAnim.Play("Get Out Of Bed");
            GameObject.FindGameObjectWithTag("AlarmSound").GetComponent<AudioSource>().enabled = false;
            GameObject.FindGameObjectWithTag("AlarmText").GetComponent<Animator>().enabled = false;
            GameObject.FindGameObjectWithTag("AlarmText").GetComponent<Text>().enabled = true;
        }
    }
}
