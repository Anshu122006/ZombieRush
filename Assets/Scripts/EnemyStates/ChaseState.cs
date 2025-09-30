using System.Collections;
using System.Linq;
using Mono.Cecil.Cil;
using UnityEngine;

public class ChaseState : EnemyState {
    private ChaseStateConfig config;

    private AIData aiData;
    private EnemyStatsData statsData;

    private float currentSpeed = 0;
    private bool reachedLastTarget;
    private Vector2 targetDir;
    private Vector2 targetPositionCached;
    private FastNoiseLite noise;

    // Constructor
    public ChaseState(DummyAI enemy, ChaseStateConfig config) : base(enemy) {
        this.config = config;
    }

    public override void Enter() {
        targetDir = Vector2.right;
        aiData = enemy.aiData;
        statsData = enemy.statsData;

        noise = new FastNoiseLite(config.seed);
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);

        Debug.Log("Entered Chase State");
    }

    public override void Update() {
        UpdateTargetDirection();
        currentSpeed += config.acceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, statsData.BASESPEED * config.speedFactor);
        enemy.rb2d.linearVelocity = targetDir * currentSpeed * Random.Range(0.9f, 1);

        Debug.Log(statsData.BASESPEED);
    }

    public override void Exit() {
        enemy.rb2d.linearVelocity = Vector2.zero;
        Debug.Log("Exited Chase State");
    }

    private void UpdateTargetDirection() {
        Vector2 pos = enemy.transform.position;
        float xNoise = noise.GetNoise(pos.x * 0.1f, pos.y * 0.1f);
        float yNoise = noise.GetNoise((pos.x + 10) * 0.1f, (pos.y + 10) * 0.1f);
        targetDir = (targetDir + new Vector2(xNoise, yNoise)).normalized;

        float[] danger = new float[8];
        float[] interest = new float[8];
        danger = WeightCalculator.GetObstacleWeights(danger, aiData, enemy.transform.position, config.collisionRadius, config.chaseRadius);
        interest = GetSteering(interest);

        Vector2 wanderDir = Vector2.zero;
        for (int i = 0; i < 8; i++) {
            float weight = Mathf.Clamp01(interest[i] - danger[i]);
            wanderDir += Directions.directions[i] * weight;
        }
        targetDir = wanderDir.normalized;
    }

    public float[] GetSteering(float[] interest) {
        if (reachedLastTarget) {
            if (TargetOutOfSight()) return interest;
            reachedLastTarget = false;
            aiData.currentTarget = aiData.targets.OrderBy(target =>
                    Vector2.Distance(target.position, enemy.transform.position)).FirstOrDefault();
        }

        if (aiData.currentTarget != null && aiData.targets != null && aiData.targets.Contains(aiData.currentTarget)) {
            targetPositionCached = aiData.currentTarget.position;
        }

        if (Vector2.Distance(enemy.transform.position, targetPositionCached) <= config.targetReachedThreshold) {
            aiData.currentTarget = null;
            reachedLastTarget = true;
            return interest;
        }

        Vector2 dir = targetPositionCached - (Vector2)enemy.transform.position;
        WeightCalculator.GetDirWeights(interest, dir);
        return interest;
    }

    public bool TargetOutOfSight() {
        if (reachedLastTarget && aiData.GetTargetCount() <= 0) {
            aiData.currentTarget = null;
            return true;
        }
        return false;
    }
}
