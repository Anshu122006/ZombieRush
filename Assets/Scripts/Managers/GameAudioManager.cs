using UnityEngine;
using System.Collections.Generic;

public class GameAudioManager : MonoBehaviour {
    public static GameAudioManager Instance { get; private set; }
    [Header("General Settings")]
    [SerializeField] private AudioSource mainMusicAudioSource;

    [Header("General Settings")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    [Header("Pooling Settings")]
    [SerializeField] private int poolSize = 10;
    [SerializeField] private AudioSource audioSourcePrefab;

    private Queue<AudioSource> audioPool = new Queue<AudioSource>();

    void Awake() {
        // if (Instance != null && Instance != this) {
        //     Destroy(gameObject);
        //     return;
        // }

        Instance = this;
        // DontDestroyOnLoad(gameObject);

        // Initialize the pool
        for (int i = 0; i < poolSize; i++) {
            AudioSource src = Instantiate(audioSourcePrefab, transform);
            src.playOnAwake = false;
            audioPool.Enqueue(src);
        }

        musicVolume = PlayerPrefs.GetFloat(PrefKeys.musicVolume);
        sfxVolume = PlayerPrefs.GetFloat(PrefKeys.sfxVolume);

        mainMusicAudioSource.volume = musicVolume;
        mainMusicAudioSource.Play();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeDefaults() {
        if (PlayerPrefs.HasKey(PrefKeys.HasRunBefore)) return;

        PlayerPrefs.SetFloat(PrefKeys.musicVolume, 1);
        PlayerPrefs.SetFloat(PrefKeys.sfxVolume, 1);
        PlayerPrefs.SetInt(PrefKeys.showHud, 1);

        PlayerPrefs.SetInt(PrefKeys.HasRunBefore, 1);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Plays a sound at a specific world position.
    /// </summary>
    public void PlaySound(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f) {
        if (clip == null) return;

        AudioSource source = GetAvailableSource();
        source.transform.position = position;
        source.clip = clip;
        source.volume = volume * sfxVolume;
        source.pitch = pitch;
        source.spatialBlend = 1f;
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
        source.volume = volume * sfxVolume;
        source.pitch = pitch;
        source.spatialBlend = 0f;
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
    public void SetMusicVolume(float value) {
        musicVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(PrefKeys.musicVolume, value);

        mainMusicAudioSource.volume = musicVolume;
    }

    public void SetSFXVolume(float value) {
        sfxVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(PrefKeys.sfxVolume, value);
    }
}
