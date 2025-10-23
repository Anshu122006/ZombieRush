using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {
    [SerializeField] private GameObject explosionPref;
    [SerializeField] private float distanceFromTargetToDestroy = 1f;
    [SerializeField] private float explosionRadius = 2;
    [SerializeField] private LayerMask targetLayers;

    private int damage;
    private int accuracy;

    public Vector2 start;
    public Vector2 targetPos;
    private float moveSpeed;
    private float maxMoveSpeed;
    private float maxTrajectoryHeight;

    private AnimationCurve trajectoryAnimationCurve;
    private AnimationCurve axisCorrectionAnimationCurve;
    private AnimationCurve speedAnimationCurve;

    public Vector2 trajectoryRange;
    private Vector2 moveDir;

    private float nextYPos;
    private float nextXPos;
    private float nextYPosCorrectionAbsolute;
    private float nextXPosCorrectionAbsolute;
    private Action<int> AddExp;

    private void OnCollisionEnter2D(Collision2D hit) {
        DestroySelf();
    }

    private void Start() {
        start = transform.position;
    }

    private void Update() {
        UpdatePosition();
        if (Vector2.Distance(transform.position, targetPos) < distanceFromTargetToDestroy) DestroySelf();
    }

    private void UpdatePosition() {
        trajectoryRange = targetPos - start;

        if (Mathf.Abs(trajectoryRange.normalized.x) < Mathf.Abs(trajectoryRange.normalized.y)) {
            // Curved along X axis
            if (trajectoryRange.y < 0) {
                moveSpeed = -moveSpeed;
            }
            UpdatePositionWithXCurve();
        }
        else {
            // Curved along Y axis
            if (trajectoryRange.x < 0) {
                moveSpeed = -moveSpeed;
            }
            UpdatePositionWithYCurve();
        }
    }

    private void UpdatePositionWithXCurve() {
        float nextPosY = transform.position.y + moveSpeed * Time.deltaTime;
        float nextPosYNormalized = (nextPosY - start.y) / trajectoryRange.y;

        float nextPosXNormalized = trajectoryAnimationCurve.Evaluate(nextPosYNormalized);
        nextXPos = nextPosXNormalized * maxTrajectoryHeight;

        float nextPosXCorrectionNormalized = axisCorrectionAnimationCurve.Evaluate(nextPosYNormalized);
        nextXPosCorrectionAbsolute = nextPosXCorrectionNormalized * trajectoryRange.x;

        if (trajectoryRange.x > 0 && trajectoryRange.y > 0) nextXPos = -nextXPos;
        if (trajectoryRange.x < 0 && trajectoryRange.y < 0) nextXPos = -nextXPos;

        float nextPosX = start.x + nextXPos + nextXPosCorrectionAbsolute;
        Vector2 newPos = new Vector2(nextPosX, nextPosY);

        CalculateNextMoveSpeed(nextPosYNormalized);
        moveDir = newPos - (Vector2)transform.position;

        transform.position = newPos;
    }

    private void UpdatePositionWithYCurve() {
        float nextPosX = transform.position.x + moveSpeed * Time.deltaTime;
        float nextPosXNormalized = (nextPosX - start.x) / trajectoryRange.x;

        float nextPosYNormalized = trajectoryAnimationCurve.Evaluate(nextPosXNormalized);
        nextYPos = nextPosYNormalized * maxTrajectoryHeight;

        float nextPosYCorrectionNormalized = axisCorrectionAnimationCurve.Evaluate(nextPosXNormalized);
        nextYPosCorrectionAbsolute = nextPosYCorrectionNormalized * trajectoryRange.y;

        float nextPosY = start.y + nextYPos + nextYPosCorrectionAbsolute;
        Vector2 newPos = new Vector2(nextPosX, nextPosY);

        CalculateNextMoveSpeed(nextPosXNormalized);
        moveDir = newPos - (Vector2)transform.position;

        transform.position = newPos;
    }

    private void CalculateNextMoveSpeed(float nextPosNormalized) {
        float nextMoveSpeedNormalized = speedAnimationCurve.Evaluate(nextPosNormalized);
        moveSpeed = nextMoveSpeedNormalized * maxMoveSpeed;
    }

    private void DestroySelf() {
        Debug.Log("destroy called");
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayers);
        foreach (var hit in hits) {
            IStatsManager enemy = hit.GetComponent<IStatsManager>();
            if (enemy != null) {
                enemy.HandleHitEffects();
                enemy.TakeDamage(damage, accuracy, out int expDrop);
                AddExp?.Invoke(expDrop);
            }
        }

        GrenadeExplosion explosion = Instantiate(explosionPref, transform.position, Quaternion.identity).GetComponent<GrenadeExplosion>();
        explosion.Initialize();
        Destroy(gameObject);
    }

    public void InitializeProperties(Vector2 targetPos, float maxMoveSpeed, float baseTrajectoryHeight, int damage, int accuracy, float explosionRadius, Action<int> AddExp) {
        this.targetPos = targetPos;
        this.maxMoveSpeed = maxMoveSpeed;
        this.start = transform.position;
        this.damage = damage;
        this.accuracy = accuracy;
        this.explosionRadius = explosionRadius;
        this.AddExp = AddExp;

        float xDistanceToTarget = targetPos.x - transform.position.x;
        this.maxTrajectoryHeight = Mathf.Abs(xDistanceToTarget) * baseTrajectoryHeight;
    }

    public void InitializeAnimationCurves(AnimationCurve trajectoryAnimationCurve, AnimationCurve axisCorrectionAnimationCurve, AnimationCurve speedAnimationCurve) {
        this.trajectoryAnimationCurve = trajectoryAnimationCurve;
        this.axisCorrectionAnimationCurve = axisCorrectionAnimationCurve;
        this.speedAnimationCurve = speedAnimationCurve;
    }

    public Vector2 GetMoveDir() {
        return moveDir;
    }

    public float GetNextYPos() {
        return nextYPos;
    }

    public float GetNextYPosCorrectionAbsolute() {
        return nextYPosCorrectionAbsolute;
    }

    public float GetNextXPos() {
        return nextXPos;
    }

    public float GetNextXPosCorrectionAbsolute() {
        return nextXPosCorrectionAbsolute;
    }
}
