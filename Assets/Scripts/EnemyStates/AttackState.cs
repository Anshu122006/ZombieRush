using System.Collections;
using UnityEngine;

public class AttackState : EnemyState {
    private AttackStateConfig config;
    private EnemyStatsData statsData;
    private EnemyStatsManager statsManager;
    private AIData aiData;
    private Coroutine attackCoroutine;

    private bool isAttacking;
    public AttackState(EnemyAI enemy, AttackStateConfig config) : base(enemy) {
        this.config = config;
    }

    public override void Enter() {
        statsData = enemy.statsData;
        statsManager = enemy.statsManager;
        aiData = enemy.aiData;
        aiData.curState = "attack";
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
        Debug.Log("Exited Attack State");
    }

    private IEnumerator ExitDelay() {
        yield return new WaitForSeconds(0.1f);
        isExiting = false;
    }


    private IEnumerator PerformAttack() {
        if (aiData.currentTarget == null) yield return null;
        Vector2 dir = (aiData.currentTarget.position - enemy.transform.position).normalized;

        isAttacking = true;
        int attackType = 1;
        int chance = Random.Range(1, 100);
        if (chance < 20) attackType = 2;
        aiData.attackPhase = attackType == 1 ? "attack1" : "attack2";

        // wait for animation to complete
        yield return new WaitForSeconds(attackType == 1 ? config.clip1.length : config.clip2.length);


        // check area infront of the enemy
        Vector2 center = (Vector2)enemy.transform.position + dir * config.attackOffset;
        Vector2 size = new Vector2(1, 2);
        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, Vector2.Angle(Vector2.right, dir), LayerMask.GetMask("Player"));
        foreach (var hit in hits) {
            IStatsManager target = hit.GetComponent<IStatsManager>();
            if (target != null) {
                target.TakeDamage((int)(statsData.ATK * (attackType == 2 ? 1.6f : 1)), statsData.LUCK);
            }
        }
        aiData.attackPhase = "waiting";

        // wait for some time
        float elapsed = 0;
        while (elapsed <= config.attackDelay) {
            if (isExiting) {
                enemy.StartCoroutine(ExitDelay());
                break;
            }
            else {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
        Debug.Log("Attacked");
        attackCoroutine = null;
    }

    public bool TargetInAttackRange() {
        if (aiData.currentTarget == null) return false;
        float dist = Vector2.Distance(aiData.currentTarget.position, enemy.transform.position);
        return dist < config.attackRange;
    }
}
