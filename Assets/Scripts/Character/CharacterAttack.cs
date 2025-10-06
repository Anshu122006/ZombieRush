using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

public class CharacterAttack : MonoBehaviour {
    [SerializeField] private CharacterGunHandler gunHandler;
    [SerializeField] private CharacterStatsManager player;
    [SerializeField] private float rotationSpeed = 720;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform crosshairPref;

    private CharacterComponents components;
    private GameInputManager input;
    private Transform crosshair;

    private Vector2 targetPos;
    // private bool hasTarget;

    private void Start() {
        input = GameInputManager.Instance;
        components = GetComponent<CharacterComponents>();
    }

    private void Update() {
        FollowCursor();
        CheckForTarget();
        if (input.IsShooting()) Shoot();
        else gunHandler.Gun.AbortShoot();

        if (Input.GetKeyDown(KeyCode.Alpha1)) gunHandler.Previous();
        if (Input.GetKeyDown(KeyCode.Alpha2)) gunHandler.Next();
    }

    private void Shoot() {
        Vector2 target = input.GetMousePosition();
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        gunHandler.Gun.Shoot(direction);
    }

    private void FollowCursor() {
        //To make the gun follow the cursor
        Transform pivot = gunHandler.transform;
        Vector2 pos = input.GetMousePosition();
        Vector2 dir = (pos - (Vector2)transform.position).normalized;

        Vector2 faceDir = components.movement.faceDir;
        dir = VectorHandler.ClampVector(faceDir, dir, 45);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        pivot.rotation = Quaternion.RotateTowards(pivot.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        gunHandler.AdjustPos(faceDir);

        // To make the pivot appear above or below player depending on the direction
        SpriteRenderer spriteRenderer = gunHandler.GunSpriteRenderer;
        if (faceDir == Vector2.up || faceDir == Vector2.left) spriteRenderer.sortingLayerName = "BelowChar";
        else spriteRenderer.sortingLayerName = "AboveChar";
    }

    private void CheckForTarget() {
        Vector2 pos = input.GetMousePosition();
        Vector2 dir = (pos - (Vector2)transform.position).normalized;

        Vector2 faceDir = components.movement.faceDir;
        dir = VectorHandler.ClampVector(faceDir, dir, 45);
        float dist = Vector2.Distance(transform.position, pos);

        int layerMask = LayerMask.GetMask("Enemy");
        float range = gunHandler.Gun.Range;
        if (dist < range) range = dist;

        Collider2D hit = Physics2D.OverlapPoint((Vector2)transform.position + dir * range, layerMask);

        //To show or remove crosshair
        if (hit != null) {
            Vector2 tPos = hit.transform.position;
            if (targetPos != tPos) {
                RemoveCrosshair();
                targetPos = tPos;
                ShowCrosshair();
            }
            // return true;
        }
        else {
            RemoveCrosshair();
            // return false;
        }
    }

    private void ShowCrosshair() {
        crosshair = Instantiate(crosshairPref);
        crosshair.position = (Vector3)targetPos + new Vector3(0, 0, -5);
    }

    private void RemoveCrosshair() {
        if (crosshair == null) return;

        targetPos = Vector2.zero;
        Destroy(crosshair.gameObject);
        crosshair = null;
    }
}
