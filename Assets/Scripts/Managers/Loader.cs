using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader {
    public static string TargetScene { get; private set; }

    public static void LoadScene(string targetScene, bool waitForInput = false) {
        TargetScene = targetScene;
        if (waitForInput) SceneManager.LoadScene("LoadInput");
        else SceneManager.LoadScene("LoadNoInput");
    }
}   
