using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCreationManager : MonoBehaviour {
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<Transform> charPrefs;
    [SerializeField] private CinemachineCamera followCamera;

    private Dictionary<string, Transform> charPrefsDict = new();

    private void Start() {
        foreach (var pref in charPrefs) {
            string charName = pref.GetComponent<CharacterComponents>().stats.Name;
            charPrefsDict[charName] = pref;
        }

        string curChar = PlayerPrefs.GetString(PrefKeys.CurCharacterName);
        curChar ??= charPrefs[0].GetComponent<CharacterComponents>().stats.Name;

        Transform player = Instantiate(charPrefsDict[curChar], spawnPoints[0].position, Quaternion.identity);
        player.GetComponent<CharacterStatsManager>().spawnPoints = spawnPoints;
        followCamera.Follow = player;
        Destroy(gameObject);
    }
}
