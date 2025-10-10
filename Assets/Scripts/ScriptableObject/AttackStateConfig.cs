using UnityEngine;

[CreateAssetMenu(fileName = "AttackStateConfig", menuName = "StateConfigs/AttackStateConfig")]
public class AttackStateConfig : ScriptableObject {
    public float attackOffset = 0.7f;
    public float attackRange = 1;
    public float attackDelay = 0.6f;
    public float specialAttackFactor = 1.6f;
    public AnimationClip clip1;
    public AnimationClip clip2;
}
