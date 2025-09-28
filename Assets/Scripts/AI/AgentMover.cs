using System.Collections;
using UnityEngine;

public class AgentMover : MonoBehaviour {
    private Rigidbody2D rb2d;

    [SerializeField] private float maxSpeed = 3, acceleration = 50, deacceleration = 100;
    [SerializeField] private float currentSpeed = 0;
    [SerializeField] private float moveDuration = 0.4f, waitDuration = 0.4f;
    private Vector2 oldMovementInput;
    public Vector2 MovementInput { get; set; }
    private Coroutine stepCoroutine;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if (MovementInput.magnitude > 0 && currentSpeed >= 0) {
            if (stepCoroutine == null) {
                oldMovementInput = MovementInput;
                stepCoroutine = StartCoroutine(MoveStep());
            }
        }
        else {
            currentSpeed -= deacceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        }
        // currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        // rb2d.linearVelocity = oldMovementInput * currentSpeed;
    }

    private IEnumerator MoveStep() {
        float elapsed = 0;
        while (elapsed < moveDuration) {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
            rb2d.linearVelocity = oldMovementInput * currentSpeed * Random.Range(0.7f, 1);
            elapsed += Time.deltaTime;
            yield return null;
        }
        elapsed = 0;
        while (elapsed < waitDuration) {
            currentSpeed -= deacceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
            rb2d.linearVelocity = oldMovementInput * currentSpeed * Random.Range(0.7f, 1);
            elapsed += Time.deltaTime;
            yield return null;
        }
        stepCoroutine = null;
    }
}