using System;

using Game.Utils;

using UnityEngine;

public class CharacterAnimation : MonoBehaviour {
    [SerializeField] private CharacterAnimationsSO playerAnimations;

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
        HandleWalkAnimation();

        UpdateAnimation();
    }

    private void HandleWalkAnimation() {
        // if (!components.movement.canWalk) return;

        isMoving = input.GetMovementVectorNormalized() != Vector2.zero;
        Vector2 dir = components.movement.faceDir;
        if (!isMoving) {
            if (dir == Vector2.right) {
                newAnimation = playerAnimations.IdleRight.name;
            }
            else if (dir == Vector2.left) {
                newAnimation = playerAnimations.IdleLeft.name;
            }
            else if (dir == Vector2.up) {
                newAnimation = playerAnimations.IdleUp.name;
            }
            else if (dir == Vector2.down) {
                newAnimation = playerAnimations.IdleDown.name;
            }
        }
        else {
            if (dir == Vector2.right) {
                newAnimation = playerAnimations.WalkRight.name;
            }
            else if (dir == Vector2.left) {
                newAnimation = playerAnimations.WalkLeft.name;
            }
            else if (dir == Vector2.up) {
                newAnimation = playerAnimations.WalkUp.name;
            }
            else if (dir == Vector2.down) {
                newAnimation = playerAnimations.WalkDown.name;
            }
        }
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
