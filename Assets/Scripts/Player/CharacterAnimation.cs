using System;

using Game.Utils;

using UnityEngine;

public class CharacterAnimation : MonoBehaviour {
    [SerializeField] AnimationsSO playerAnimations;

    private GameInputManager input;
    private CharacterComponents components;
    private string currAnimation;
    private string newAnimation;
    private bool isMoving;
    private void Start() {
        input = GameInputManager.Instance;
        components = GetComponent<CharacterComponents>();
    }

    private void Update() {
        HandleMoveAnimation();

        UpdateAnimation();
    }

    private void HandleMoveAnimation() {
        // if (!components.movement.canMove) return;

        isMoving = input.GetMovementVectorNormalized() != Vector2.zero;
        Direction dir = ConvertToEnum(components.movement.faceDir);
        if (!isMoving) {
            switch (dir) {
                case Direction.Right:
                    newAnimation = playerAnimations.IdleRight.name;
                    break;
                case Direction.Left:
                    newAnimation = playerAnimations.IdleLeft.name;
                    break;
                case Direction.Down:
                    newAnimation = playerAnimations.IdleDown.name;
                    break;
                case Direction.Up:
                    newAnimation = playerAnimations.IdleUp.name;
                    break;
            }
        }
        else {
            switch (dir) {
                case Direction.Right:
                    newAnimation = playerAnimations.MoveRight.name;
                    break;
                case Direction.Left:
                    newAnimation = playerAnimations.MoveLeft.name;
                    break;
                case Direction.Down:
                    newAnimation = playerAnimations.MoveDown.name;
                    break;
                case Direction.Up:
                    newAnimation = playerAnimations.MoveUp.name;
                    break;
            }
        }
    }

    public void PlayShootAnimation(float waitTime, Action animDone) {
        components.movement.DisableMovement();
        Direction dir = ConvertToEnum(components.movement.faceDir);
        float animTime = 0;

        switch (dir) {
            case Direction.Right:
                newAnimation = playerAnimations.ShootRight.name;
                animTime = playerAnimations.ShootRight.length;
                break;
            case Direction.Left:
                newAnimation = playerAnimations.ShootLeft.name;
                animTime = playerAnimations.ShootLeft.length;
                break;
            case Direction.Down:
                newAnimation = playerAnimations.ShootDown.name;
                animTime = playerAnimations.ShootDown.length;
                break;
            case Direction.Up:
                newAnimation = playerAnimations.ShootUp.name;
                animTime = playerAnimations.ShootUp.length;
                break;
        }

        FunctionTimer.CreateSceneTimer(() => {
            animDone();
        }, animTime - 0.12f);
        FunctionTimer.CreateSceneTimer(() => {
            components.movement.EnableMovement();
        }, animTime + waitTime);
    }

    private void UpdateAnimation() {
        if (newAnimation != currAnimation) {
            currAnimation = newAnimation;
            components.animator.CrossFade(currAnimation, 0.2f, 0);
        }
    }

    private Direction ConvertToEnum(Vector2 dir) {
        if (dir == Vector2.left) return Direction.Left;
        else if (dir == Vector2.right) return Direction.Right;
        else if (dir == Vector2.down) return Direction.Down;
        else return Direction.Up;
    }
}
