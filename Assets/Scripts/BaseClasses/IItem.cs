using System;
using System.Collections;
using UnityEngine;

public abstract class IItem : MonoBehaviour {
    [SerializeField] protected string itemName = "item";
    [SerializeField] private AudioClip pickupAudio;
    [SerializeField] protected int maxItemPerSpawnArea = 3;
    [SerializeField] protected float speed = 3;
    [SerializeField] protected float accleration = 0.3f;

    public string Name => itemName;
    public int MaxItemPerSpawnArea => maxItemPerSpawnArea;
    public Action onDestroyed;

    private Rigidbody2D rb;
    private Transform target;
    private bool collected = false;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if (target == null) return;
        if (Vector2.Distance(target.position, transform.position) > 1f) {
            Vector2 dir = (target.position - transform.position).normalized;
            float dist = speed * Time.fixedDeltaTime;
            rb.MovePosition((Vector2)transform.position + dir * dist);
            speed += accleration;
        }
        else {
            GameAudioManager.Instance.PlaySound(pickupAudio, transform.position);
            OnCollect(target.GetComponent<CharacterComponents>());
            DestroySelf();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        CharacterComponents comp = collider.GetComponent<CharacterComponents>();
        if (comp != null && !collected) {
            target = comp.transform;
            collected = true;
        }
    }

    protected abstract void OnCollect(CharacterComponents player);

    protected void DestroySelf(float time = 0) {
        onDestroyed?.Invoke();
        Destroy(gameObject, time);
    }
}
