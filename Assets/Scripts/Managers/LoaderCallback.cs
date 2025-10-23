using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoaderCallback : MonoBehaviour {
    [Header("LOADING SCREEN")]
    public bool isSilent = false;
    public bool waitForInput = true;
    public GameObject loadingMenu;
    public Slider loadingBar;
    public TextMeshProUGUI loadPromptText;
    public KeyCode userPromptKey = KeyCode.Space;

    [Header("LOADING SETTINGS")]
    public float baseDelay = 0.3f;

    private void Start() {
        if (isSilent) {
            loadingMenu.SetActive(false);
            loadPromptText.gameObject.SetActive(false);
            loadingBar.gameObject.SetActive(false);
            SceneManager.LoadScene(Loader.TargetScene);
        }
        else {
            loadingBar.value = 0;
            loadPromptText.gameObject.SetActive(false);
            StartCoroutine(LoadAsynchronously());
        }
    }

    private IEnumerator LoadAsynchronously() {
        AsyncOperation operation = SceneManager.LoadSceneAsync(Loader.TargetScene);
        operation.allowSceneActivation = false;

        float elapsedTime = 0f;
        float visualProgress = 0f;

        while (!operation.isDone) {
            float sceneProgress = Mathf.Clamp01(operation.progress / 0.9f);
            elapsedTime += Time.deltaTime;

            visualProgress = Mathf.Pow(Mathf.Min(sceneProgress, elapsedTime / baseDelay), 0.3f);
            loadingBar.value = visualProgress;

            if (sceneProgress >= 1f && elapsedTime >= baseDelay) {
                if (waitForInput) {
                    loadPromptText.gameObject.SetActive(true);
                    loadPromptText.text = $"Press [{userPromptKey}] to continue";
                    yield return new WaitUntil(() => Input.GetKeyDown(userPromptKey));
                }

                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
