using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CharacterEffectsManager : MonoBehaviour {
    [SerializeField] private float detectionRate = 0.1f;
    [SerializeField] private float detectionRange = 6;

    [SerializeField] private float torchFlickerDelay = 6;
    [SerializeField] private float randomSoundDelay = 3;
    [SerializeField] private float zombieEnterSoundDelay = 3;
    [SerializeField] private float zombieExitSoundDelay = 3;
    [SerializeField] private float zombieInRangeSoundDelay = 3;

    [SerializeField] private float flickerSpeed = 0.3f;
    [SerializeField] private float minIntensity = 0.3f;
    [SerializeField] private float originalIntensity = 0.4f;

    [SerializeField] private LayerMask zombieLayer;

    [SerializeField] private Light2D torch;
    [SerializeField] private List<AudioClip> randomSounds;
    [SerializeField] private List<AudioClip> zombieEnterSound;
    [SerializeField] private List<AudioClip> zombieExitSound;
    [SerializeField] private List<AudioClip> zombieInRangeSound;

    private Coroutine randomSoundCoroutine;
    private Coroutine zombieInRangeCoroutine;
    private Coroutine flickerDelayCoroutine;

    private bool zombieInRange = false;

    private void Start() {
        InvokeRepeating("PerformDetection", 0.3f, detectionRate);
        InvokeRepeating("PlayRandomSound", 0.3f, detectionRate);
        InvokeRepeating("TorchFlicker", 0.3f, detectionRate);
    }

    private void PerformDetection() {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRange, zombieLayer);
        if (hit != null) {
            if (zombieInRange) {
                if (zombieInRangeCoroutine != null) return;
                AudioClip audioEffect = zombieInRangeSound[Random.Range(0, zombieInRangeSound.Count)];
                GameAudioManager.Instance.PlaySound(audioEffect, transform.position, 3);
                zombieInRangeCoroutine = StartCoroutine(DelayInRangeSound());
            }
            else {
                AudioClip audioEffect = zombieEnterSound[Random.Range(0, zombieEnterSound.Count)];
                GameAudioManager.Instance.PlaySound(audioEffect, transform.position);
                zombieInRange = true;
            }
        }
        else {
            if (zombieInRange) {
                AudioClip audioEffect = zombieExitSound[Random.Range(0, zombieExitSound.Count)];
                GameAudioManager.Instance.PlaySound(audioEffect, transform.position);
                zombieInRange = false;
            }
        }
    }

    private void PlayRandomSound() {
        if (randomSoundCoroutine != null) return;
        int chance = Random.Range(1, 101);
        if (chance < 40) return;
        // Debug.Log("Random sound played");
        AudioClip audioEffect = randomSounds[Random.Range(0, randomSounds.Count)];
        GameAudioManager.Instance.PlaySound(audioEffect, transform.position);
        randomSoundCoroutine = StartCoroutine(DelayRandomSound());
    }

    private void TorchFlicker() {
        if (flickerDelayCoroutine != null) return;
        int chance = Random.Range(1, 101);
        if (chance < 60) return;

        StartCoroutine(FlickerOnce());
        flickerDelayCoroutine = StartCoroutine(DelayTorchFlicker());
    }

    private IEnumerator DelayInRangeSound() {
        yield return new WaitForSeconds(Random.Range(zombieInRangeSoundDelay, zombieInRangeSoundDelay + 2f));
        zombieInRangeCoroutine = null;
    }

    private IEnumerator DelayRandomSound() {
        yield return new WaitForSeconds(randomSoundDelay);
        randomSoundCoroutine = null;
    }

    private IEnumerator DelayTorchFlicker() {
        yield return new WaitForSeconds(torchFlickerDelay);
        flickerDelayCoroutine = null;
    }

    private IEnumerator FlickerOnce() {
        int flickerCount = Random.Range(3, 7);
        for (int i = 0; i < flickerCount; i++) {
            float targetIntensity = Random.Range(minIntensity, minIntensity * 1.2f);

            float elapsed = 0f;
            while (elapsed < flickerSpeed) {
                torch.intensity = Mathf.Lerp(originalIntensity, targetIntensity, elapsed / flickerSpeed);
                elapsed += Time.deltaTime;
                yield return null;
            }
            torch.intensity = targetIntensity;

            elapsed = 0f;
            while (elapsed < flickerSpeed) {
                torch.intensity = Mathf.Lerp(targetIntensity, originalIntensity, elapsed / flickerSpeed);
                elapsed += Time.deltaTime;
                yield return null;
            }
            torch.intensity = originalIntensity;
            yield return null;
        }
    }
}
