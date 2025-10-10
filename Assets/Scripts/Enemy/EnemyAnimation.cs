using System.Collections.Generic;
using Game.Utils;

using UnityEngine;

public class EnemyAnimation : MonoBehaviour {
    [SerializeField] private AIData aiData;
    [SerializeField] private EnemyAnimationsSO enemyAnimation;
    [SerializeField] private Animator animator;

    private Dictionary<string, DirectionAnimation> anims = new();
    private Vector2 prevDir;
    private string currAnimation;
    private string newAnimation;

    private void Start() {
        anims["idle"] = enemyAnimation.idle;
        anims["walk"] = enemyAnimation.walk;
        anims["attack1"] = enemyAnimation.attack1;
        anims["attack2"] = enemyAnimation.attack2;
    }

    private void Update() {
        HandleMoveAnimation();
        UpdateAnimation();
    }

    private void HandleMoveAnimation() {
        Vector2 dir = aiData.curDir;
        if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x)) dir = new Vector2(0, dir.y).normalized;
        else dir = new Vector2(dir.x, 0).normalized;

        string curState = aiData.curState;
        if (curState != "attack") {
            if (dir == Vector2.zero) {
                AnimationClip clip = GetAnimationClip(prevDir, "idle");
                newAnimation = clip.name;
            }
            else {
                prevDir = dir;
                AnimationClip clip = GetAnimationClip(prevDir, "walk");
                newAnimation = clip.name;
            }
        }
        else {
            prevDir = dir;
            switch (aiData.attackPhase) {
                case "attack1":
                    AnimationClip clip = GetAnimationClip(prevDir, "attack1");
                    newAnimation = clip.name;
                    aiData.attackPhase = "running";
                    break;
                case "attack2":
                    clip = GetAnimationClip(prevDir, "attack2");
                    newAnimation = clip.name;
                    aiData.attackPhase = "running";
                    break;
                case "waiting":
                    clip = GetAnimationClip(prevDir, "idle");
                    newAnimation = clip.name;
                    break;
            }
        }
    }

    private void UpdateAnimation() {
        if (newAnimation != currAnimation) {
            currAnimation = newAnimation;
            animator.CrossFade(currAnimation, 0.1f);
        }
    }

    private AnimationClip GetAnimationClip(Vector2 dir, string animationName) {
        if (dir == Vector2.up) return anims[animationName].up;
        else if (dir == Vector2.down) return anims[animationName].down;
        else if (dir == Vector2.left) return anims[animationName].left;
        else return anims[animationName].right;
    }
}
