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

        noise = new FastNoiseLite(config.seed);
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);

        Debug.Log("Entered Wander State");
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
        Debug.Log("Exited Wander State");
    }

    private IEnumerator ExitDelay() {
        yield return new WaitForSeconds(0.1f);
        isExiting = false;
    }

    // To update the direction the zombies is currently pointing to
    private void UpdateTargetDirection() {
        Vector2 pos = enemy.transform.position;
        float xNoise = noise.GetNoise(pos.x * 0.3f, pos.y * 0.3f);
        float yNoise = noise.GetNoise((pos.x + 10) * 0.3f, (pos.y + 10) * 0.3f);
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
}
