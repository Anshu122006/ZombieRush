using UnityEngine;

public class GameInputManager : MonoBehaviour {
    private InputActions inputActions;

    public static GameInputManager Instance { get; private set; }

    void Awake() {
        Instance = GetComponent<GameInputManager>();

        inputActions = new InputActions();
        inputActions.Player.Enable();
    }

    public Vector2 getMoveDir() {
        Vector2 inputVector = inputActions.Player.Movement.ReadValue<Vector2>();

        return inputVector;
    }
}
