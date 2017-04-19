using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadScene("Master Scene");
        SceneManager.LoadScene("Cereal Mission", LoadSceneMode.Additive);
        SceneManager.LoadScene("Coffee Mission", LoadSceneMode.Additive);
        SceneManager.LoadScene("Toilet Mission", LoadSceneMode.Additive);
        SceneManager.LoadScene("Shower Mission", LoadSceneMode.Additive);
        SceneManager.LoadScene("Wake Up", LoadSceneMode.Additive);
        SceneManager.LoadScene("End Game", LoadSceneMode.Additive);
    }

    public void Options()
    {
        SceneManager.LoadScene("Options");
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
    }

    public void SaveOptions()
    {
        bool invertY = GameObject.Find("togInvertY").GetComponent<Toggle>().isOn;


        using (System.IO.StreamWriter file = new System.IO.StreamWriter("Assets/options.txt", false))
        {
            if (invertY)
                file.WriteLine("1");
            else
                file.WriteLine("0");
        }
    }
}
