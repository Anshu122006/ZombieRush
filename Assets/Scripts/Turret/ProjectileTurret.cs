using System.Collections;
using Game.Utils;
using UnityEditor;
using UnityEngine;

public class ProjectileTurret : MonoBehaviour {
    private bool isReloaded;
    private bool searched;
    private bool isturning;
    private int curAmmo;
    private float searchRate;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPref;

    private Transform curTarget;
    private TurretType turretType;

    private bool seeThroughWalls;
    private int particlesPerShot;
    private int maxAmmo;
    private int damage;
    private int accuracy;
    private float deviation;
    private float reloadSpeed;
    private float range;
    private float turnTime;

    private void Start() {
        seeThroughWalls = true;
        isReloaded = true;
        searched = false;
        isturning = false;
        searchRate = 0.1f;
        reloadSpeed = 0.4f;
        maxAmmo = 30;
        curAmmo = 30;
        particlesPerShot = 3;
        damage = 6;
        deviation = 1;
        range = 6;
        turnTime = 0.5f;
    }

    private void Update() {
        if (curTarget != null)
            Shoot();

        SearchForEnemies();
    }
    public void SearchForEnemies() {
        if (maxAmmo != 0 && curAmmo < 1) return;

        if (!searched) {
            FunctionTimer.CreateSceneTimer(() => {
                Transform prevTarget = curTarget;
                Vector2 centre = transform.position;
                int layerMask = seeThroughWalls ? LayerMask.GetMask("Enemy") : LayerMask.GetMask("Enemy", "BlockBullets");

                Collider2D hit = Physics2D.OverlapCircle(centre, range, layerMask);

                if (hit != null && hit.CompareTag("Enemy")) curTarget = hit.transform;
                else curTarget = null;

                if (prevTarget == null && curTarget != null) {
                    isturning = true;
                    StartCoroutine(TurnTowardsTarget(curTarget.position));
                }
                searched = false;
            }, searchRate);
            searched = true;
        }

        if (!isturning) AimAtTarget();
    }

    private void AimAtTarget() {
        if (curTarget == null) return;
        Quaternion rot = VectorHandler.RotationFromVector(curTarget.position - transform.position);
        if (transform.rotation != rot)
            transform.rotation = rot;
    }

    private IEnumerator TurnTowardsTarget(Vector3 target) {
        float elapsed = 0;
        float angle = VectorHandler.AngleFromVector(target);
        float duration = angle / 360 * turnTime;

        while (elapsed < duration) {
            float t = elapsed / duration;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, 0, angle);
        isturning = false;
    }

    private void Shoot() {
        if (maxAmmo != 0 && curAmmo < 1) return;

        if (isReloaded) {
            float time = reloadSpeed;
            if (curAmmo % particlesPerShot > 0)
                time = 0.1f * reloadSpeed;
            FunctionTimer.CreateSceneTimer(() => {
                if (curTarget == null) {
                    isReloaded = true;
                    return;
                }

                Vector2 dir = VectorHandler.GenerateRandomDir(curTarget.position - transform.position, deviation);
                Bullet bullet = Instantiate(bulletPref).GetComponent<Bullet>();
                bullet.transform.position = firePoint.position;
                bullet.Setup(dir, 8, range * 1.2f, damage, accuracy);
                curAmmo -= maxAmmo > 0 ? 1 : 0;
                isReloaded = true;
            }, time);
            isReloaded = false;
        }
    }
}
