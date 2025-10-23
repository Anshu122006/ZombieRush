using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roof : MonoBehaviour {
    [SerializeField] private float enterSoundDelay = 3;
    [SerializeField] private GameObject gateLight;
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private List<AudioClip> enterSounds;

    private Coroutine enterSoundCoroutine;

    private void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log(collider);
        if ((targetLayers & (1 << collider.gameObject.layer)) != 0) {
            Debug.Log("invisible");
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) return;
            Color color = spriteRenderer.color;
            spriteRenderer.color = new Color(color.r, color.g, color.b, 0.1f);
            gateLight.SetActive(false);

            if (enterSoundCoroutine == null) {
                int chance = Random.Range(1, 101);
                if (chance < 40) return;
                AudioClip audioEffect = enterSounds[Random.Range(0, enterSounds.Count)];
                GameAudioManager.Instance.PlaySound(audioEffect, transform.position);
                enterSoundCoroutine = StartCoroutine(DelayEnterSound());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if ((targetLayers & (1 << collider.gameObject.layer)) != 0) {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) return;
            Color color = spriteRenderer.color;
            spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);
            gateLight.SetActive(true);
        }
    }

    private IEnumerator DelayEnterSound() {
        yield return new WaitForSeconds(enterSoundDelay);
        enterSoundCoroutine = null;
    }
}
