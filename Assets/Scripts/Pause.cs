using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour {

    public bool paused = false;
    public bool pausedOnce = false;
    public GameObject pauseMenu;
    public PlayerController playerController;
    public RaycastShooting raycastShooting;
    public GameObject wakeUp;
    public Animator menuEyes;
    public EventSystem myEventSystem;

    public void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        raycastShooting = GameObject.FindGameObjectWithTag("Player").GetComponent<RaycastShooting>();
    }

    public void Update()
    {        

        if (Input.GetButtonDown("Start") && playerController.GetComponent<RaycastShooting>().wokenUp)
        {
            paused = !paused;
            if (paused)
            {
                //Time.timeScale = 0;
                StartCoroutine("highlightBtn");
                playerController.enabled = false;
                raycastShooting.enabled = false;
                pauseMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pausedOnce = true;
            }

        }
    }

    public void ResetHighlightButton()
    {
        StartCoroutine("highlightBtn");
    }

    IEnumerator highlightBtn()
    {
        myEventSystem.SetSelectedGameObject(null);
        yield return null;
        myEventSystem.SetSelectedGameObject(myEventSystem.firstSelectedGameObject);
    }

    public void FixedUpdate()
    {
        Unpaused();
    }

    public void Unpaused()
    {
        if (!paused && pausedOnce)
        {
            //Time.timeScale = 1;
            playerController.enabled = true;
            raycastShooting.enabled = true;
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ReturntoGame()
    {
            //Time.timeScale = 1;
            playerController.enabled = true;
            raycastShooting.enabled = true;
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        paused = false;
    }

    public void loadMainMenu()
    {
        StartCoroutine(LoadSceneAfterDelay("Menu", 0.80f));
    }
    public IEnumerator LoadSceneAfterDelay(string sceneName, float seconds)
    {
        menuEyes.Play("MenuClose");
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(sceneName);
    }

    public void QuitToDesktop()
    {
        Application.Quit();
    }

}
