using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

    bool paused = false;
    public GameObject pauseMenu;
    public GameObject fpsController;
    public GameObject cam;
    public GameObject colourCam;
    public GameObject wakeUp;

	// Use this for initialization
	void Awake () {
        PlayAnimation();
    }

    void PlayAnimation ()
    {
        if (!fpsController.GetComponent<Animation>().isPlaying)
        {
            fpsController.GetComponent<Animation>().Play();
            Animation[] eyelids = wakeUp.GetComponentsInChildren<Animation>();
            for (int index = 0; index < eyelids.Length; ++index)
                eyelids[index].Play();
        }
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
                fpsController.SetActive(false);
                cam.transform.parent = null;
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
        cam.transform.parent = fpsController.transform;
        cam.transform.position = colourCam.transform.position;
        paused = false;
        PlayAnimation();
    }
}
