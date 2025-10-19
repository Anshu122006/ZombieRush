using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Healthbar : MonoBehaviour {
    [SerializeField] private Transform fill;
    [SerializeField] private SpriteRenderer[] renderers;
    [SerializeField] private float fadeinTime;
    [SerializeField] private float stayTime;
    [SerializeField] private float fadeoutTime;

    private Coroutine fadeCoroutine;
    private float stayElapsed = 0;

    public void SetFill(float value) {
        if (value > 1)
            fill.localScale = new Vector3(1, 1, 1);
        else if (value < 0)
            fill.localScale = new Vector3(0, 1, 1);
        else
            fill.localScale = new Vector3(value, 1, 1);
    }

    public void Fade() {
        if (fadeCoroutine == null) fadeCoroutine = StartCoroutine(FadeAnimation());
        else stayElapsed = 0;
    }

    public IEnumerator FadeAnimation() {
        Debug.Log("Started");
        foreach (var r in renderers) {
            Color color = r.color;
            r.color = new Color(color.r, color.g, color.b, 0);
        }

        float elapsed = 0;
        stayElapsed = 0;

        // fadein
        while (elapsed < fadeinTime) {
            float t = elapsed / fadeinTime;
            foreach (var r in renderers) {
                Color color = r.color;
                r.color = new Color(color.r, color.g, color.b, t);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        foreach (var r in renderers) {
            Color color = r.color;
            r.color = new Color(color.r, color.g, color.b, 1);
        }

        // stay
        while (stayElapsed < stayTime) {
            float t = stayElapsed / fadeinTime;
            stayElapsed += Time.deltaTime;
            yield return null;
        }

        // fadeout
        elapsed = fadeinTime;
        while (elapsed >= 0) {
            float t = elapsed / fadeinTime;
            foreach (var r in renderers) {
                Color color = r.color;
                r.color = new Color(color.r, color.g, color.b, t);
            }
            elapsed -= Time.deltaTime;
            yield return null;
        }
        foreach (var r in renderers) {
            Color color = r.color;
            r.color = new Color(color.r, color.g, color.b, 0);
        }

        gameObject.SetActive(false);
        fadeCoroutine = null;
    }
}
