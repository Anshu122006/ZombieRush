using System.Collections;
using UnityEngine;

public class DeathState : EnemyState {
    private DeathStateConfig config;
    private AIData aiData;
    private Rigidbody2D rb;

    public DeathState(EnemyAI enemy, DeathStateConfig config) : base(enemy) {
        this.config = config;
    }

    public override void Enter() {
        aiData = enemy.aiData;
        rb = enemy.GetComponent<Rigidbody2D>();
    }

    public override void Update() {
        if (rb.linearVelocity.magnitude > 0.3f) rb.linearVelocity = Vector2.zero;
        aiData.curDir = Vector2.zero;
    }

    public override void Exit() {
        isExiting = true;
    }
}
