using UnityEngine;

[CreateAssetMenu(fileName = "AttackStateConfig", menuName = "StateConfigs/AttackStateConfig")]
public class AttackStateConfig : ScriptableObject {
    public float attackOffset = 0.7f;
    public float attackRange = 1;
}
