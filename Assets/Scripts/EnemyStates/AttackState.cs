using System.Collections;
using UnityEngine;

public class AttackState : EnemyState {
    private AttackStateConfig config;
    private EnemyStatsData statsData;
    private EnemyStatsManager statsManager;
    private AIData aiData;
    private Coroutine attackCoroutine;
    public AttackState(DummyAI enemy, AttackStateConfig config) : base(enemy) {
        this.config = config;
    }

    public override void Enter() {
        statsData = enemy.statsData;
        statsManager = enemy.statsManager;
        aiData = enemy.aiData;
        Debug.Log("Entered Attack State");
    }

    public override void Update() {
        if (attackCoroutine == null) {
            attackCoroutine = enemy.StartCoroutine(PerformAttack());
        }
    }

    public override void Exit() {
        Debug.Log("Exited Attack State");
    }


    private IEnumerator PerformAttack() {
        if (aiData.currentTarget == null) yield return null;
        Vector2 dir = (aiData.currentTarget.position - enemy.transform.position).normalized;

        // wait for animation to complete
        yield return new WaitForSeconds(1);

        // check area infront of the enemy
        Vector2 center = (Vector2)enemy.transform.position + dir * config.attackOffset;
        Vector2 size = new Vector2(1, 2);
        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, Vector2.Angle(Vector2.right, dir), LayerMask.GetMask("Player"));
        foreach (var hit in hits) {
            IStatsManager target = hit.GetComponent<IStatsManager>();
            if (target != null) {
                target.TakeDamage(statsData.ATK, statsManager);
            }
        }

        // wait for some time
        Debug.Log("Attacked");
        attackCoroutine = null;
    }

    public bool TargetInAttackRange() {
        if (aiData.currentTarget == null) return false;
        float dist = Vector2.Distance(aiData.currentTarget.position, enemy.transform.position);
        return dist < config.attackRange;
    }
}
