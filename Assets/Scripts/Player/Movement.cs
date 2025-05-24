using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField]
    private float moveSpeed = 10;
    private Rigidbody2D rigidBody;

    private GameInputManager input;
    private void Start() {
        input = GameInputManager.Instance;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update() {

    }

    private void FixedUpdate() {
        HandleMovement();
    }

    private void HandleMovement() {
        Vector2 moveDir = input.getMoveDir();
        float moveDist = moveSpeed * Time.deltaTime;
        Vector2 newPos = rigidBody.position + moveDir * moveDist;

        rigidBody.MovePosition(newPos);
    }
}
