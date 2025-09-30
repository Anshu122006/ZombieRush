using System;

[System.Serializable]
public class StateTransition {
    public EnemyState TargetState;
    public Func<bool> Condition;

    public StateTransition(EnemyState target, Func<bool> condition) {
        TargetState = target;
        Condition = condition;
    }
}
