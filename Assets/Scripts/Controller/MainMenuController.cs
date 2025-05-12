using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void OnPlayClicked()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnSettingsMenuClicked()
    {
        Debug.Log("Ayarlar açılıyor...");
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }
}
