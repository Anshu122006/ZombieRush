using Game.Utils;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private GameInputManager input;
    private Rigidbody2D rb;
    private PlayerShared shared;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        input = GameInputManager.Instance;
        shared = GetComponent<PlayerShared>();
    }

    private void FixedUpdate() {
        HandleMovement();
    }

    private void HandleMovement() {
        Vector2 newDir = input.GetMovementVectorNormalized();
        float moveDist = shared.moveSpeed * Time.fixedDeltaTime;
        if (newDir.x != 0 && newDir.y != 0) {
            if (shared.playerDir.x != 0)
                newDir.y = 0;
            else if (shared.playerDir.y != 0)
                newDir.x = 0;
        }
        newDir = newDir.normalized;

        if (newDir != Vector2.zero)
            rb.MovePosition(rb.position + (newDir * moveDist));
        if (newDir != Vector2.zero) shared.playerDir = newDir;
        // shared.firePoint.eulerAngles = VectorHandler.AngleFromVector(shared.playerDir);
    }

    public float GetMoveSpeed() {
        return shared.moveSpeed;
    }
}
