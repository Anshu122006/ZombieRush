using Game.Utils;
using UnityEngine;
using Unity.VisualScripting;

public class Movement : MonoBehaviour {
    private GameInputManager input;
    private PlayerShared shared;
    private Rigidbody2D rb;
    public Vector2 newDir;
    public Vector2 currDir;
    private Vector2 prevInput;
    private bool m_Dash;
    public Vector2 playerDir;
    public bool canMove { get; private set; }
    public bool canDash = true;
    public bool inDash = false;
    public float baseMoveSpeed;
    public float moveSpeed;

    private void Awake() {
        newDir = Vector2.right;
        currDir = Vector2.right;
        playerDir = Vector2.right;
        rb = GetComponent<Rigidbody2D>();
        canMove = true;
    }
    private void Start() {
        input = GameInputManager.Instance;
        shared = GetComponent<PlayerShared>();
    }

    private void Update() {
        FixDirection();
    }

    private void FixedUpdate() {
        HandleMovement();
    }

    private void FixDirection() {
        newDir = input.GetMovementVectorNormalized();

        if (prevInput.x != 0 && prevInput.y != 0) {
            if (newDir.x != 0 && newDir.y != 0) {
                if (currDir.x != 0)
                    newDir.y = 0;
                else if (currDir.y != 0)
                    newDir.x = 0;
            }
        }
        else {
            if (newDir.x != 0 && newDir.y != 0) {
                if (currDir.x != 0)
                    newDir.x = 0;
                else if (currDir.y != 0)
                    newDir.y = 0;
            }
        }
        newDir = newDir.normalized;
        prevInput = input.GetMovementVectorNormalized();
        if (newDir != Vector2.zero) playerDir = newDir;
    }

    private void HandleMovement() {
        float moveDist = moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + (newDir * moveDist));
        if (newDir != Vector2.zero) currDir = newDir;
    }

    public void DisableMovement() {
        canMove = false;
    }

    public void EnableMovement() {
        canMove = true;
    }
}
