using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    bool yInvert;

    public PlayerController playerController;
    public Animator menuEyes;
    public Slider masterVolume;
    public Slider masterFOV;
    public Slider xsensitivity;
    public Slider ysensitivity;
    public Toggle invertYTog;
    public Camera playerCam;
    public float tempXSens;
    public float tempYSens;
    public float tempVol;
    public float tempFOV;
    public int tempInv;
    public bool invertYVal;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("MenuHandler");
        if (objs.Length > 1)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        string[] temp = Input.GetJoystickNames();

        //Check whether array contains anything
        if (temp.Length > 0)
        {
            //Iterate over every element
            for (int i = 0; i < temp.Length; ++i)
            {
                //Check if the string is empty or not
                if (!string.IsNullOrEmpty(temp[i]))
                {
                    //Not empty, controller temp[i] is connected
                    Debug.Log("Controller " + i + " is connected using: " + temp[i]);
                    Cursor.visible = false;
                }
                else
                {
                    //If it is empty, controller i is disconnected
                    //where i indicates the controller number
                    Debug.Log("Controller: " + i + " is disconnected.");
                    Cursor.visible = true;
                }
            }
        }
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerController.mouseSensX = xsensitivity.value;
        playerController.mouseSensY = ysensitivity.value;
    }

    public void FixedUpdate()
    {
        changeMasterVolume();
        changeFOV();
        changeXSensitivity();
        changeYSensitivity();
    }

    public void PlayGame()
    {
        StartCoroutine(LoadGameAfterDelay("Master Scene", "Wake Up", "Kitchen Missions", "Toilet Mission", "Shower Mission", "Key End", 0.80f));
    }

    public void Options()
    {
        StartCoroutine(LoadSceneAfterDelay("Options", 0.80f));
    }

    public void Credits()
    {
        StartCoroutine(LoadSceneAfterDelay("Credits", 0.80f));
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void LoadMenu()
    {
        StartCoroutine(LoadSceneAfterDelay("Menu", 0.80f));
        SceneManager.UnloadSceneAsync("Master Scene");
        SceneManager.UnloadSceneAsync("Kitchen Missions");
        SceneManager.UnloadSceneAsync("Toilet Mission");
        SceneManager.UnloadSceneAsync("Shower Mission");
        SceneManager.UnloadSceneAsync("Wake Up");
        SceneManager.UnloadSceneAsync("Key End");
    }

    public void Return()
    {
        if(SceneManager.GetSceneByName("Master Scene").isLoaded)
        {
            SceneManager.LoadScene("Pause", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("Options");
        }
        else
        {
            StartCoroutine(LoadSceneAfterDelay("Menu", 0.80f));
        }
    }

    public void SaveOptions()
    {
        bool invertY = GameObject.Find("togInvertY").GetComponent<Toggle>().isOn;


        using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.dataPath + "/options.txt", false))
        {
            if (invertY)
                file.WriteLine("-1");
            else
                file.WriteLine("1");
        }
    }

    public void InvertY()
    {
        Debug.Log("Toggling");
        invertYVal = invertYTog.isOn;
        if (invertYVal && invertYTog != null)
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            playerController.yInvert = +1;
            tempInv = playerController.yInvert;
        }    
        else if (!invertYVal && invertYTog != null)
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            playerController.yInvert = -1;
            tempInv = playerController.yInvert;
        }
        else if (invertYTog = null)
            playerController.yInvert = tempInv;
    }

    public void changeMasterVolume()
    {
        if (masterVolume != null)
        {
            masterVolume = GameObject.FindGameObjectWithTag("MasterVolume").GetComponent<Slider>();
            AudioListener.volume = masterVolume.value;
            tempVol = masterVolume.value;
        }
        else if (masterVolume = null)
            AudioListener.volume = tempVol;            
    }

    public void changeFOV()
    {
        if (masterFOV != null)
        {          
            masterFOV = GameObject.FindGameObjectWithTag("FOV").GetComponent<Slider>();
            playerCam = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
            tempFOV = masterFOV.value;
            playerCam.fieldOfView = masterFOV.value;
        }
        else if (masterFOV = null)
            playerCam.fieldOfView = tempFOV;
    }

    public void changeXSensitivity()
    {
        if (xsensitivity != null)
        {
            xsensitivity = GameObject.FindGameObjectWithTag("Xsens").GetComponent<Slider>();
            playerController.mouseSensX = xsensitivity.value;
            tempXSens = xsensitivity.value;
        }
        else if (xsensitivity = null)
            playerController.mouseSensX = tempVol;
    }

    public void changeYSensitivity()
    {
        if (ysensitivity != null)
        {
            ysensitivity = GameObject.FindGameObjectWithTag("Ysens").GetComponent<Slider>();
            playerController.mouseSensY = ysensitivity.value;
            tempYSens = ysensitivity.value;
        }
        else if (ysensitivity = null)
            playerController.mouseSensY = tempVol;
    }

    public IEnumerator LoadSceneAfterDelay(string sceneName, float seconds)
    {
        menuEyes.Play("MenuClose");
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(sceneName);
    }
    public IEnumerator LoadGameAfterDelay(string sceneName1, string sceneName2, string sceneName3, string sceneName4, string sceneName5, string sceneName6, float seconds)
    {
        menuEyes.Play("MenuClose");
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(sceneName1);
        SceneManager.LoadScene(sceneName2, LoadSceneMode.Additive);
        SceneManager.LoadScene(sceneName3, LoadSceneMode.Additive);
        SceneManager.LoadScene(sceneName4, LoadSceneMode.Additive);
        SceneManager.LoadScene(sceneName5, LoadSceneMode.Additive);
        SceneManager.LoadScene(sceneName6, LoadSceneMode.Additive);
    }
}
