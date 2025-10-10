using UnityEngine;

public class EnemyFSM {
    private EnemyState currentState;
    private EnemyState targetState;

    public void SetState(EnemyState newState) {
        currentState = newState;
        targetState = null;
        currentState.Enter();
    }

    public void Update() {
        if (currentState == null) return;
        if (targetState != null && currentState.isExiting) return;
        else if (targetState != null && !currentState.isExiting) SetState(targetState);
        Debug.Log(currentState.isExiting);

        // Evaluate transitions
        foreach (var t in currentState.transitions) {
            if (t.Condition()) {
                targetState = t.TargetState;
                currentState.Exit();
                return;
            }
        }
        currentState.Update();
    }
}
