using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class ButtonSounds : MonoBehaviour,
    IPointerEnterHandler, IPointerClickHandler {
    [SerializeField] private AudioClip pointerEnterSound;
    [SerializeField] private AudioClip pointerExitSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip disableSound;

    [SerializeField] private AudioSource audioSource;

    private void Awake() {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        PlaySound(pointerEnterSound);
    }

    // public void OnPointerExit(PointerEventData eventData) {
    //     PlaySound(pointerExitSound);
    // }

    public void OnPointerClick(PointerEventData eventData) {
        PlaySound(clickSound);
    }

    public void OnDisable() {
        PlaySound(disableSound);
    }

    private void PlaySound(AudioClip clip) {
        if (clip != null && audioSource != null) {
            float sfxVolume = PlayerPrefs.GetFloat(PrefKeys.sfxVolume, 1f);
            audioSource.PlayOneShot(clip, sfxVolume);
        }
    }

}
