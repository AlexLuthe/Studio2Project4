using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

    bool paused = false;
    public GameObject pauseMenu;
    public GameObject fpsController;
    public GameObject camera;
    public GameObject colourCam;
    public GameObject wakeUp;

	// Use this for initialization
	void Awake () {
        fpsController.GetComponent<Animation>().Play();
        Animation[] eyelids = wakeUp.GetComponentsInChildren<Animation>();
        for(int index = 0; index < eyelids.Length; ++index)
            eyelids[index].Play();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Start")) {// || Input.GetKeyDown(KeyCode.P)) {
            if (paused) {
                Unpause();
            }
            else {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                fpsController.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                fpsController.GetComponent<Animation>().enabled = false;
                fpsController.SetActive(false);
                camera.transform.parent = null;
                paused = true;
            }
        }
	}

    public void Unpause() {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        fpsController.SetActive(true);
        fpsController.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.LoadOptions();
        camera.transform.parent = fpsController.transform;
        camera.transform.position = colourCam.transform.position;
        paused = false;
    }
}
