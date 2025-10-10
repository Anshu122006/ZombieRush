using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    // FInite State Machine
    public EnemyFSM FSM { get; private set; }
    public WanderState wanderState { get; private set; }
    public ChaseState chaseState { get; private set; }
    public AttackState attackState { get; private set; }

    // Data required by the states
    public Rigidbody2D rb2d;
    public AIData aiData;
    public EnemyStatsData statsData;
    public EnemyStatsManager statsManager;

    //Configs for each state
    [SerializeField] private WanderStateConfig wanderStateConfig;
    [SerializeField] private ChaseStateConfig chaseStateConfig;
    [SerializeField] private AttackStateConfig attackStateConfig;

    // Detection delay
    [SerializeField] private List<Detector> detectors;
    [SerializeField] private float detectionDelay = 0.05f;


    private void Awake() {
        FSM = new EnemyFSM();

        wanderState = new WanderState(this, wanderStateConfig);
        chaseState = new ChaseState(this, chaseStateConfig);
        attackState = new AttackState(this, attackStateConfig);

        chaseState.transitions = new List<StateTransition>{
            new StateTransition(wanderState, () => !CanChase()),
            new StateTransition(attackState, () => CanChase() && CanAttack())
        };

        wanderState.transitions = new List<StateTransition>{
            new StateTransition(chaseState, () => CanChase())
        };

        attackState.transitions = new List<StateTransition>{
            new StateTransition(chaseState, () => CanChase() && !CanAttack()),
            new StateTransition(wanderState, () => !CanChase() && !CanAttack())
        };

        FSM.SetState(wanderState);
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    private void Update() {
        FSM.Update();
    }

    private void PerformDetection() {
        foreach (Detector detector in detectors) {
            detector.Detect(aiData);
        }
    }

    public bool CanChase() => aiData.GetTargetCount() > 0 || aiData.currentTarget != null;
    public bool CanAttack() {
        if (aiData.currentTarget == null) return false;
        float dist = Vector2.Distance(aiData.currentTarget.position, transform.position);
        return dist < attackStateConfig.attackRange;
    }
}
