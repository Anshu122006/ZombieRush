using System.Collections;
using UnityEngine;

public class WanderState : EnemyState {
    private WanderStateConfig config;

    private AIData aiData;
    private EnemyStatsData statsData;

    private float currentSpeed = 0;
    private Vector2 center;
    private Vector2 targetDir;
    private Coroutine stepCoroutine;
    private FastNoiseLite noise;

    // Constructor
    public WanderState(EnemyAI enemy, WanderStateConfig config) : base(enemy) {
        this.config = config;
    }

    public override void Enter() {
        center = enemy.transform.position;
        targetDir = Vector2.right;
        aiData = enemy.aiData;
        statsData = enemy.statsData;
        aiData.curState = "wander";

        SetupNoise();
    }

    public override void Update() {
        if (stepCoroutine == null) {
            UpdateTargetDirection();
            stepCoroutine = enemy.StartCoroutine(MoveStep());
        }
    }

    public override void Exit() {
        isExiting = true;
        if (stepCoroutine != null) {
            enemy.StopCoroutine(stepCoroutine);
            stepCoroutine = null;
        }
        enemy.StartCoroutine(ExitDelay());
    }

    private IEnumerator ExitDelay() {
        yield return new WaitForSeconds(0);
        isExiting = false;
    }

    // To update the direction the zombies is currently pointing to
    private void UpdateTargetDirection() {
        Vector2 pos = enemy.transform.position;
        float xNoise = noise.GetNoise(pos.x * 0.6f, pos.y * 0.8f);
        float yNoise = noise.GetNoise((pos.x + 30) * 0.8f, (pos.y + 30) * 0.6f);
        targetDir = (targetDir + new Vector2(xNoise, yNoise)).normalized;

        float[] danger = new float[8];
        float[] interest = new float[8];
        Vector2 rangeVec = (Vector2)enemy.transform.position - center;
        danger = WeightCalculator.GetObstacleWeights(danger, aiData, enemy.transform.position, config.collisionRadius, config.wanderRadius);
        interest = WeightCalculator.GetDirWeightInRange(interest, targetDir, rangeVec.normalized, config.wanderRadius, rangeVec.magnitude);

        Vector2 wanderDir = Vector2.zero;
        for (int i = 0; i < 8; i++) {
            float weight = Mathf.Clamp01(interest[i] - danger[i]);
            wanderDir += Directions.directions[i] * weight;
        }
        targetDir = wanderDir.normalized;
        aiData.curDir = wanderDir;
    }

    // To move in steps like a zombie
    private IEnumerator MoveStep() {
        float elapsed = 0;
        while (elapsed < config.moveDuration) {
            currentSpeed += config.acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, statsData.BASESPEED * config.speedFactor);
            enemy.rb2d.linearVelocity = targetDir * currentSpeed * Random.Range(0.7f, 1);
            elapsed += Time.deltaTime;
            yield return null;
        }
        elapsed = 0;
        while (elapsed < config.waitDuration) {
            currentSpeed -= config.deacceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, statsData.BASESPEED * config.speedFactor);
            enemy.rb2d.linearVelocity = targetDir * currentSpeed * Random.Range(0.7f, 1);
            elapsed += Time.deltaTime;
            yield return null;
        }
        stepCoroutine = null;
    }

    private void SetupNoise() {
        int seed = Random.Range(0, 9999);
        noise = new FastNoiseLite(seed);
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);

        noise.SetFrequency(0.02f);                   // quite low → slow large changes
        noise.SetFractalType(FastNoiseLite.FractalType.FBm);
        noise.SetFractalOctaves(2);                  // few layers → rougher, simpler motion
        noise.SetFractalGain(0.4f);                  // lower amplitude for finer layers → less jitter
        noise.SetFractalLacunarity(1.8f);            // moderate frequency increase per octave 
        noise.SetFractalWeightedStrength(0.0f);
    }
}
