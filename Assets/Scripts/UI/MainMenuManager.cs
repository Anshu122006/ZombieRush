using System.Collections;
using SlimUI.ModernMenu;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
    private Animator anim;

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

    [Header("PANELS")]
    public GameObject PanelGame;
    public GameObject PanelControls;


    // highlights in settings screen
    [Header("HIGHLIGHTS")]
    public GameObject lineGame;
    public GameObject lineControls;

    [SerializeField] private Vector2 leftmostPos;
    [SerializeField] private Vector2 rightmostPos;

    private void Start() {
        anim = transform.GetComponent<Animator>();

        playMenu.SetActive(false);
        exitMenu.SetActive(false);
        mainMenu.SetActive(true);
        mainScreen.SetActive(true);

        SetThemeColors();
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

    public void TestLoading() {
        exitMenu.SetActive(false);
        playMenu.SetActive(false);
        Loader.LoadScene("ChuckRendering",true);
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

    // audio slider


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


    // public void OnOptionsSlideIn() { }// OnSlideOver(leftmostPos, Vector2.zero, rightmostPos);
    // public void OnOptionsSlideOut() => OnSlideOver(Vector2.zero, rightmostPos, rightmostPos);
    // public void OnMapSlideIn() => OnSlideOver(leftmostPos, rightmostPos, Vector2.zero);
    // public void OnMapSlideOut() => OnSlideOver(Vector2.zero, rightmostPos, rightmostPos);


    // private void OnSlideOver(Vector2 mainCanvasPos, Vector2 settingsScreenPos, Vector2 mapCanvasPos) {
    //     RectTransform mainRect = mainScreen.GetComponent<RectTransform>();
    //     RectTransform optionsRect = settingsScreen.GetComponent<RectTransform>();
    //     RectTransform mapsRect = selectScreen.GetComponent<RectTransform>();

    //     mainRect.anchoredPosition = mainCanvasPos;
    //     optionsRect.anchoredPosition = settingsScreenPos;
    //     mapsRect.anchoredPosition = mapCanvasPos;

    //     Debug.Log(mainRect.anchoredPosition);
    //     Debug.Log(optionsRect.anchoredPosition);
    //     Debug.Log(mapsRect.anchoredPosition);
    // }
}
