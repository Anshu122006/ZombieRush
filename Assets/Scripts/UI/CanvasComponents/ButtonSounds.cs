using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class ButtonSounds : MonoBehaviour,
    IPointerEnterHandler, IPointerClickHandler {
    [SerializeField] private AudioClip pointerEnterSound;
    [SerializeField] private AudioClip pointerExitSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip disableSound;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject disableOverlay;

    public bool isDisabled = false;

    private void Awake() {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        PlaySound(pointerEnterSound);
    }

    public void OnPointerClick(PointerEventData eventData) {
        PlaySound(isDisabled ? disableSound : clickSound);
    }

    public void DisableButton() {
        disableOverlay.SetActive(true);
        GetComponent<Button>().interactable = false;
        isDisabled = true;
    }

    public void EnableButton() {
        disableOverlay.SetActive(false);
        GetComponent<Button>().interactable = true;
        isDisabled = false;
    }


    private void PlaySound(AudioClip clip) {
        if (clip != null && audioSource != null && gameObject.activeSelf) {
            float sfxVolume = PlayerPrefs.GetFloat(PrefKeys.sfxVolume, 1f);
            audioSource.PlayOneShot(clip, sfxVolume);
        }
    }
}
