using System.Collections.Generic;

public class EnemyFSM {
    private EnemyState currentState;
    private List<StateTransition> transitions = new List<StateTransition>();

    public void SetState(EnemyState newState, List<StateTransition> newTransitions = null) {
        currentState?.Exit();

        currentState = newState;
        transitions = newTransitions ?? newState.transitions;
        currentState.Enter();
    }

    public void Update() {
        if (currentState == null) return;

        // Evaluate transitions
        foreach (var t in transitions) {
            if (t.Condition()) {
                SetState(t.TargetState);
                return;
            }
        }

        currentState.Update();
    }
}
