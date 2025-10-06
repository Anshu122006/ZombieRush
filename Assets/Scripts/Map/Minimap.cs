using UnityEngine;

public class Minimap : MonoBehaviour {
    private GameObject playerFollowCamera;

    private void Start() {
        playerFollowCamera = GameObject.FindWithTag("PlayerFollowCamera");
    }
}
