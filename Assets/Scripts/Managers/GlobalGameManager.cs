using UnityEngine;

public class GlobalGameManager : MonoBehaviour {
    public static GlobalGameManager Instance;

    private float startTime;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            startTime = Time.time;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }
    }

    public void SaveCharacterInfo(string playerName, string characterName) {
        PlayerPrefs.SetString(PrefKeys.CurPlayerName, playerName);
        PlayerPrefs.SetString(PrefKeys.CurCharacterName, characterName);
    }

    public void SaveCurGold(int val) {
        int highest = PlayerPrefs.GetInt(PrefKeys.HighestGold);
        if (val > highest) PlayerPrefs.SetInt(PrefKeys.HighestGold, val);
        PlayerPrefs.SetInt(PrefKeys.CurGold, val);
    }

    public void SaveCurKills(int val) {
        int highest = PlayerPrefs.GetInt(PrefKeys.HighestKills);
        if (val > highest) PlayerPrefs.SetInt(PrefKeys.HighestKills, val);
        PlayerPrefs.SetInt(PrefKeys.CurKills, val);
    }

    public void SaveMaxLevel(int val) {
        int highest = PlayerPrefs.GetInt(PrefKeys.HighestMaxLevel);
        if (val > highest) PlayerPrefs.SetInt(PrefKeys.HighestMaxLevel, val);
        PlayerPrefs.SetInt(PrefKeys.CurMaxLevel, val);
    }

    public void SaveSurvivalTime() {
        float curTime = Time.time;
        float duration = curTime - startTime;
        float highest = PlayerPrefs.GetFloat(PrefKeys.HighestSurvivalTime);
        if (duration > highest) PlayerPrefs.SetFloat(PrefKeys.HighestSurvivalTime, duration);
        PlayerPrefs.SetFloat(PrefKeys.CurSurvivalTime, duration);
    }
}
