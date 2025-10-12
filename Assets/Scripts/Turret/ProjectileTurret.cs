using System.Collections;
using Game.Utils;
using UnityEditor;
using UnityEngine;

public class ProjectileTurret : ITurretBehaviour {
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPref;
    [SerializeField] private LayerMask losLayers;
    [SerializeField] private LayerMask targetLayers;

    private int curAmmo;
    private Transform curTarget;
    private Coroutine shootCoroutine;

    public int MaxAmmo => ((ProjectileTurretDefinition)data).maxAmmo.EvaluateStat(curLevel, maxLevel);
    public float ReloadRate => ((ProjectileTurretDefinition)data).reloadRate.EvaluateStat(curLevel, maxLevel);
    public float ShootDelay => ((ProjectileTurretDefinition)data).shootDelay.EvaluateStat(curLevel, maxLevel);
    public float FireDelay => ((ProjectileTurretDefinition)data).fireDelay.EvaluateStat(curLevel, maxLevel);
    public float ProjectileSpeed => ((ProjectileTurretDefinition)data).projectileSpeed.EvaluateStat(curLevel, maxLevel);
    public int PelletsPerShot => ((ProjectileTurretDefinition)data).pelletsPerShot.EvaluateStat(curLevel, maxLevel);



    private void Start() {
        curLevel = 1;
        maxLevel = ((ProjectileTurretDefinition)data).maxLevel;
        curHp = MHP;
        curAmmo = MaxAmmo;
        InvokeRepeating("SearchForEnemies", 0, ((ProjectileTurretDefinition)data).searchRate);
        InvokeRepeating("ReloadAmmo", 0, ReloadRate);
    }

    private void Update() {
        if (shootCoroutine == null && curTarget != null && curAmmo > 0)
            shootCoroutine = StartCoroutine(Shoot());
        AimAtTarget();
    }

    public void SearchForEnemies() {
        if (curAmmo < 1) return;
        Vector2 center = visual.position;
        Collider2D col = Physics2D.OverlapCircle(center, Range, targetLayers);

        if (col != null) {
            Vector2 dir = (Vector2)col.transform.position - center;
            float dist = Vector2.Distance(col.transform.position, center);
            RaycastHit2D hit = Physics2D.Raycast(center, dir, dist, losLayers);
            if (hit.collider != null) curTarget = hit.collider.transform;
        }
        else {
            curTarget = null;
        }
    }

    private void AimAtTarget() {
        if (curTarget == null || curAmmo <= 0) return;
        Vector2 dir = (curTarget.position - visual.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        visual.rotation = Quaternion.RotateTowards(visual.rotation, targetRotation, ((ProjectileTurretDefinition)data).rotationSpeed * Time.deltaTime);
    }

    private IEnumerator Shoot() {
        int n = Mathf.Min(PelletsPerShot, curAmmo);

        for (int i = 0; i < n; i++) {
            if (curTarget == null) {
                shootCoroutine = null;
                yield break;
            }

            Vector2 dir = (curTarget.position - transform.position).normalized;
            Bullet bullet = Instantiate(bulletPref).GetComponent<Bullet>();
            bullet.transform.position = firePoint.position;
            bullet.Setup(dir, ProjectileSpeed, Range * 1.2f, Damage, Accuracy);

            curAmmo--;
            yield return new WaitForSeconds(FireDelay);
        }

        yield return new WaitForSeconds(ShootDelay);
        shootCoroutine = null;
    }

    private void ReloadAmmo() {
        if (curAmmo < MaxAmmo)
            curAmmo++;
    }

    public override void LevelUp() {
        if (curLevel < maxLevel) {
            curLevel++;
            curAmmo = MaxAmmo;
        }
    }
}
