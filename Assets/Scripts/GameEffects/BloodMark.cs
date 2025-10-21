using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodMark : MonoBehaviour {
    [SerializeField] private float stayTime = 3;
    [SerializeField] private float fadeTime = 1;
    [SerializeField] private float adjustmentFactor = 1;
    [SerializeField] private float offset = 0.7f;
    [SerializeField] private List<Sprite> bloodSprites;

    private SpriteRenderer spriteRenderer;

    public void Setup(Vector2 pos) {
        pos += Vector2.right * Random.Range(-offset, offset) + Vector2.up * Random.Range(-offset, offset) * 0.3f;
        transform.position = pos + Vector2.down * adjustmentFactor;

        if (bloodSprites == null || bloodSprites.Count == 0) return;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = bloodSprites[Random.Range(0, bloodSprites.Count)];

        StartCoroutine(BloodFade());
    }

    private IEnumerator BloodFade() {
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
