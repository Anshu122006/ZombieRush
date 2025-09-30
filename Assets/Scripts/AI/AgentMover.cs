using System.Collections;
using UnityEngine;

public class AgentMover : MonoBehaviour {
    private Rigidbody2D rb2d;

    [SerializeField] private float radius = 6;
    [SerializeField] private AIData aiData;
    private Vector2 center;
    private Vector2 wanderDir = Vector2.right;

    [SerializeField] private float maxSpeed = 1, acceleration = 50, deacceleration = 100;
    [SerializeField] private float currentSpeed = 0;
    [SerializeField] private float moveDuration = 0.2f, waitDuration = 0.4f;
    private Vector2 oldMovementInput;
    public Vector2 MovementInput { get; set; }
    private Coroutine stepCoroutine;

    public int seed = 12345;
    private FastNoiseLite noise;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        center = transform.position;
        noise = new FastNoiseLite(seed);
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
    }

    private void FixedUpdate() {
        // if (MovementInput.magnitude > 0 && currentSpeed >= 0) {
        if (stepCoroutine == null) {
            // wanderDir = CalculateFinalDirection();
            oldMovementInput = wanderDir;
            // stepCoroutine = StartCoroutine(MoveStep());
        }
        // }
        // else {
        //     currentSpeed -= deacceleration * Time.deltaTime;
        //     currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        // }
    }

    // private void OnDrawGizmosSelected() {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(center, radius);
    //     Gizmos.DrawRay(transform.position, wanderDir);

    // }

}