using UnityEngine;

[CreateAssetMenu(fileName = "WanderStateConfig", menuName = "StateConfigs/WanderStateConfig")]
public class WanderStateConfig : ScriptableObject {
    public float speedFactor = 0.8f;
    public float wanderRadius = 6;
    public float collisionRadius = 1;
    public float acceleration = 50;
    public float deacceleration = 100;
    public float moveDuration = 0.2f;
    public float waitDuration = 0.4f;
    public int seed = 12345;
}

