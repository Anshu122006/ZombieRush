using System.Collections.Generic;
using UnityEngine;

public class GunAim : MonoBehaviour {
    [SerializeField] private float rotationSpeed = 180;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform crosshairPref;
    [SerializeField] private GunListSO gunListSO;

    private GameInputManager input;

    private List<GunBase> guns;
    private int curGunIndex;

    private Transform crosshair;
    private Vector2 dir;

    private Vector2 targetPos;
    private bool hasTarget;

    private void Start() {
        guns = new List<GunBase>();
        guns.Add(GunBase.Create(gunListSO.Pistol));

        input = GameInputManager.Instance;
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
        Vector2 target = input.GetMousePosition();

        Vector2 direction = (target - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if ((angle > 90 || angle < -90) && !sprite.flipY) sprite.flipY = true;
        if ((angle < 90 && angle > -90) && sprite.flipY) sprite.flipY = false;
    }

    private bool CheckForTarget() {
        Vector2 pos = input.GetMousePosition();
        int layerMask = LayerMask.GetMask("Enemy");
        float range = 4;

        Collider2D hit = Physics2D.OverlapPoint(pos, layerMask);

        if (hit != null) {
            if (targetPos != (Vector2)hit.transform.position) {
                targetPos = hit.transform.position;
                float dist = Vector2.Distance(transform.position, targetPos);

                if (dist > range) {
                    if (crosshair != null) {
                        targetPos = Vector2.zero;
                        Destroy(crosshair.gameObject);
                        crosshair = null;
                    }

                    return false;
                }

                if (crosshair != null) {
                    targetPos = Vector2.zero;
                    Destroy(crosshair.gameObject);
                    crosshair = null;
                }
                crosshair = Instantiate(crosshairPref);
                crosshair.position = (Vector3)targetPos + new Vector3(0, 0, -5);
            }
            return true;
        }
        else {
            if (crosshair != null) {
                targetPos = Vector2.zero;
                Destroy(crosshair.gameObject);
                crosshair = null;
            }

            return false;
        }
    }
}
