using System.Collections;
using UnityEngine;

public class BulletShell : MonoBehaviour {
    [SerializeField] private float stayTime = 2f;
    [SerializeField] private float fadeTime = 1.6f;
    [SerializeField] private float forceMag = 7f;
    [SerializeField] private float torqueMag = 7f;
    [SerializeField] private AudioClip shellDropAudio;

    private Rigidbody2D rb;
    private bool soundPlayed = false;

    private void Update() {
        if (rb.linearVelocity.magnitude < 0.6f && !soundPlayed) {
            GameAudioManager.Instance.PlaySound(shellDropAudio, transform.position);
            soundPlayed = true;
        }
    }

    public void Setup() {
        rb = GetComponent<Rigidbody2D>();
        Vector2 dir = Vector2.down + Vector2.right * Random.Range(-1f, 1f);
        rb.AddForce(dir * forceMag, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-1f, 1f) * torqueMag, ForceMode2D.Impulse);
        StartCoroutine(BulletFade());
    }

    private IEnumerator BulletFade() {
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
