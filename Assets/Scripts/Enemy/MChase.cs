using Game.Utils;

using UnityEngine;

public class MChase : MovementBase {
    [SerializeField] private MoveState moveState;
    private Transform player;
    private MeleeAttack meleeAttack;
    private FunctionTimer chaseTimer;
    private string chaseTimerName;
    private string waitTimerName;
    private string reachedTimerName;

    [SerializeField] private bool moveBackToCentre;
    [SerializeField] private float speed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float radius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float waitTime;
    [SerializeField] private Transform centre;

    private float updateTime;

    private void Awake() {
        InitBasicComponents();
        waitTimerName = "WaitTimer" + gameObject.GetInstanceID();
        chaseTimerName = "ChaseTimer" + gameObject.GetInstanceID();
        reachedTimerName = "ReachedTimer" + gameObject.GetInstanceID();
        moveState = MoveState.Random;
        agent.maxSpeed = speed;
        updateTime = 0.02f;
    }

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        meleeAttack = GetComponent<MeleeAttack>();
    }

    private void Update() {
        Vector2? pos = CheckForTarget(transform.position, radius, "Player", "Wall", Vector2.right, 120);
        if (pos != null) {
            Debug.Log(pos);
        }
        // switch (moveState) {
        //     case MoveState.Random:
        //         // if (!canMove) return;
        //         MoveRandom(centre.position, radius, waitTime, waitTimerName);
        //         break;
        //     case MoveState.Chasing:
        //         // if (!canMove) return;
        //         MoveToTarget(player, updateTime, ref chaseTimer, chaseTimerName);
        //         break;
        // }

        // UpdateState();
    }

    private void UpdateState() {
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist < chaseRadius && moveState != MoveState.Chasing) {
            agent.maxSpeed = chaseSpeed;
            moveState = MoveState.Chasing;
            seeker.StartPath(transform.position, player.position);
        }
        if (dist >= chaseRadius && moveState != MoveState.Random) {
            agent.maxSpeed = speed;
            moveState = MoveState.Random;
            if (!moveBackToCentre) centre.position = transform.position;
            seeker.StartPath(transform.position, transform.position);
            hasStarted = true;
        }
    }
}
