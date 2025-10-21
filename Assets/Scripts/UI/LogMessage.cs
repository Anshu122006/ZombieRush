using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LogMessage : MonoBehaviour {
    [SerializeField] private float fadeTime = 0.7f;
    [SerializeField] private float stayTime = 2;
    [SerializeField] private TextMeshProUGUI message;

    public void Setup(string text) {
        message.text = text;
        StartCoroutine(MessageFade());
    }

    private IEnumerator MessageFade() {
        yield return new WaitForSeconds(stayTime);
        float elapsed = 0;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        while (elapsed <= fadeTime) {
            elapsed += Time.deltaTime;
            float t = 1 - (elapsed / fadeTime);
            spriteRenderer.color = new Color(color.r, color.g, color.b, t);
            yield return null;
        }
        spriteRenderer.color = new Color(color.r, color.g, color.b, 0);
        Destroy(gameObject);
    }
}
