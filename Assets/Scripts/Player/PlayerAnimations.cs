using UnityEngine;

public class PlayerAnimations : MonoBehaviour {
    private GameInputManager input;
    private PlayerShared shared;
    private Animator animator;
    [SerializeField] PlayerAnimationsSO playerAnimations;

    private string currAnimation;
    private string newAnimation;
    private bool isMoving;
    private void Start() {
        input = GameInputManager.Instance;
        shared = GetComponent<PlayerShared>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        HandleMoveAnimation();

        UpdateAnimation();
    }

    private void HandleMoveAnimation() {
        isMoving = input.GetMovementVectorNormalized() != Vector2.zero;
        Vector3 dir = shared.playerDir;
        if (!isMoving) {
            if (dir.y < 0) newAnimation = playerAnimations.IdleDown.name;
            else if (dir.y > 0) newAnimation = playerAnimations.IdleUp.name;
            else if (dir.x > 0) newAnimation = playerAnimations.IdleRight.name;
            else if (dir.x < 0) newAnimation = playerAnimations.IdleLeft.name;
        }
        else {
            if (dir.y < 0) newAnimation = playerAnimations.MoveDown.name;
            else if (dir.y > 0) newAnimation = playerAnimations.MoveUp.name;
            else if (dir.x > 0) newAnimation = playerAnimations.MoveRight.name;
            else if (dir.x < 0) newAnimation = playerAnimations.MoveLeft.name;
        }
        shared.UpdateGunDirection();
    }

    private void UpdateAnimation() {
        if (newAnimation != currAnimation) {
            currAnimation = newAnimation;
            animator.CrossFade(currAnimation, 0.2f);
        }
    }
}
