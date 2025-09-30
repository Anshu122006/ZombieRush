using UnityEngine;

[CreateAssetMenu(fileName = "ChaseStateConfig", menuName = "StateConfigs/ChaseStateConfig")]
public class ChaseStateConfig : ScriptableObject {
    public float speedFactor = 1.2f;
    public float chaseRadius = 3;
    public float collisionRadius = 0.5f;
    public float targetReachedThreshold = 1f;
    public float acceleration = 50;
    public float deacceleration = 100;
    public int seed = 12345;
}
