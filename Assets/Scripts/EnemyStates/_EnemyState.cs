using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EnemyState {
    public List<StateTransition> transitions;
    protected DummyAI enemy;

    public EnemyState(DummyAI enemy) {
        this.enemy = enemy;
    }
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
