using Game.Utils;
using UnityEngine;
using Unity.VisualScripting;

public class CharacterMovement : MonoBehaviour {
    private GameInputManager input;
    private CharacterComponents components;
    private Rigidbody2D rb;
    public Vector2 faceDir;

    public bool canMove { get; private set; }
    public float gunWeightDebuff => Mathf.Clamp(Mathf.Exp(-components.gunHandler.GunWeight / 15), 0.2f, 1);
    public float isShootingDebuff => components.gunHandler.Gun.Shooting ? 0.8f : 1;
    public float Speed => components.stats.BaseSpeed * gunWeightDebuff * isShootingDebuff;

    private void Awake() {
        faceDir = Vector2.right;
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
        if (components.statsManager.InReviveStage) {
            if (rb.linearVelocity.magnitude > 0.1f) rb.linearVelocity = Vector2.zero;
            return;
        }
        Vector2 dir = input.GetInputDir();
        float moveDist = Speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + (dir * moveDist));
    }

    private void SetFaceDir() {
        if (components.statsManager.InReviveStage) return;
        // bool isMouseBased = GlobalVariables.Instance.isMouseBased();
        bool isShooting = GameInputManager.Instance.IsShooting();
        if (isShooting) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 pointDir = SnapVector((mousePos - (Vector2)transform.position).normalized);
            Vector2 inputDir = input.GetInputDir();
            if (Vector2.Dot(pointDir, inputDir) != 0) {
                faceDir = pointDir;
            }
            else {
                if (inputDir == Vector2.zero || inputDir == faceDir) return;
                faceDir = inputDir;
            }
        }
        else {
            Vector2 dir = input.GetInputDir();
            if (dir == Vector2.zero || dir == faceDir) return;
            faceDir = dir;
        }
    }

    private Vector2 SnapVector(Vector2 vec) {
        if (Vector2.Angle(vec, Vector2.up) <= 45) return Vector2.up;
        else if (Vector2.Angle(vec, Vector2.down) <= 45) return Vector2.down;
        else if (Vector2.Angle(vec, Vector2.left) <= 45) return Vector2.left;
        else return Vector2.right;
    }
}
