using System.Collections;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class AttackState : EnemyState {
    private AttackStateConfig config;
    private EnemyStatsData statsData;
    private AIData aiData;
    private Coroutine attackCoroutine;

    private float originalMass;
    private float originalDrag;

    public AttackState(EnemyAI enemy, AttackStateConfig config) : base(enemy) {
        this.config = config;
    }

    public override void Enter() {
        statsData = enemy.statsData;
        aiData = enemy.aiData;
        aiData.curState = "attack";

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null) {
            originalMass = rb.mass;
            originalDrag = rb.linearDamping;

            rb.mass = originalMass * 20f;
            rb.linearDamping = originalDrag + 8f;
        }

        Debug.Log("Entered Attack State");
    }

    public override void Update() {
        if (attackCoroutine == null) {
            attackCoroutine = enemy.StartCoroutine(PerformAttack());
            aiData.curDir = Vector2.zero;
        }
        Vector2 dir = (aiData.currentTarget.position - enemy.transform.position).normalized;
        aiData.curDir = dir;
    }

    public override void Exit() {
        isExiting = true;
        if (attackCoroutine == null) enemy.StartCoroutine(ExitDelay());
    }

    private IEnumerator ExitDelay() {
        yield return new WaitForSeconds(0.1f);

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.mass = originalMass;
            rb.linearDamping = originalDrag;
        }

        isExiting = false;
        Debug.Log("Exited Attack State");
    }


    private IEnumerator PerformAttack() {
        if (aiData.currentTarget == null) yield break;
        Vector2 dir = (aiData.currentTarget.position - enemy.transform.position).normalized;

        int attackType = Random.Range(1, 100) < 20 ? 2 : 1;
        aiData.attackPhase = attackType == 1 ? "attack1" : "attack2";

        // Wait for animation to complete
        yield return new WaitForSeconds(attackType == 1 ? config.clip1.length : config.clip2.length);

        Vector2 center = (Vector2)enemy.transform.position + dir * config.attackOffset;
        Vector2 size = new Vector2(1, 2) * config.attackRange * 1.5f;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, Vector2.Angle(Vector2.right, dir), config.targetLayers);

        foreach (var hit in hits) {
            IStatsManager target = hit.GetComponent<IStatsManager>();
            if (target != null) {
                float damageMultiplier = attackType == 2 ? 1.6f : 1f;
                target.TakeDamage((int)(statsData.ATK * damageMultiplier), statsData.LUCK);
            }
        }

        // Wait for attack delay or interruption
        aiData.attackPhase = "waiting";
        float elapsed = 0f;
        Debug.Log(config.attackDelay);
        while (elapsed <= config.attackDelay) {
            if (isExiting) {
                enemy.StartCoroutine(ExitDelay());
                break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        aiData.SetCurrentTarget();
        attackCoroutine = null;
    }


    public bool TargetInAttackRange() {
        if (aiData.currentTarget == null) return false;
        float dist = Vector2.Distance(aiData.currentTarget.position, enemy.transform.position);
        return dist < config.attackRange;
    }
}
