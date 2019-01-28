using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    int yInvert;

    public Animator menuEyes;
    public Slider masterVolume;
    public Slider masterFOV;
    public Camera playerCam;
    public float tempVol;
    public float tempFOV;

    private void Awake()
    {
        ReadIn();
        Cursor.visible = false;
    }

    public void FixedUpdate()
    {
        changeMasterVolume();
        changeFOV();
    }

    public void ReadIn ()
    {
        if (SceneManager.GetActiveScene().name == "Options")
        {
            using (System.IO.StreamReader file = new System.IO.StreamReader(Application.dataPath + "/options.txt"))
            {
                yInvert = int.Parse(file.ReadLine());
            }
            if (yInvert == -1)
                GameObject.Find("togInvertY").GetComponent<Toggle>().isOn = true;
            else if (yInvert == 1)
                GameObject.Find("togInvertY").GetComponent<Toggle>().isOn = false;

            Debug.Log("I am running");
            GameObject.Find("togInvertY").GetComponent<Toggle>().isOn = true;
        }
    }

    public void PlayGame()
    {
        StartCoroutine(LoadGameAfterDelay("Master Scene", "Wake Up", "Cereal Mission", "Coffee Mission", "Toilet Mission", "Shower Mission", "Key End", 0.80f));
    }

    public void Options()
    {
        StartCoroutine(LoadSceneAfterDelay("Options", 0.80f));
        if (SceneManager.GetActiveScene().name == "Pause")
        {
            SceneManager.UnloadSceneAsync("Pause");
        }
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            SceneManager.UnloadSceneAsync("Menu");
        }
        ReadIn();
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
        SceneManager.UnloadSceneAsync("Cereal Mission");
        SceneManager.UnloadSceneAsync("Coffee Mission");
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
            playerCam.fieldOfView = masterFOV.value;
            tempFOV = masterFOV.value;
        }
        else if (masterFOV = null)
            playerCam.fieldOfView = tempFOV;
    }

    public IEnumerator LoadSceneAfterDelay(string sceneName, float seconds)
    {
        menuEyes.Play("MenuClose");
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(sceneName);
    }
    public IEnumerator LoadGameAfterDelay(string sceneName1, string sceneName2, string sceneName3, string sceneName4, string sceneName5, string sceneName6, string sceneName7, float seconds)
    {
        menuEyes.Play("MenuClose");
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(sceneName1);
        SceneManager.LoadScene(sceneName2, LoadSceneMode.Additive);
        SceneManager.LoadScene(sceneName3, LoadSceneMode.Additive);
        SceneManager.LoadScene(sceneName4, LoadSceneMode.Additive);
        SceneManager.LoadScene(sceneName5, LoadSceneMode.Additive);
        SceneManager.LoadScene(sceneName6, LoadSceneMode.Additive);
        SceneManager.LoadScene(sceneName7, LoadSceneMode.Additive);
    }
}
