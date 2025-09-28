using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    [SerializeField] private float rotationSpeed = 720;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Transform gun;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform crosshairPref;
    [SerializeField] private GunListSO gunListSO;

    private PlayerShared shared;
    private GameInputManager input;
    private Transform crosshair;
    private List<GunBase> guns;
    private int curGunIndex;

    private Vector2 targetPos;
    private bool hasTarget;

    private void Start() {
        guns = new List<GunBase>();
        guns.Add(GunBase.Create(gunListSO.Pistol));

        input = GameInputManager.Instance;
        shared = GetComponent<PlayerShared>();
    }

    private void Update() {
        FollowCursor();
        hasTarget = CheckForTarget();
        if (input.IsShooting() && hasTarget) {
            Shoot();
        }
    }

    private void Shoot() {
        Vector2 target = input.GetMousePosition();

        Vector2 direction = (target - (Vector2)transform.position).normalized;
        guns[curGunIndex].Shoot(firePoint, direction);
    }

    private void FollowCursor() {
        Vector2 pos = input.GetMousePosition();
        Vector2 dir = (pos - (Vector2)transform.position).normalized;

        // Vector2 faceDir = shared.move.faceDir;
        // dir = VectorHandler.ClampVector(faceDir, dir, 45);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        gun.rotation = Quaternion.RotateTowards(gun.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        float x = gun.transform.position.x, y = gun.transform.position.y, z = 0;
        if (shared.move.faceDir == Vector2.up) z = 5;
        gun.transform.position = new Vector3(x, y, z);

        if (dir.x < 0 && !sprite.flipY) sprite.flipY = true;
        if (dir.x > 0 && sprite.flipY) sprite.flipY = false;
    }

    private bool CheckForTarget() {
        Vector2 pos = input.GetMousePosition();
        Vector2 dir = (pos - (Vector2)transform.position).normalized;

        // Vector2 faceDir = shared.move.faceDir;
        // dir = VectorHandler.ClampVector(faceDir, dir, 45);
        float dist = Vector2.Distance(transform.position, pos);

        int layerMask = LayerMask.GetMask("Enemy");
        float range = guns[curGunIndex].Range;
        if (dist < range) range = dist;

        Collider2D hit = Physics2D.OverlapPoint((Vector2)transform.position + dir * range, layerMask);

        if (hit != null) {
            Vector2 tPos = hit.transform.position;
            if (targetPos != tPos) {
                RemoveCrosshair();
                targetPos = tPos;
                ShowCrosshair();
            }

            return true;
        }
        else {
            RemoveCrosshair();
            return false;
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
