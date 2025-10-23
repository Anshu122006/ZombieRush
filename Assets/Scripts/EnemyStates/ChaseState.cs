using System.Collections;
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
    public ChaseState(EnemyAI enemy, ChaseStateConfig config) : base(enemy) {
        this.config = config;
    }

    public override void Enter() {
        targetDir = Vector2.right;
        aiData = enemy.aiData;
        statsData = enemy.statsData;
        aiData.curState = "chase";

        SetupNoise();
        UpdateTargetDirection();
    }

    public override void Update() {
        UpdateTargetDirection();
        currentSpeed += config.acceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, statsData.BASESPEED * config.speedFactor);
        enemy.rb2d.linearVelocity = targetDir * currentSpeed * Random.Range(0.9f, 1);
    }

    public override void Exit() {
        isExiting = true;
        enemy.rb2d.linearVelocity = Vector2.zero;
        aiData.curDir = Vector2.zero;
        enemy.StartCoroutine(ExitDelay());
    }

    private IEnumerator ExitDelay() {
        yield return new WaitForSeconds(1);
        isExiting = false;
    }

    private void UpdateTargetDirection() {
        Vector2 pos = enemy.transform.position;
        float xNoise = noise.GetNoise(pos.x * 0.3f, pos.y * 0.4f);
        float yNoise = noise.GetNoise((pos.x + 30) * 0.4f, (pos.y + 30) * 0.3f);
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
        aiData.curDir = wanderDir;
    }

    public float[] GetSteering(float[] interest) {
        if (reachedLastTarget) {
            if (TargetOutOfSight()) return interest;
            reachedLastTarget = false;
            aiData.SetCurrentTarget();
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
        if (reachedLastTarget && aiData.GetTargetCount <= 0) {
            aiData.currentTarget = null;
            return true;
        }
        return false;
    }

    private void SetupNoise() {
        int seed = Random.Range(0, 9999);
        noise = new FastNoiseLite(seed);
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);

        noise.SetFrequency(0.04f);                        // moderate frequency → movement changes somewhat slowly
        noise.SetFractalType(FastNoiseLite.FractalType.FBm);
        noise.SetFractalOctaves(2);                       // only a few layers → not super detailed–random
        noise.SetFractalGain(0.45f);                      // moderate fallback to main layer → some randomness but not wild
        noise.SetFractalLacunarity(1.8f);                 // subtle increase in frequency per layer
                                                          // moderate warp amplitude

    }
}
