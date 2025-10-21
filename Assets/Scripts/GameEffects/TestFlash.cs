using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class TestFlash : MonoBehaviour {
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float flashDuration = 3f;
    private Coroutine flashCoroutine;

    private void Update() {
        if (flashCoroutine == null) {
            flashCoroutine = StartCoroutine(Flash());
        }
    }

    private IEnumerator Flash() {
        Color original = spriteRenderer.color;
        float elapsed = 0;
        while (elapsed <= flashDuration) {
            elapsed += Time.deltaTime;
            float t = 1 + 0.5f * (elapsed / flashDuration);
            spriteRenderer.color = new Color(t, t, t, 1f);
            yield return null;
        }
        yield return new WaitForSeconds(flashDuration);
        while (elapsed >= 0) {
            elapsed -= Time.deltaTime;
            float t = 1 + 0.5f * (elapsed / flashDuration);
            spriteRenderer.color = new Color(t, t, t, 1f);
            yield return null;
        }
        spriteRenderer.color = original;
        yield return new WaitForSeconds(0.7f);
        flashCoroutine = null;
    }
}
