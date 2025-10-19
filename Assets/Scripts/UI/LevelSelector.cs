using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {
    [SerializeField] private List<MapImage> maps;
    [SerializeField] private List<CharImage> characters;
    [SerializeField] private TextMeshProUGUI charName;
    [SerializeField] private TextMeshProUGUI charDesc;

    // [SerializeField] private TextMeshPro mapName;
    // [SerializeField] private TextMeshPro mapDesc;


    [SerializeField] private Transform charSlider;
    [SerializeField] private float charSlideDuration = 0.7f;
    [SerializeField] private float charSlideDelta = 1400;

    [SerializeField] private Transform mapSlider;
    [SerializeField] private float mapSlideDuration = 0.7f;
    [SerializeField] private float mapSlideDelta = 300;

    private int currentMap = 0;
    private int currentChar = 0;

    private Coroutine mapSlideCoroutine;
    private Coroutine charSlideCoroutine;

    private void Start() {
        UpdateCharData();
    }

    public void PrevChar() {
        Debug.Log(currentChar);
        Debug.Log(charSlideCoroutine);
        if (currentChar > 0 && charSlideCoroutine == null) {
            charSlideCoroutine = StartCoroutine(SlideChar(-1));
        }
    }

    public void NextChar() {
        Debug.Log(currentChar);
        Debug.Log(charSlideCoroutine);
        if (currentChar < characters.Count - 1 && charSlideCoroutine == null) {
            charSlideCoroutine = StartCoroutine(SlideChar(1));
        }
    }

    public void PrevMap() {
        Debug.Log(currentMap);
        Debug.Log(mapSlideCoroutine);
        if (currentMap > 0 && mapSlideCoroutine == null) {
            mapSlideCoroutine = StartCoroutine(SlideMap(-1));
        }
    }

    public void NextMap() {
        Debug.Log(currentMap);
        Debug.Log(mapSlideCoroutine);
        if (currentMap < maps.Count - 1 && mapSlideCoroutine == null) {
            mapSlideCoroutine = StartCoroutine(SlideMap(1));
        }
    }

    private void UpdateCharData() {
        if (characters == null) return;
        charName.text = characters[currentChar].data.charName;
        charDesc.text = characters[currentChar].data.charDesc;
    }

    // private void UpdateMapData() {
    //     charName.text = characters[currentChar].data.charName;
    //     charDesc.text = characters[currentChar].data.charDesc;
    // }

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
        UpdateCharData();
        charSlideCoroutine = null;
    }
}
