using System;
using System.Collections.Generic;
using Game.Utils;

using UnityEngine;

public class CharacterAnimation : MonoBehaviour {
    [SerializeField] private CharacterAnimationsSO playerAnimations;
    [SerializeField] private float baseMoveAnimationSpeed = 6;

    private Dictionary<string, DirectionAnimation> idle = new();
    private Dictionary<string, DirectionAnimation> walk = new();
    private CharacterComponents components;
    private string curAnimation;
    private string newAnimation;

    private void Start() {
        components = GetComponent<CharacterComponents>();
        idle = playerAnimations.GetIdleAnimations();
        walk = playerAnimations.GetWalkAnimations();
    }

    private void Update() {
        HandleWalkAnimation();
        UpdateAnimation();
    }

    public void PlayDeathAnimation() {
        Vector2 dir = components.movement.faceDir;
        if (dir.x < 0 || dir.y > 0) newAnimation = playerAnimations.death_left.name;
        else newAnimation = playerAnimations.death_right.name;
    }

    private void HandleWalkAnimation() {
        if (components.statsManager.InReviveStage) return;

        bool isIdle = GameInputManager.Instance.GetInputDir() == Vector2.zero;
        Vector2 dir = components.movement.faceDir;
        string gunName = components.gunHandler.Gun.Name;

        if (isIdle) {
            AnimationClip clip = idle[gunName].down;
            if (dir == Vector2.up) clip = idle[gunName].up;
            else if (dir == Vector2.right) clip = idle[gunName].right;
            else if (dir == Vector2.left) clip = idle[gunName].left;

            newAnimation = clip.name;
        }
        else {
            AnimationClip clip = walk[gunName].down;
            if (dir == Vector2.up) clip = walk[gunName].up;
            else if (dir == Vector2.right) clip = walk[gunName].right;
            else if (dir == Vector2.left) clip = walk[gunName].left;

            CharacterMovement move = components.movement;
            components.animator.speed = Mathf.Clamp(move.Speed / baseMoveAnimationSpeed, 0.8f, 1.4f);
            newAnimation = clip.name;
        }
    }

    private void UpdateAnimation() {
        if (newAnimation != curAnimation) {
            // bool isShooting = GameInputManager.Instance.IsShooting();
            // bool isIdle = GameInputManager.Instance.GetInputDir() == Vector2.zero;

            curAnimation = newAnimation;
            components.animator.CrossFade(curAnimation, 0f, 0);

            // if (isIdle && isShooting) components.animator.speed = 0;
            // else components.animator.speed = 1;
        }
    }
}
