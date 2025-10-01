using Game.Utils;
using UnityEngine;
using Unity.VisualScripting;

public class CharacterMovement : MonoBehaviour {
    private GameInputManager input;
    private CharacterComponents components;
    private Rigidbody2D rb;

    public Vector2 prevDir;
    public Vector2 curDir;
    public Vector2 faceDir;

    public bool canMove { get; private set; }
    private float SpeedDebuff => Mathf.Clamp(Mathf.Exp(-components.gunHandler.GunWeight / 15), 0.2f, 1);

    private void Awake() {
        prevDir = Vector2.down;
        curDir = Vector2.down;
        faceDir = Vector2.down;
        rb = GetComponent<Rigidbody2D>();
        canMove = true;
    }
    private void Start() {
        input = GameInputManager.Instance;
        components = GetComponent<CharacterComponents>();
    }

    private void Update() {
        SetFaceDir();
    }

    private void FixedUpdate() {
        HandlePlayerMovement();
    }

    private void HandlePlayerMovement() {
        curDir = input.GetMovementVectorNormalized();
        float moveDist = components.stats.BaseSpeed * Time.fixedDeltaTime;
        if (input.IsShooting() && components.gunHandler.Gun.CanShoot) moveDist *= SpeedDebuff;

        rb.MovePosition(rb.position + (curDir * moveDist));
        // prevDir = curDir;
    }

    private void SetFaceDir() {
        // if (curDir == Vector2.zero) return;
        Vector2 pos = input.GetMousePosition();
        Vector2 dir = pos - (Vector2)transform.position;
        if (Vector2.Angle(Vector2.up, dir) < 45)
            faceDir = Vector2.up;
        else if (Vector2.Angle(Vector2.down, dir) < 45)
            faceDir = Vector2.down;
        else if (Vector2.Angle(Vector2.right, dir) < 45)
            faceDir = Vector2.right;
        else
            faceDir = Vector2.left;

        // if (curDir.x == 0 || curDir.y == 0) {
        //     faceDir = curDir;
        // }
    }

    public void DisableMovement() => canMove = false;
    public void EnableMovement() => canMove = true;
}
