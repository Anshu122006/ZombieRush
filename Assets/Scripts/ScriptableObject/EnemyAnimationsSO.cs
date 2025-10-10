using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAnimationsSO", menuName = "Animations/Enemy")]
public class EnemyAnimationsSO : ScriptableObject {
    public DirectionAnimation idle;
    public DirectionAnimation walk;
    public DirectionAnimation attack1;
    public DirectionAnimation attack2;
    public AnimationClip deathLeft;
    public AnimationClip deathRight;
}
