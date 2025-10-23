using UnityEngine;

[CreateAssetMenu(fileName = "AttackStateConfig", menuName = "StateConfigs/AttackStateConfig")]
public class AttackStateConfig : ScriptableObject {
    public float attackOffset = 0.3f;
    public float attackRange = 0.7f;
    public float attackDelay = 1f;
    public float specialAttackFactor = 1.6f;
    public AnimationClip clip1;
    public AnimationClip clip2;
    public LayerMask targetLayers;
}
