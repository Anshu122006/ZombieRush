using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {
    [SerializeField] private List<MapImage> maps;
    [SerializeField] private List<CharImage> characters;
    [SerializeField] private TextMeshProUGUI charName;
    [SerializeField] private TextMeshProUGUI charDesc;

    [SerializeField] private ButtonSounds selectButton;

    [SerializeField] private Transform charSlider;
    [SerializeField] private float charSlideDuration = 0.7f;
    [SerializeField] private float charSlideDelta = 1400;

    [SerializeField] private Transform mapSlider;
    [SerializeField] private float mapSlideDuration = 0.7f;
    [SerializeField] private float mapSlideDelta = 300;

    [SerializeField] private List<CharacterDefinition> validChars;
    [SerializeField] private List<SceneName> validMaps;

    private int currentMap = 0;
    private int currentChar = 0;

    private string curPlayerName = "Player1";
    private string curCharName = "";
    private string curMapName = "";

    private Coroutine mapSlideCoroutine;
    private Coroutine charSlideCoroutine;

    private void Start() {
        UpdateCharData();
        if (characters != null) curCharName = characters[0].Name;
        if (maps != null) curMapName = maps[0].Name;
    }

    public void PrevChar() {
        if (currentChar > 0 && charSlideCoroutine == null) {
            charSlideCoroutine = StartCoroutine(SlideChar(-1));
        }
    }

    public void NextChar() {
        if (currentChar < characters.Count - 1 && charSlideCoroutine == null) {
            charSlideCoroutine = StartCoroutine(SlideChar(1));
        }
    }

    public void PrevMap() {
        if (currentMap > 0 && mapSlideCoroutine == null) {
            mapSlideCoroutine = StartCoroutine(SlideMap(-1));
        }
    }

    public void NextMap() {
        if (currentMap < maps.Count - 1 && mapSlideCoroutine == null) {
            mapSlideCoroutine = StartCoroutine(SlideMap(1));
        }
    }

    private void UpdateCharData() {
        if (characters == null) return;
        charName.text = characters[currentChar].Name;
        charDesc.text = characters[currentChar].Desc;
    }

    // side = 1 for next
    private IEnumerator SlideMap(int side) {
        RectTransform rect = mapSlider.GetComponent<RectTransform>();
        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = startPos + Vector2.left * mapSlideDelta * side;
        float elapsed = 0f;

        while (elapsed < mapSlideDuration) {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / mapSlideDuration);
            t = t * t * (3f - 2f * t);
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }
        rect.anchoredPosition = endPos;
        currentMap += side;
        curMapName = maps[currentMap].Name;

        if (IsValidMap() && selectButton.isDisabled) selectButton.EnableButton();
        if (!IsValidMap() && !selectButton.isDisabled) selectButton.DisableButton();

        mapSlideCoroutine = null;
    }

    // side = 1 for next
    private IEnumerator SlideChar(int side) {
        RectTransform rect = charSlider.GetComponent<RectTransform>();
        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = startPos + Vector2.left * charSlideDelta * side;
        float elapsed = 0f;

        while (elapsed < charSlideDuration) {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / charSlideDuration);
            t = t * t * (3f - 2f * t);
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }
        rect.anchoredPosition = endPos;
        currentChar += side;
        curCharName = characters[currentChar].Name;
        UpdateCharData();

        if (IsValidChar() && selectButton.isDisabled) selectButton.EnableButton();
        if (!IsValidChar() && !selectButton.isDisabled) selectButton.DisableButton();

        charSlideCoroutine = null;
    }


    public void OnSelectClick() {
        if (!IsValidChar() || !IsValidMap()) return;

        PlayerPrefs.SetString(PrefKeys.CurPlayerName, curPlayerName);
        PlayerPrefs.SetString(PrefKeys.CurCharacterName, curCharName);
        Loader.LoadScene(curMapName, true);
    }

    private bool IsValidChar() => validChars.Any(x => x.charName == curCharName);
    private bool IsValidMap() => validMaps.Any(x => x.GetString() == curMapName);
}
