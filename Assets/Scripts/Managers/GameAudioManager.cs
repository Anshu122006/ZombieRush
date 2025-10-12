using UnityEngine;
using System.Collections.Generic;

public class GameAudioManager : MonoBehaviour {
    public static GameAudioManager Instance { get; private set; }

    [Header("General Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    [Header("Pooling Settings")]
    [SerializeField] private int poolSize = 10;
    [SerializeField] private AudioSource audioSourcePrefab;

    private Queue<AudioSource> audioPool = new Queue<AudioSource>();

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize the pool
        for (int i = 0; i < poolSize; i++) {
            AudioSource src = Instantiate(audioSourcePrefab, transform);
            src.playOnAwake = false;
            audioPool.Enqueue(src);
        }
    }

    /// <summary>
    /// Plays a sound at a specific world position.
    /// </summary>
    public void PlaySound(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f) {
        if (clip == null) return;

        AudioSource source = GetAvailableSource();
        source.transform.position = position;
        source.clip = clip;
        source.volume = volume * sfxVolume * masterVolume;
        source.pitch = pitch;
        source.spatialBlend = 1f; // 3D sound
        source.Play();

        StartCoroutine(ReturnToPoolAfterPlay(source, clip.length / pitch));
    }

    /// <summary>
    /// Plays a 2D sound (UI click, etc.).
    /// </summary>
    public void PlaySound2D(AudioClip clip, float volume = 1f, float pitch = 1f) {
        if (clip == null) return;

        AudioSource source = GetAvailableSource();
        source.transform.position = transform.position;
        source.clip = clip;
        source.volume = volume * sfxVolume * masterVolume;
        source.pitch = pitch;
        source.spatialBlend = 0f; // 2D sound
        source.Play();

        StartCoroutine(ReturnToPoolAfterPlay(source, clip.length / pitch));
    }

    /// <summary>
    /// Fetches an available AudioSource from the pool.
    /// </summary>
    private AudioSource GetAvailableSource() {
        if (audioPool.Count > 0) {
            AudioSource src = audioPool.Dequeue();
            src.gameObject.SetActive(true);
            return src;
        }

        // If pool exhausted, create a new one (optional)
        AudioSource newSrc = Instantiate(audioSourcePrefab, transform);
        newSrc.playOnAwake = false;
        return newSrc;
    }

    /// <summary>
    /// Returns AudioSource to pool after playback ends.
    /// </summary>
    private System.Collections.IEnumerator ReturnToPoolAfterPlay(AudioSource src, float delay) {
        yield return new WaitForSeconds(delay);
        src.Stop();
        src.clip = null;
        src.gameObject.SetActive(false);
        audioPool.Enqueue(src);
    }

    /// <summary>
    /// Adjust global volumes dynamically.
    /// </summary>
    public void SetMasterVolume(float value) {
        masterVolume = Mathf.Clamp01(value);
    }

    public void SetSFXVolume(float value) {
        sfxVolume = Mathf.Clamp01(value);
    }
}
