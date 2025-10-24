using System;
using System.Collections;
using SlimUI.ModernMenu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MainMenuManager;

public class GameMenuManager : MonoBehaviour {
    public static GameMenuManager Instance;

    [Header("THEME SETTINGS")]
    public Theme theme;
    private int themeIndex;
    public ThemedUIData themeController;
    [SerializeField] private float screenFadeTime = 0.7f;

    [Header("COMPONENTS")]
    public GameObject exitMessage;
    public GameObject playerStats;
    public TextMeshProUGUI fullscreenText;
    public TextMeshProUGUI hudText;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Image fadeOverlay;

    [Header("SCREEN")]
    public GameObject hud;
    public GameObject settings;

    [Header("PANELS")]
    public GameObject PanelGame;
    public GameObject PanelControls;

    [Header("HIGHLIGHTS")]
    public GameObject lineGame;
    public GameObject lineControls;

    private GameInputManager input;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        input = GameInputManager.Instance;
        SetThemeColors();

        settings.SetActive(false);
        hud.SetActive(true);
        bool showHud = PlayerPrefs.GetInt(PrefKeys.showHud) == 1;
        if (showHud) playerStats.SetActive(true);
        else playerStats.SetActive(false);

        musicSlider.value = PlayerPrefs.GetFloat(PrefKeys.musicVolume);
        sfxSlider.value = PlayerPrefs.GetFloat(PrefKeys.sfxVolume);
        fullscreenText.text = Screen.fullScreen ? "ON" : "OFF";
        hudText.text = PlayerPrefs.GetInt(PrefKeys.showHud) == 1 ? "ON" : "OFF";

        input.OnPausePerformed += OnPausePressed;
    }

    private void SetThemeColors() {
        switch (theme) {
            case Theme.custom1:
                themeController.currentColor = themeController.custom1.graphic1;
                themeController.textColor = themeController.custom1.text1;
                themeIndex = 0;
                break;
            case Theme.custom2:
                themeController.currentColor = themeController.custom2.graphic2;
                themeController.textColor = themeController.custom2.text2;
                themeIndex = 1;
                break;
            case Theme.custom3:
                themeController.currentColor = themeController.custom3.graphic3;
                themeController.textColor = themeController.custom3.text3;
                themeIndex = 2;
                break;
            default:
                Debug.Log("Invalid theme selected.");
                break;
        }
    }

    // Butoon functions

    public void ShowGamePanel() {
        DisablePanels();
        PanelGame.SetActive(true);
        lineGame.SetActive(true);
    }

    public void ShowControlsPanel() {
        DisablePanels();
        PanelControls.SetActive(true);
        lineControls.SetActive(true);
    }

    public void ToggleFullscreen() {
        Screen.fullScreen = !Screen.fullScreen;
        PlayerPrefs.SetInt(PrefKeys.fullscreen, Screen.fullScreen ? 1 : 0);
        Time.timeScale = 1;
        fullscreenText.text = Screen.fullScreen ? "ON" : "OFF";
        Time.timeScale = 0;
    }

    public void ToggleHud() {
        bool showHud = PlayerPrefs.GetInt(PrefKeys.showHud) == 1;
        if (showHud) PlayerPrefs.SetInt(PrefKeys.showHud, 0);
        else PlayerPrefs.SetInt(PrefKeys.showHud, 1);
        showHud = !showHud;
        Debug.Log(showHud);
        hudText.text = showHud ? "ON" : "OFF";
    }

    public void PauseGame() {
        hud.SetActive(false);
        settings.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame() {
        bool showHud = PlayerPrefs.GetInt(PrefKeys.showHud) == 1;
        Time.timeScale = 1;
        settings.SetActive(false);
        hud.SetActive(true);
        DisableExitMessage();

        if (showHud) playerStats.SetActive(true);
        else playerStats.SetActive(false);
    }

    public void OnExitClick() {
        if (!exitMessage.activeSelf) EnableExitMessage();
        else DisableExitMessage();
    }

    public void ReturnToMainMenu() {
        Time.timeScale = 1;
        Loader.LoadScene(SceneName.MainMenu.GetString());
    }

    public void SetMusicVolume(float value) {
        GameAudioManager.Instance?.SetMusicVolume(value);
    }

    public void SetSFXVolume(float value) {
        GameAudioManager.Instance?.SetSFXVolume(value);
    }


    private void OnPausePressed(System.Object sender, EventArgs e) {
        if (settings.activeSelf) ResumeGame();
        else PauseGame();
    }

    private void EnableExitMessage() {
        exitMessage.SetActive(true);
    }
    private void DisableExitMessage() {
        exitMessage.SetActive(false);
    }

    private void DisablePanels() {
        PanelControls.SetActive(false);
        PanelGame.SetActive(false);

        lineGame.SetActive(false);
        lineControls.SetActive(false);
    }

    public void OnGameOver() {
        StartCoroutine(FadeScreen());
    }

    private IEnumerator FadeScreen() {
        fadeOverlay.gameObject.SetActive(true);
        Color color = fadeOverlay.color;
        float elapsed = 0;
        float curFade = 0;

        fadeOverlay.color = new Color(color.r, color.g, color.b, 0);
        while (elapsed <= screenFadeTime) {
            elapsed += Time.deltaTime;
            curFade = Mathf.Lerp(0, 1, elapsed / screenFadeTime);
            fadeOverlay.color = new Color(color.r, color.g, color.b, curFade);
            yield return null;
        }

        fadeOverlay.color = new Color(color.r, color.g, color.b, 1);
        Loader.LoadSceneSilently(SceneName.GameOver.GetString());
    }
}
