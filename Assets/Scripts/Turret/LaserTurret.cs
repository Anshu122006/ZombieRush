using System.Collections;
using System.Reflection;
using Game.Utils;
using UnityEngine;

public class LaserTurret : MonoBehaviour {
    private bool isCharged;
    private bool searched;
    private bool isturning;
    private float currCharge;
    private float searchRate;
    [SerializeField] Transform firePoint;
    [SerializeField] Material laserMaterial;

    private Transform curTarget;
    private TurretType turretType;

    private bool seeThroughWalls;
    private float maxCharge;
    private float dischargeRate;
    private float chargeRate;
    private float reloadSpeed;
    private int damage;
    private int accuracy;
    private float range;
    private float turnTime;

    private void Start() {
        seeThroughWalls = true;
        isCharged = true;
        searched = false;
        isturning = false;
        searchRate = 0.1f;
        maxCharge = 100;
        currCharge = 100;
        dischargeRate = 2;
        chargeRate = 0.5f;
        reloadSpeed = 0.01f;
        accuracy = 100;
        damage = 4;
        range = 6;
        turnTime = 3;
    }

    private void Update() {
        if (curTarget != null)
            Shoot();
        SearchForEnemies();
    }
    public void SearchForEnemies() {
        if (!isCharged) return;

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
        if (curTarget == null || !isCharged)
            return;
        Quaternion rot = VectorHandler.RotationFromVector(curTarget.position - transform.position);
        if (transform.rotation != rot)
            transform.rotation = rot;
    }

    private IEnumerator TurnTowardsTarget(Vector3 curTarget) {
        float elapsed = 0;
        float angle = VectorHandler.AngleFromVector(curTarget);
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
        if (isCharged && currCharge <= 0) {
            currCharge = 0;
            isCharged = false;
        }
        else if (!isCharged && currCharge >= maxCharge) {
            currCharge = maxCharge;
            isCharged = true;
        }

        if (isCharged) {
            FunctionTimer.CreateSceneTimer(() => {
                if (curTarget == null) return;

                MeshHandler.DrawLineMesh(firePoint.position, curTarget.position, reloadSpeed + 0.01f, 0.007f, 0.007f, laserMaterial);
                IStats enemy = curTarget.GetComponent<IStats>();
                enemy.TakeDamage(damage, accuracy);
                currCharge -= dischargeRate;
            }, reloadSpeed);
        }
        else {
            FunctionTimer.CreateSceneTimer(() => {
                currCharge += chargeRate;
            }, reloadSpeed);
        }
    }
}
