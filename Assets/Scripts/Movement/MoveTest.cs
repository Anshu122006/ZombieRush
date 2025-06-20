using UnityEngine;

public class MoveTest : MonoBehaviour {

    // // Components needed for pathfinding
    // private AIPath agent;
    // private Seeker seeker;
    // private GridGraph grid;
    // private Rigidbody2D rigidBody;

    // // To keep track of current move state
    // public MoveType moveType;
    // private MoveState currState;

    // // Information about the current path
    // private Path currPath;
    // private float pathLength;
    // private bool pathComplete = true;

    // public bool pathFromVectors = false;
    // public bool sameAsPlayerSpeed = true;
    // private Transform player;
    // private Transform currTarget;
    // private Vector3 centre;
    // private int targetIndex = 0;
    // private float updateRate = 0.1f;
    // private float extraSpeed = 0;
    // private float playerSpeed = 0;
    // private FunctionTimer chaseTimer;
    // private FunctionTimer runAwayTimer;
    // private FunctionTimer followTimer;
    // private string waitTimerName;
    // private string traceTimerName;
    // private string chaseTimerName;
    // private string runAwayTimerName;
    // private string followTimerName;

    // // Serializable fields
    // [SerializeField] private List<Transform> targetList;
    // [SerializeField] private List<Vector3> vectorList;
    // [SerializeField] private bool moveBackToOrigin = true;
    // [SerializeField] private float restingSpeed = 2;
    // [SerializeField] private float activeSpeed = 6;
    // [SerializeField] private float restingRadius = 10;
    // [SerializeField] private float activeRadius = 5;
    // [SerializeField] private float waitTime = 1;
    // [SerializeField] private float slowdownDistance = 0.3f;
    // [SerializeField] private float endReachedDistance = 0.3f;
    // [SerializeField] private float separation = 2;

    // private void Start() {
    //     InitBasicComponents();
    //     switch (moveType) {
    //         case MoveType.Random:
    //             currState = MoveState.Random;
    //             break;
    //         case MoveType.Chase:
    //             currState = MoveState.Random;
    //             break;
    //         case MoveType.Patrol:
    //             PopulateTargets();
    //             currState = MoveState.Patrolling;
    //             break;
    //         case MoveType.RunAway:
    //             currState = MoveState.Random;
    //             break;
    //         case MoveType.Follow:
    //             endReachedDistance = separation;
    //             slowdownDistance = separation;
    //             playerSpeed = player.GetComponent<Movement>().GetMoveSpeed();
    //             agent.maxSpeed = sameAsPlayerSpeed ? playerSpeed : restingSpeed;
    //             agent.endReachedDistance = endReachedDistance;
    //             agent.slowdownDistance = slowdownDistance;
    //             currState = MoveState.Following;
    //             break;
    //     }
    // }

    // private void Update() {
    //     switch (moveType) {
    //         case MoveType.Random:
    //             MoveRandom();
    //             break;
    //         case MoveType.Chase:
    //             if (activeRadius > 0 && Distance(transform.position, player.position) < activeRadius)
    //                 ChasePlayer();
    //             else {
    //                 MoveRandom();
    //             }
    //             break;
    //         case MoveType.Patrol:
    //             if (activeRadius > 0 && Distance(transform.position, player.position) < activeRadius)
    //                 ChasePlayer();
    //             else
    //                 Patrol();
    //             break;
    //         case MoveType.RunAway:
    //             if (activeRadius > 0 && Distance(transform.position, player.position) < activeRadius)
    //                 RunAway();
    //             else
    //                 MoveRandom();
    //             break;
    //         case MoveType.Follow:
    //             Follow();
    //             break;
    //     }
    // }

    // private void PopulateTargets() {
    //     float x = transform.position.x;
    //     float y = transform.position.y;
    //     float z = transform.position.z;

    //     if (vectorList == null || vectorList.Count == 0) {
    //         vectorList = new List<Vector3>();
    //         vectorList.Add(transform.position);
    //     }

    //     if (targetList == null || targetList.Count == 0) {
    //         targetList = new List<Transform>();
    //         foreach (Vector3 p in vectorList) {
    //             float dx = p.x;
    //             float dy = p.y;
    //             x += dx;
    //             y += dy;

    //             Transform currTarget = new GameObject("Target${1}").transform;
    //             currTarget.position = new Vector3(x, y, z);
    //             targetList.Add(currTarget);
    //         }
    //     }
    // }

    // private void FindClosestTarget() {
    //     float minDist = float.PositiveInfinity;
    //     int minIndex = 0;

    //     if (targetList == null)
    //         return;

    //     for (int i = 0; i < targetList.Count; i++) {
    //         float dist = Distance(transform.position, targetList[i].position);
    //         if (dist < minDist) {
    //             minDist = dist;
    //             minIndex = i;
    //         }
    //     }

    //     targetIndex = minIndex;
    // }

    // private void MoveRandom(Vector3 centre) {
    // if (currState != MoveState.Random) {
    //     agent.maxSpeed = restingSpeed;
    //     currState = MoveState.Random;
    //     ClearTimers();
    //     if (moveBackToOrigin) {
    //         agent.maxSpeed = restingSpeed;
    //         Vector3 target = centre;
    //         seeker.StartPath(transform.position, target, TracePath);

    //         float followTime = pathLength / restingSpeed;
    //         FunctionTimer.Create(() => {
    //             pathComplete = true;
    //         }, followTime, traceTimerName);
    //     }
    //     else {
    //         centre = transform.position;
    //         pathComplete = true;
    //     }
    // }

    //     if (agent.reachedEndOfPath) {
    //         agent.SetPath(null);
    //         float time = Random.Range(waitTime, waitTime + 2);
    //         FunctionTimer.Create(() => {
    //             GenerateRandomPath();
    //         }, time, waitTimerName);
    //     }
    // }

    // private void ChaseTarget(Transform target) {
    // if (currState != MoveState.Chasing) {
    //     agent.maxSpeed = activeSpeed;
    //     currState = MoveState.Chasing;
    //     ClearTimers();
    //     currTarget = player;
    // }

    //     if (chaseTimer == null || chaseTimer.TimeLeft() < 0) {
    //         chaseTimer = FunctionTimer.Create(() => {
    //             Vector3 targetPos = target.position;
    //             seeker.StartPath(transform.position, targetPos, UpdatePathDate);
    //         }, updateRate, chaseTimerName);
    //     }
    // }

    // private void Patrol() {
    //     if (currState != MoveState.Patrolling) {
    //         agent.maxSpeed = restingSpeed;
    //         currState = MoveState.Patrolling;
    //         ClearTimers();
    //         // FindClosestTarget();
    //         currTarget = targetList[targetIndex];

    //         Vector3 target = targetList[targetIndex].position;
    //         seeker.StartPath(transform.position, target, UpdatePathDate);
    //     }

    //     if (currTarget != targetList[targetIndex]) {
    //         currTarget = targetList[targetIndex];

    //         Vector3 target = targetList[targetIndex].position;
    //         seeker.StartPath(transform.position, target, UpdatePathDate);
    //     }
    //     else if (Distance(transform.position, currTarget.position) < endReachedDistance) {
    //         targetIndex = (targetIndex + 1) % targetList.Count;
    //     }
    // }

    // private void RunAway() {
    //     if (currState != MoveState.RunningAway) {
    //         agent.maxSpeed = activeSpeed;
    //         ClearTimers();
    //         currState = MoveState.RunningAway;
    //     }
    //     if (runAwayTimer == null || runAwayTimer.TimeLeft() < 0) {
    //         runAwayTimer = FunctionTimer.Create(() => {
    //             float dist = Distance(transform.position, currTarget.position);
    //             extraSpeed = 1 / (dist == 0 ? 0.1f : dist);
    //             if (extraSpeed > 4) extraSpeed = 4;

    //             agent.maxSpeed = activeSpeed + extraSpeed;

    //             Vector3 target = GenerateRunAwayTarget();
    //             seeker.StartPath(transform.position, target, UpdatePathDate);
    //         }, updateRate, runAwayTimerName);
    //     }
    // }

    // private void Follow() {
    //     if (currState != MoveState.Following) {
    //         if (sameAsPlayerSpeed && agent.maxSpeed != playerSpeed)
    //             agent.maxSpeed = playerSpeed;
    //         else
    //             agent.maxSpeed = restingSpeed;
    //     }

    //     if (followTimer == null || followTimer.TimeLeft() < 0) {
    //         followTimer = FunctionTimer.Create(() => {
    //             Vector3 target = player.position;
    //             seeker.StartPath(transform.position, target, UpdatePathDate);
    //         }, updateRate, followTimerName);
    //     }
    // }

    // private void InitBasicComponents() {
    //     agent = gameObject.AddComponent<AIPath>();
    //     seeker = GetComponent<Seeker>();
    //     grid = AstarPath.active.data.gridGraph;

    //     player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    //     rigidBody = GetComponent<Rigidbody2D>();

    //     waitTimerName = waitTimerName + gameObject.GetInstanceID();
    //     traceTimerName = waitTimerName + gameObject.GetInstanceID();
    //     chaseTimerName = waitTimerName + gameObject.GetInstanceID();
    //     runAwayTimerName = waitTimerName + gameObject.GetInstanceID();
    //     followTimerName = waitTimerName + gameObject.GetInstanceID();

    //     agent.maxSpeed = restingSpeed;
    //     agent.maxAcceleration = float.PositiveInfinity;
    //     agent.slowdownDistance = slowdownDistance;
    //     agent.endReachedDistance = endReachedDistance;
    //     agent.radius = 1f;
    //     agent.enableRotation = false;
    //     agent.pickNextWaypointDist = 1;
    //     agent.orientation = OrientationMode.YAxisForward;
    //     agent.whenCloseToDestination = CloseToDestinationMode.Stop;
    //     agent.canMove = true;
    //     agent.autoRepath.mode = AutoRepathPolicy.Mode.Never;
    // }

    // private void ClearTimers() {
    //     FunctionTimer.DestroyTimer(waitTimerName);
    //     FunctionTimer.DestroyTimer(traceTimerName);
    //     FunctionTimer.DestroyTimer(chaseTimerName);
    //     FunctionTimer.DestroyTimer(runAwayTimerName);
    //     FunctionTimer.DestroyTimer(followTimerName);
    //     chaseTimer = null;
    //     runAwayTimer = null;
    //     followTimer = null;
    // }
}
