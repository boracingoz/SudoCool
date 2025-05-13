using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music Sources")]
    public AudioSource mainMenuMusic;
    public AudioSource gameMusic;

    [Header("SFX Sources")]
    public AudioSource buttonSFX;
    // İleride efekt sesleri için ek kaynaklar:
    // public AudioSource cellSelectSFX;
    // public AudioSource cellErrorSFX;
    // public AudioSource cellCorrectSFX;
    // public AudioSource winSFX;
    // public AudioSource loseSFX;
    // public AudioSource timerTickSFX;

    [Header("UI Controls")]
    public Toggle musicToggle;
    public Slider musicSlider;
    // İleride efekt sesleri için volume kontrolleri:
    // public Toggle effectsToggle;
    // public Slider effectsSlider;

    // PlayerPrefs anahtarları
    private const string PREF_MUSIC_ON = "IsMusicOn";
    private const string PREF_MUSIC_VOL = "MusicVolume";
    // private const string PREF_EFFECTS_ON  = "AreEffectsOn";
    // private const string PREF_EFFECTS_VOL = "EffectsVolume";

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        LoadSettings();
        if (musicToggle != null)
        {
            musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        }

        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(OnSliderChanged);
        }
        HookupUIEvents();
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    #region Settings Load & UI

    private void LoadSettings()
    {
        bool isMusicOn = PlayerPrefs.GetInt("IsMusicOn", 1) == 1;
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        ApplyVolumeSettings(isMusicOn, musicVolume);

        if (musicToggle != null)
            musicToggle.isOn = isMusicOn;

        if (musicSlider != null)
            musicSlider.value = musicVolume;

        // Eğer efekt ayarları eklenirse:
        // bool fxOn = PlayerPrefs.GetInt(PREF_EFFECTS_ON, 1) == 1;
        // float fxVol = PlayerPrefs.GetFloat(PREF_EFFECTS_VOL, 1f);
        // if (effectsToggle != null) effectsToggle.isOn = fxOn;
        // if (effectsSlider != null) effectsSlider.value = fxVol;
    }

    private void HookupUIEvents()
    {
        if (musicToggle != null) musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        if (musicSlider != null) musicSlider.onValueChanged.AddListener(OnSliderChanged);
        // if (effectsToggle != null) effectsToggle.onValueChanged.AddListener(OnEffectsToggleChanged);
        // if (effectsSlider != null) effectsSlider.onValueChanged.AddListener(OnEffectsSliderChanged);
    }

    #endregion
    #region Music Control

    private void PlayMusicForScene(string sceneName)
    {
        if (sceneName == "MainMenu")
        {
            gameMusic?.Stop();
            if (mainMenuMusic != null && !mainMenuMusic.isPlaying)
                mainMenuMusic.Play();
        }
        else if (sceneName == "Game")
        {
            mainMenuMusic?.Stop();
            if (gameMusic != null && !gameMusic.isPlaying)
                gameMusic.Play();
        }
        else
        {
            // Başka sahneler varsa gelecekte buraya ekleyebiliriz
        }
    }

    public void OnMusicToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("IsMusicOn", isOn ? 1 : 0);
        ApplyVolumeSettings(isOn, musicSlider.value);
    }

    public void OnSliderChanged(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        ApplyVolumeSettings(musicToggle.isOn, volume);
    }

    private void ApplyVolumeSettings(bool musicOn, float musicVol)
    {
        float finalMusic = musicOn ? musicVol : 0f;
        if (mainMenuMusic != null) mainMenuMusic.volume = finalMusic;
        if (gameMusic != null) gameMusic.volume = finalMusic;
        if (buttonSFX != null) buttonSFX.volume = musicOn ? 1f : 0f;
        // Efekt sesleri için:
        // float finalFx = effectsToggle.isOn ? effectsSlider.value : 0f;
        // if (cellSelectSFX  != null) cellSelectSFX.volume  = finalFx;
        // if (cellErrorSFX   != null) cellErrorSFX.volume   = finalFx;
        // if (cellCorrectSFX != null) cellCorrectSFX.volume = finalFx;
        // if (winSFX         != null) winSFX.volume         = finalFx;
        // if (loseSFX        != null) loseSFX.volume        = finalFx;
        // if (timerTickSFX   != null) timerTickSFX.volume   = finalFx;
    }

    #endregion

    #region Play One-Shot SFX

    public void PlayButtonSFX()
    {
        if (buttonSFX != null && buttonSFX.volume > 0f)
            buttonSFX.Play();
    }

    // Gelecekte kullanılacak efekt metotları:
    /*
    public void PlayCellSelect()
    {
        if (cellSelectSFX != null && cellSelectSFX.volume > 0f)
            cellSelectSFX.Play();
    }

    public void PlayCellError()
    {
        if (cellErrorSFX != null && cellErrorSFX.volume > 0f)
            cellErrorSFX.Play();
    }

    public void PlayCellCorrect()
    {
        if (cellCorrectSFX != null && cellCorrectSFX.volume > 0f)
            cellCorrectSFX.Play();
    }

    public void PlayWin()
    {
        if (winSFX != null && winSFX.volume > 0f)
            winSFX.Play();
    }

    public void PlayLose()
    {
        if (loseSFX != null && loseSFX.volume > 0f)
            loseSFX.Play();
    }

    public void PlayTimerTick()
    {
        if (timerTickSFX != null && timerTickSFX.volume > 0f)
            timerTickSFX.Play();
    }
    */

    #endregion
}
