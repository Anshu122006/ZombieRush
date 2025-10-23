using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    // FInite State Machine
    public EnemyFSM FSM { get; private set; }
    public WanderState wanderState { get; private set; }
    public ChaseState chaseState { get; private set; }
    public AttackState attackState { get; private set; }
    public DeathState deathState { get; private set; }

    // Data required by the states
    public Rigidbody2D rb2d;
    public AIData aiData;
    public EnemyStatsData statsData;
    public EnemyStatsManager statsManager;

    //Configs for each state
    [SerializeField] private WanderStateConfig wanderStateConfig;
    [SerializeField] private ChaseStateConfig chaseStateConfig;
    [SerializeField] private AttackStateConfig attackStateConfig;
    [SerializeField] private DeathStateConfig deathStateConfig;

    // Detection delay
    [SerializeField] private List<Detector> detectors;
    [SerializeField] private float detectionDelay = 0.05f;

    private bool isHit;

    private void Awake() {
        FSM = new EnemyFSM();

        wanderState = new WanderState(this, wanderStateConfig);
        chaseState = new ChaseState(this, chaseStateConfig);
        attackState = new AttackState(this, attackStateConfig);
        deathState = new DeathState(this, deathStateConfig);

        chaseState.transitions = new List<StateTransition>{
            new StateTransition(wanderState, () => !CanChase()),
            new StateTransition(attackState, () => CanAttack()),
            new StateTransition(deathState, () => IsDead())
        };

        wanderState.transitions = new List<StateTransition>{
            new StateTransition(chaseState, () => CanChase()),
            new StateTransition(deathState, () => IsDead())
        };

        attackState.transitions = new List<StateTransition>{
            new StateTransition(chaseState, () => CanChase() && !CanAttack()),
            new StateTransition(wanderState, () => !CanChase() && !CanAttack()),
            new StateTransition(deathState, () => IsDead())
        };

        deathState.transitions = new List<StateTransition>{
            new StateTransition(wanderState, () => false),
        };

        if (CanChase()) FSM.SetState(chaseState);
        else FSM.SetState(wanderState);
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    private void Update() {
        FSM.Update();
    }

    private void PerformDetection() {
        foreach (Detector detector in detectors)
            detector.Detect(aiData);

        // Debug.Log(CanChase() && !CanAttack());
        // if (CanAttack()) Debug.Log("Can Attack");
        // if (CanChase()) Debug.Log("Can Chase");
    }

    public bool CanChase() {
        aiData.SetCurrentTarget();
        return aiData.GetTargetCount > 0 || aiData.currentTarget != null || isHit;
    }
    public bool CanAttack() {
        if (aiData.currentTarget == null) return false;

        Collider2D col = GetComponent<Collider2D>();
        if (col == null) return false;

        Vector2 closestPoint = col.ClosestPoint(aiData.currentTarget.position);
        float dist = Vector2.Distance(closestPoint, aiData.currentTarget.position);
        return CanChase() && dist < attackStateConfig.attackRange;
    }

    public bool IsDead() {
        return statsData.hp <= 0;
    }

    public void SetIsHit(Transform target) {
        if (target == null) return;
        if (aiData.curState == "wander" && !isHit) {
            aiData.currentTarget = target;
            StartCoroutine(ResetHit(target));
            isHit = true;
        }
    }

    private IEnumerator ResetHit(Transform target) {
        Vector2 closestPoint = target.GetComponent<Collider2D>().ClosestPoint(aiData.currentTarget.position);
        yield return new WaitUntil(() => Vector2.Distance(closestPoint, transform.position) < chaseStateConfig.chaseRadius);
        isHit = false;
    }
}
