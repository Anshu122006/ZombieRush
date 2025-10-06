using UnityEngine;

[CreateAssetMenu(fileName = "AnimationsSO", menuName = "Animations/Character")]
public class CharacterAnimationsSO : ScriptableObject {
    public AnimationClip IdleDown;
    public AnimationClip IdleLeft;
    public AnimationClip IdleRight;
    public AnimationClip IdleUp;

    public AnimationClip WalkDown;
    public AnimationClip WalkLeft;
    public AnimationClip WalkRight;
    public AnimationClip WalkUp;

    public AnimationClip RunDown;
    public AnimationClip RunLeft;
    public AnimationClip RunRight;
    public AnimationClip RunUp;
}
