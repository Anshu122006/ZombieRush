using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EnemyState {
    public List<StateTransition> transitions;
    public bool isExiting;
    protected EnemyAI enemy;

    public EnemyState(EnemyAI enemy) {
        this.enemy = enemy;
    }
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
