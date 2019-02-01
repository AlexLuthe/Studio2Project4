﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RaycastShooting : MonoBehaviour {
    
    public float shootingDistance;
    public Camera playerCam;
    public Animator playerAnim;
    public Animator crosshairAnim;
    public Animator endGameFade;
    public GameObject crosshair;
    public GameObject activateableObjs;
    public bool lookingAt;
    public bool activateCross;
    public GameObject blinkEyes;
    public float blinkRate;

    public GameObject holdingArm;
    public float lerpSpeed = 1.5f;

    [Header("Wake Up Mission")]
    public LayerMask alarmClock;
    public GameObject alarmClockObj;
    public bool wokenUp;

    [Header("Keys Mission")]
    public LayerMask keys;
    public LayerMask endGameDoor;
    public bool keysCollected;

    // Use this for initialization
    void Start ()
    {
        playerCam = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        wokenUp = false;
}

    private void FixedUpdate()
    {
        RaycastCheck();
        RaycastingAlways();
        EyesBlinking();
        activateableObjs = GameObject.FindGameObjectWithTag("Activateable");
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
        //Debug.Log("Trying to Hit");
        float x = Screen.width / 2;
        float y = Screen.height / 2;

        Ray ray = playerCam.ScreenPointToRay(new Vector2(x, y));
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shootingDistance, alarmClock))
        {
            //Debug.Log("Hit Alarm");
            playerAnim.Play("Get Out Of Bed");
            GameObject.FindGameObjectWithTag("AlarmSound").GetComponent<AudioSource>().enabled = false;
            GameObject.FindGameObjectWithTag("AlarmText").GetComponent<Animator>().enabled = false;
            GameObject.FindGameObjectWithTag("AlarmText").GetComponent<Text>().enabled = true;
            GameObject.Find("Alarm Clock").tag = "Untagged";
            StartCoroutine(WaitForWakeUp(4.5f));
        }

        if (Physics.Raycast(ray, out hit, shootingDistance, keys))
        {
            GameObject endGameKeys = GameObject.Find("EndGameKeys");
            endGameKeys.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(LerpToHand(endGameKeys));
            keysCollected = true;
            GameObject.Find("EndGameDoor").tag = "Activateable";
            endGameKeys.tag = "Untagged";
        }

        if(Physics.Raycast(ray, out hit, shootingDistance, endGameDoor) && keysCollected)
        {
            crosshair.SetActive(false);
            endGameFade.Play("FadeToWhite");
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<RaycastShooting>().enabled = false;
            Destroy(GameObject.Find("EndGameKeys"));
            Debug.Log("FadingOut");
            StartCoroutine(LoadSceneAfterDelay("Menu", 15.0f));            
        }
    }

    public void RaycastingAlways()
    {
        //Debug.Log("Trying to Hit");
        float x = Screen.width / 2;
        float y = Screen.height / 2;

        Ray ray = playerCam.ScreenPointToRay(new Vector2(x, y));
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shootingDistance) && hit.transform.gameObject.tag == "Activateable")
        {
            StartCoroutine(WaitForCrosshair(0.2f));
        }
        else if (activateCross)
        {
            StartCoroutine(WaitForCrosshairDeact(0.2f));
        }
    }

    public IEnumerator LerpToHand(GameObject fromPoint)
    {
        while(fromPoint.transform.localPosition != holdingArm.transform.localPosition)
        {
            fromPoint.transform.parent = holdingArm.transform;
            fromPoint.transform.localPosition = Vector3.Lerp(fromPoint.transform.localPosition, holdingArm.transform.localPosition, lerpSpeed * Time.deltaTime);
            fromPoint.transform.localRotation = Quaternion.Lerp(fromPoint.transform.localRotation, holdingArm.transform.localRotation, lerpSpeed * Time.deltaTime);
            Debug.Log("Lerping");
            yield return null;
        }       
    }
    
    IEnumerator WaitForCrosshair(float time)
    {
        crosshairAnim.Play("Activateable");
        yield return new WaitForSeconds(time);
        activateCross = true;
    }
    IEnumerator WaitForCrosshairDeact(float time)
    {
        crosshairAnim.Play("Deactivateable");
        yield return new WaitForSeconds(time);
        activateCross = false;
    }

    IEnumerator WaitForWakeUp(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<Animator>().enabled = false;
        wokenUp = true;
    }

    public IEnumerator LoadSceneAfterDelay(string sceneName, float seconds)
    {        
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(sceneName);
    }

    public void EyesBlinking()
    {
        float randomNum = Random.Range(0.0f, 100.0f);
        if (randomNum <= blinkRate && wokenUp)
        {
            Debug.Log("Below 0.10f");
            blinkEyes.GetComponent<Animator>().Play("Blink");
        }
    }
}
