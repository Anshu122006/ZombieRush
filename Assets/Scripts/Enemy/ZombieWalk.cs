using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

public class ZombieWalk : MonoBehaviour {
    private enum Timer {
        wait,
        slide,
        step,
    }
    private Transform player;
    private Rigidbody2D rb;

    private Coroutine stepCoroutine;
    private Vector2 moveDir;
    private bool slideStarted;
    private Vector2 lastWallNormal = Vector2.zero;
    private float clearTime = 0.1f;

    private Dictionary<Timer, float> elapsed = new Dictionary<Timer, float>();
    [SerializeField] private float speed;
    [SerializeField] private float stepDistance;
    [SerializeField] private float waitTime;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        elapsed[Timer.wait] = 0;
    }
    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
        HandleMovement();
    }

    void OnCollisionStay2D(Collision2D collision) {
        rb.linearVelocity = Vector2.zero;
        if (collision.gameObject.CompareTag("Player")) {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null) {
                Vector2 pushDir = (collision.transform.position - transform.position).normalized;
                playerRb.AddForce(pushDir * 10, ForceMode2D.Force);
            }
        }
    }

    private void HandleMovement() {
        if (stepCoroutine == null) {
            if (elapsed[Timer.wait] <= waitTime) {
                elapsed[Timer.wait] += Time.deltaTime;
            }
            else {
                SetMoveDirection();
                stepCoroutine = StartCoroutine(StepInDir(moveDir));
                elapsed[Timer.wait] = 0;
            }
        }
    }

    private IEnumerator StepInDir(Vector2 dir) {
        float elapsed = 0;
        float delay = stepDistance / speed;
        rb.linearVelocity = Vector2.zero;

        while (elapsed <= delay) {
            elapsed += Time.deltaTime;
            rb.MovePosition((Vector2)transform.position + dir * Time.deltaTime * speed);
            yield return null;
        }

        stepCoroutine = null;
    }


    // Utility functions
    private void SetMoveDirection() {
        float wallClearMargin = 0.8f;
        float normalChangeThreshold = 5f; // degrees tolerance

        Vector2 toPlayer = player.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            toPlayer.normalized,
            toPlayer.magnitude,
            LayerMask.GetMask("Wall")
        );

        if (hit.collider != null && hit.distance < wallClearMargin) {
            float angleDiff = Vector2.Angle(lastWallNormal, hit.normal);

            if (!slideStarted || angleDiff > normalChangeThreshold) {
                Vector2 s = Vector2.Perpendicular(hit.normal).normalized;

                float d1 = ((Vector2)player.position - ((Vector2)transform.position + s)).magnitude;
                float d2 = ((Vector2)player.position - ((Vector2)transform.position - s)).magnitude;

                moveDir = (d1 < d2) ? s : -s;

                slideStarted = true;
                elapsed[Timer.slide] = 0;
                lastWallNormal = hit.normal;
            }
        }
        else {
            if (slideStarted) {
                elapsed[Timer.slide] += Time.deltaTime;
                if (elapsed[Timer.slide] > clearTime) {
                    slideStarted = false;
                    moveDir = toPlayer.normalized;
                    lastWallNormal = Vector2.zero;
                }
                // else keep sliding
            }
            else {
                moveDir = toPlayer.normalized;
            }
        }
    }

}
