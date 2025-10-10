using System;
using System.Collections.Generic;
using Game.Utils;

using UnityEngine;

public class CharacterAnimation : MonoBehaviour {
    [SerializeField] private CharacterAnimationsSO playerAnimations;
    [SerializeField] private float baseMoveAnimationSpeed = 6;

    private Dictionary<string, DirectionAnimation> anims = new();
    private CharacterComponents components;
    private string curAnimation;
    private string newAnimation;

    private void Start() {
        components = GetComponent<CharacterComponents>();
        anims["pistol"] = playerAnimations.pistol;
        anims["smg"] = playerAnimations.smg;
        anims["shotgun"] = playerAnimations.shotgun;
        anims["grenade"] = playerAnimations.grenade;
        anims["minigun"] = playerAnimations.minigun;
        anims["flamethrower"] = playerAnimations.flamethrower;
    }

    private void Update() {
        HandleWalkAnimation();
        UpdateAnimation();
    }

    private void HandleWalkAnimation() {
        Vector2 dir = components.movement.faceDir;
        string gunName = components.gunHandler.Gun.Name;

        AnimationClip clip = anims[gunName].down;
        if (dir == Vector2.up) clip = anims[gunName].up;
        else if (dir == Vector2.right) clip = anims[gunName].right;
        else if (dir == Vector2.left) clip = anims[gunName].left;

        CharacterMovement move = components.movement;
        components.animator.speed = Mathf.Clamp(move.Speed / baseMoveAnimationSpeed, 0.8f, 1.4f);
        newAnimation = clip.name;
    }

    private void UpdateAnimation() {
        if (newAnimation != curAnimation) {
            curAnimation = newAnimation;
            components.animator.CrossFade(curAnimation, 0.2f, 0);
        }
    }
}
