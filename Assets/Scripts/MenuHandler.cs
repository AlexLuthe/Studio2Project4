using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    int yInvert;

    private void Awake()
    {
        ReadIn();
    }

    void ReadIn ()
    {
        if (SceneManager.GetActiveScene().name == "Options")
        {
            using (System.IO.StreamReader file = new System.IO.StreamReader("Assets/options.txt"))
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
        SceneManager.LoadScene("Master Scene");
        SceneManager.LoadScene("Cereal Mission", LoadSceneMode.Additive);
        SceneManager.LoadScene("Coffee Mission", LoadSceneMode.Additive);
        SceneManager.LoadScene("Toilet Mission", LoadSceneMode.Additive);
        SceneManager.LoadScene("Shower Mission", LoadSceneMode.Additive);
        SceneManager.LoadScene("Wake Up", LoadSceneMode.Additive);
        SceneManager.LoadScene("Key End", LoadSceneMode.Additive);
    }

    public void Options()
    {
        SceneManager.LoadScene("Options", LoadSceneMode.Additive);
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
        SceneManager.LoadScene("Credits");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
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
            SceneManager.LoadScene("Menu");
        }
    }

    public void SaveOptions()
    {
        bool invertY = GameObject.Find("togInvertY").GetComponent<Toggle>().isOn;


        using (System.IO.StreamWriter file = new System.IO.StreamWriter("Assets/options.txt", false))
        {
            if (invertY)
                file.WriteLine("-1");
            else
                file.WriteLine("1");
        }
    }
}
