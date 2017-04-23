using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

    bool paused = false;
    public GameObject pauseMenu;
    public GameObject fpsController;
    public GameObject camera;
    public GameObject wakeUp;

	// Use this for initialization
	void Start () {
        fpsController.GetComponent<Animation>().Play();
        Animation[] eyelids = wakeUp.GetComponentsInChildren<Animation>();
        for(int index = 0; index < eyelids.Length; ++index)
            eyelids[index].Play();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) {
            if (paused) {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                fpsController.SetActive(true);
                camera.transform.parent = fpsController.transform;
                camera.transform.position = fpsController.transform.position;
                paused = false;
            }
            else {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                fpsController.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
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
        camera.transform.parent = fpsController.transform;
        camera.transform.position = fpsController.transform.position;
        paused = false;
    }
}
