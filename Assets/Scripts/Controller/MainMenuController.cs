using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _settingsPanel;

    private void Start()
    {
        ShowMainMenu();    
    }

    public void ShowMainMenu()
    {
        _mainMenu.SetActive(true);
        _settingsPanel.SetActive(false);
    }

    public void ShowSettingsPanel()
    {
        _mainMenu.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    public void OnPlayClicked()
    {
        AudioManager.Instance.PlayButtonSFX();
        SceneManager.LoadScene("Game");
    }

    public void OnSettingsMenuClicked()
    {
        AudioManager.Instance.PlayButtonSFX();
        Debug.Log("Ayarlar açılıyor...");
    }

    public void OnBackFromSettingsClicked()
    {
        AudioManager.Instance.PlayButtonSFX();
        ShowMainMenu();
    }

    public void OnApplicationQuit()
    {
        AudioManager.Instance.PlayButtonSFX();
        Application.Quit();
    }
}
