using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenuManager : MonoBehaviour {
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject HighScorePanel;

    [SerializeField] private float fadeoutTime = 0.7f;
    [SerializeField] private float fadeinTime = 0.7f;
    [SerializeField] private float gameoverStayTime = 2;
    [SerializeField] private AudioClip gameOverAudio;

    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI characterName;

    [SerializeField] private TextMeshProUGUI gameoverText;

    [SerializeField] private TextMeshProUGUI curMaxLevel;
    [SerializeField] private TextMeshProUGUI curMaxGold;
    [SerializeField] private TextMeshProUGUI curKills;
    [SerializeField] private TextMeshProUGUI curSurvivalTime;

    [SerializeField] private TextMeshProUGUI highestMaxLevel;
    [SerializeField] private TextMeshProUGUI highestMaxGold;
    [SerializeField] private TextMeshProUGUI highestKills;
    [SerializeField] private TextMeshProUGUI highestSurvivalTime;

    [SerializeField] private Image charImage;
    [SerializeField] private List<CharacterDefinition> charDefinitions;

    private Dictionary<string, CharacterDefinition> charDefDict = new();

    private void Start() {
        HighScorePanel.SetActive(false);
        foreach (var def in charDefinitions)
            charDefDict[def.charName] = def;
        GlobalGameManager.Instance.SaveSurvivalTime();
        StartCoroutine(StartFade());
    }

    private void LoadData() {
        string charName = PlayerPrefs.GetString(PrefKeys.CurCharacterName) ?? "";
        if (charDefDict?.ContainsKey(charName) ?? false) charImage.sprite = charDefDict[charName].bust;

        playerName.text = PlayerPrefs.GetString(PrefKeys.CurPlayerName) ?? "Player1";
        characterName.text = charName;

        curMaxLevel.text = PlayerPrefs.GetInt(PrefKeys.CurMaxLevel).ToString();
        curMaxGold.text = PlayerPrefs.GetInt(PrefKeys.CurGold).ToString();
        curKills.text = PlayerPrefs.GetInt(PrefKeys.CurKills).ToString();
        curSurvivalTime.text = ConvertToTime(PlayerPrefs.GetFloat(PrefKeys.CurSurvivalTime));

        highestMaxLevel.text = PlayerPrefs.GetInt(PrefKeys.HighestMaxLevel).ToString();
        highestMaxGold.text = PlayerPrefs.GetInt(PrefKeys.HighestGold).ToString();
        highestKills.text = PlayerPrefs.GetInt(PrefKeys.HighestKills).ToString();
        highestSurvivalTime.text = ConvertToTime(PlayerPrefs.GetFloat(PrefKeys.HighestSurvivalTime));
    }

    private IEnumerator StartFade() {
        float elapsed = 0;
        Color color = gameoverText.color;
        gameoverText.color = new Color(color.r, color.g, color.b, 0);

        GameAudioManager.Instance.PlaySound(gameOverAudio, Vector2.zero);
        yield return new WaitForSeconds(gameOverAudio.length * 0.3f);

        while (elapsed <= fadeinTime) {
            elapsed += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(0, 1, elapsed / fadeinTime);
            gameoverText.color = new Color(color.r, color.g, color.b, currentAlpha);
            yield return null;
        }
        gameoverText.color = new Color(color.r, color.g, color.b, 1);

        yield return new WaitForSeconds(gameOverAudio.length * 0.3f);
        elapsed = 0;
        while (elapsed <= fadeinTime) {
            elapsed += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(1, 0, elapsed / fadeinTime);
            gameoverText.color = new Color(color.r, color.g, color.b, currentAlpha);
            yield return null;
        }
        gameoverText.color = new Color(color.r, color.g, color.b, 0);

        GameOverPanel.SetActive(false);
        HighScorePanel.SetActive(true);
        LoadData();
    }

    private string ConvertToTime(float val) {
        int hrs = (int)val / 3600;
        int mins = (int)(val % 3600) / 60;
        int sec = (int)val % 60;
        return hrs + "h " + mins + "m " + sec + "s";
    }

    public void ReturnToMainMenu() {
        Time.timeScale = 1;
        Loader.LoadScene(SceneName.MainMenu.GetString());
    }
}
