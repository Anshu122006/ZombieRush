using System.Collections;
using System.Reflection;
using Game.Utils;
using UnityEngine;

public class LaserTurret : ITurretBehaviour {
    [SerializeField] private Transform firePoint;
    [SerializeField] private Material laserMaterial;
    [SerializeField] private LayerMask losLayers;
    [SerializeField] private LayerMask targetLayers;

    private bool isDischarged;
    private float curcharge;
    private Transform curTarget;
    private Coroutine shootCoroutine;

    public float FireDelay => ((LaserTurretDefinition)data).fireDelay.EvaluateStat(curLevel, maxLevel);
    public float RechargeRate => ((LaserTurretDefinition)data).rechargeRate.EvaluateStat(curLevel, maxLevel);
    public float DischargeRate => ((LaserTurretDefinition)data).dischargeRate.EvaluateStat(curLevel, maxLevel);


    private void Start() {
        curLevel = 1;
        maxLevel = ((LaserTurretDefinition)data).maxLevel;
        curHp = MHP;
        InvokeRepeating("SearchForEnemies", 0, ((LaserTurretDefinition)data).searchRate);
        InvokeRepeating("Recharge", 0, FireDelay);
    }

    private void Update() {
        if (shootCoroutine == null && curTarget != null)
            shootCoroutine = StartCoroutine(Shoot());
        AimAtTarget();
    }
    public void SearchForEnemies() {
        Vector2 center = visual.position;
        Collider2D hit = Physics2D.OverlapCircle(center, Range, losLayers);

        if (hit != null && (targetLayers & (1 << hit.gameObject.layer)) != 0) curTarget = hit.transform;
        else curTarget = null;
    }

    private void AimAtTarget() {
        if (curTarget == null || isDischarged) return;
        Vector2 dir = (curTarget.position - visual.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        visual.rotation = Quaternion.RotateTowards(visual.rotation, targetRotation, ((LaserTurretDefinition)data).rotationSpeed * Time.deltaTime);
    }

    private IEnumerator Shoot() {
        if (curTarget == null || isDischarged) {
            shootCoroutine = null;
            yield break;
        }

        MeshHandler.DrawLineMesh(firePoint.position, curTarget.position, FireDelay + 0.01f, 0.007f, 0.007f, laserMaterial);
        IStatsManager enemy = curTarget.GetComponent<IStatsManager>();
        enemy.TakeDamage(Damage, Accuracy);

        curcharge -= DischargeRate;
        curcharge = Mathf.Clamp(curcharge, 0, 100);
        if (curcharge == 0) isDischarged = true;

        yield return new WaitForSeconds(FireDelay);
        shootCoroutine = null;
    }

    private void Recharge() {
        if (isDischarged) {
            curcharge += RechargeRate;
            curcharge = Mathf.Clamp(curcharge, 0, 100);
            if (curcharge == 100) isDischarged = false;
        }
    }

    public override void LevelUp() {
        if (curLevel < maxLevel) {
            curLevel++;
            curcharge = 100;
        }
    }
}
