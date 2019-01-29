using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RaycastShooting : MonoBehaviour {
    
    public float shootingDistance;
    public Camera playerCam;
    public Animator playerAnim;
    public Animator crosshairAnim;
    public GameObject activateableObjs;
    public bool lookingAt;
    public bool activateCross;

    public GameObject holdingArm;

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
            StartCoroutine(WaitForWakeUp(4.25f));
        }

        if (Physics.Raycast(ray, out hit, shootingDistance, keys))
        {
            Vector3.Lerp(GameObject.Find("EndGameKeys").transform.position, holdingArm.transform.position, Time.deltaTime * 5);
            keysCollected = true;
            GameObject.Find("EndGameDoor").tag = "Activateable";
        }

        if(Physics.Raycast(ray, out hit, shootingDistance, endGameDoor) && keysCollected)
        {            
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<RaycastShooting>().enabled = false;
            playerAnim.Play("End The Game");
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
}
