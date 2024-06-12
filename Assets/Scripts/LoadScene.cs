using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void ToCombat()
    {
        SceneManager.LoadScene("DemoCombat");
    }

    public void ReloadScene()
    {
        string SceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(SceneName);
    }

    public void Close()
    {
        Application.Quit();
    }
}
