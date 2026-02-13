using System;
using System.Collections;
using UnityEngine;

public abstract class IItem : MonoBehaviour {
    [SerializeField] protected string itemName = "item";
    [SerializeField] private AudioClip pickupAudio;
    [SerializeField] protected int maxItemPerSpawnArea = 3;
    [SerializeField] protected float speed = 3;
    [SerializeField] protected float accleration = 0.3f;
    [SerializeField] protected float stayTime = 2f;
    [SerializeField] protected float fadeTime = 0.03f;

    public string ItemName => itemName;
    public float chance = 0.1f;
    public int MaxItemPerSpawnArea => maxItemPerSpawnArea;
    public Action onDestroyed;

    private Rigidbody2D rb;
    private Transform target;
    private bool collected = false;
    private bool destroyed = false;

    protected abstract void OnCollect(CharacterComponents player);

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(DestroyOnTimeUp());
    }

    private void FixedUpdate() {
        if (target == null) return;
        if (Vector2.Distance(target.position, transform.position) > 0.8f) {
            Vector2 dir = (target.position - transform.position).normalized;
            float dist = speed * Time.fixedDeltaTime;
            rb.MovePosition((Vector2)transform.position + dir * dist);
            speed += accleration;
        }
        else {
            if (!destroyed) DestroySelf();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        CharacterComponents comp = collider.GetComponent<CharacterComponents>();
        if (comp != null && !collected) {
            target = comp.transform;
            collected = true;
        }
    }

    protected void DestroySelf() {
        destroyed = true;
        OnCollect(target.GetComponent<CharacterComponents>());
        GameAudioManager.Instance.PlaySound(pickupAudio, transform.position);
        StartCoroutine(ItemFade());
    }

    private IEnumerator DestroyOnTimeUp() {
        if (destroyed) yield break;
        yield return new WaitForSeconds(stayTime);
        StartCoroutine(ItemFade());
    }

    private IEnumerator ItemFade() {
        float elapsed = 0;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        while (elapsed <= fadeTime) {
            elapsed += Time.deltaTime;
            float currentFade = Mathf.Lerp(1, 0, elapsed / fadeTime);
            spriteRenderer.color = new Color(color.r, color.g, color.b, currentFade);
            yield return null;
        }
        spriteRenderer.color = new Color(color.r, color.g, color.b, 0);

        onDestroyed?.Invoke();
        Destroy(gameObject);
    }
}
