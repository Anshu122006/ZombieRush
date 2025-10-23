using System.Collections;
using SlimUI.ModernMenu;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
    [SerializeField] private float flickDelay = 3;
    [SerializeField] private float flashIntensity = 0.3f;
    [SerializeField] private float originalIntensity = 0;
    [SerializeField] private float flickerSpeed = 0.03f;
    [SerializeField] private float checkRate = 0.3f;

    public enum Theme { custom1, custom2, custom3 };
    [Header("THEME SETTINGS")]
    public Theme theme;
    private int themeIndex;
    public ThemedUIData themeController;

    [Header("SCREEN")]
    public GameObject mainScreen;
    public GameObject settingsScreen;
    public GameObject selectScreen;

    [Header("COMPONENTS")]
    public GameObject mainMenu;
    public GameObject playMenu;
    public GameObject exitMenu;
    public TextMeshProUGUI hudText;
    public TextMeshProUGUI fullscreen;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Image flashOverlay;

    [Header("PANELS")]
    public GameObject PanelGame;
    public GameObject PanelControls;


    // highlights in settings screen
    [Header("HIGHLIGHTS")]
    public GameObject lineGame;
    public GameObject lineControls;

    [SerializeField] private Vector2 leftmostPos;
    [SerializeField] private Vector2 rightmostPos;

    private Animator anim;
    private Coroutine flickDelayCoroutine;

    private void Start() {
        anim = transform.GetComponent<Animator>();

        playMenu.SetActive(false);
        exitMenu.SetActive(false);
        mainMenu.SetActive(true);
        mainScreen.SetActive(true);

        musicSlider.value = PlayerPrefs.GetFloat(PrefKeys.musicVolume);
        sfxSlider.value = PlayerPrefs.GetFloat(PrefKeys.sfxVolume);

        SetThemeColors();
        InvokeRepeating("RandomFlickEffect", 0.3f, checkRate);
    }

    // Managing the theme
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


    // Button functions
    public void OnPlayClick() {
        if (playMenu.activeSelf) DisablePlayMessage();
        else EnablePlayMessage();
    }

    public void OnExitClick() {
        if (exitMenu.activeSelf) DisableExitMessage();
        else EnableExitMessage();
    }

    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
    }

    public void ShowSettingsScreen() {
        Debug.Log("Button pressed");
        DisablePlayMessage();
        DisableExitMessage();
        anim.CrossFade("ShowSettings", 0);
    }

    public void ShowSelectionScreen() {
        DisablePlayMessage();
        DisableExitMessage();
        anim.CrossFade("ShowSelect", 0);
    }

    public void HideSettingsScreen() {
        anim.CrossFade("HideSettings", 0);
    }

    public void HideSelectionScreen() {
        anim.CrossFade("HideSelect", 0);
    }

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
        fullscreen.text = Screen.fullScreen ? "ON" : "OFF";
    }

    public void ToggleHud() {
        bool showHud = PlayerPrefs.GetInt(PrefKeys.showHud) == 1;
        if (showHud) PlayerPrefs.SetInt(PrefKeys.showHud, 0);
        else PlayerPrefs.SetInt(PrefKeys.showHud, 1);
        showHud = PlayerPrefs.GetInt(PrefKeys.showHud) == 1;
        hudText.text = showHud ? "ON" : "OFF";
    }

    // audio sliders
    public void SetMusicVolume(float value) {
        GameAudioManager.Instance?.SetMusicVolume(value);
    }

    public void SetSFXVolume(float value) {
        GameAudioManager.Instance?.SetSFXVolume(value);
    }


    // Functions to enable and disable play and exit messages
    private void EnablePlayMessage() {
        exitMenu.SetActive(false);
        playMenu.SetActive(true);
    }
    private void DisablePlayMessage() {
        playMenu.SetActive(false);
    }

    private void EnableExitMessage() {
        exitMenu.SetActive(true);
        DisablePlayMessage();
    }
    private void DisableExitMessage() {
        exitMenu.SetActive(false);
    }


    // To disable all panels
    void DisablePanels() {
        PanelControls.SetActive(false);
        PanelGame.SetActive(false);

        lineGame.SetActive(false);
        lineControls.SetActive(false);
    }

    // UI effects

    private void RandomFlickEffect() {
        if (!mainScreen.activeSelf) return;
        if (flickDelayCoroutine == null) {
            StartCoroutine(FlickerOnce());
            flickDelayCoroutine = StartCoroutine(DelayFlick());
        }
    }

    private IEnumerator FlickerOnce() {
        int flickerCount = Random.Range(1, 6);
        float elapsed = 0;
        float curIntensity = 0;
        Color color = flashOverlay.color;

        for (int i = 0; i < flickerCount; i++) {
            float targetIntensity = Random.Range(flashIntensity, flashIntensity * 1.2f);

            elapsed = 0f;
            while (elapsed < flickerSpeed) {
                curIntensity = Mathf.Lerp(originalIntensity, targetIntensity, elapsed / flickerSpeed);
                flashOverlay.color = new Color(color.r, color.g, color.b, curIntensity);
                elapsed += Time.deltaTime * Random.Range(0.6f, 1.4f);
                yield return null;
            }
            flashOverlay.color = new Color(color.r, color.g, color.b, targetIntensity);

            elapsed = 0f;
            while (elapsed < flickerSpeed) {
                curIntensity = Mathf.Lerp(originalIntensity, targetIntensity, elapsed / flickerSpeed);
                flashOverlay.color = new Color(color.r, color.g, color.b, curIntensity);
                elapsed += Time.deltaTime * Random.Range(0.6f, 1.4f);
                yield return null;
            }
            flashOverlay.color = new Color(color.r, color.g, color.b, 0);
            yield return null;
        }
    }

    private IEnumerator DelayFlick() {
        yield return new WaitForSeconds(flickDelay);
        flickDelayCoroutine = null;
    }
}
