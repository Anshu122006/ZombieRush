using System.Collections;
using Game.Utils;
using UnityEditor;
using UnityEngine;

public class ProjectileTurret : MonoBehaviour,ITurretBehaviour {
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPref;
    [SerializeField] private ProjectileTurretDefinition data;

    private int curLevel;
    private int maxLevel;
    private int curAmmo;
    private Transform curTarget;
    private Coroutine shootCoroutine;

    public int CurLevel => curLevel;
    public int UpgradeCost => data.upgradeCost.EvaluateStat(curLevel, maxLevel);
    public string Name => data.turretName;

    public float ReloadRate => data.reloadRate.EvaluateStat(curLevel, maxLevel);
    public int Damage => data.damage.EvaluateStat(curLevel, maxLevel);
    public int Accuracy => data.accuracy.EvaluateStat(curLevel, maxLevel);
    public int MaxAmmo => data.maxAmmo.EvaluateStat(curLevel, maxLevel);
    public float Range => data.range.EvaluateStat(curLevel, maxLevel);
    public float ShootDelay => data.shootDelay.EvaluateStat(curLevel, maxLevel);
    public float FireDelay => data.fireDelay.EvaluateStat(curLevel, maxLevel);
    public float ProjectileSpeed => data.projectileSpeed.EvaluateStat(curLevel, maxLevel);
    public int PelletsPerShot => data.pelletsPerShot.EvaluateStat(curLevel, maxLevel);



    private void Start() {
        curLevel = 1;
        maxLevel = data.maxLevel;
        curAmmo = MaxAmmo;
        InvokeRepeating("SearchForEnemies", 0, data.searchRate);
        InvokeRepeating("ReloadAmmo", 0, ReloadRate);
    }

    private void Update() {
        if (shootCoroutine == null && curTarget != null && curAmmo > 0)
            shootCoroutine = StartCoroutine(Shoot());
        AimAtTarget();
    }

    public void SearchForEnemies() {
        if (curAmmo < 1) return;

        Vector2 centre = transform.position;

        int layerMask = data.seeThroughWalls ? LayerMask.GetMask("Enemy") : LayerMask.GetMask("Enemy", "BlockBullets");
        Collider2D hit = Physics2D.OverlapCircle(centre, Range, layerMask);

        if (hit != null && (LayerMask.GetMask("Enemy") & (1 << hit.gameObject.layer)) != 0) curTarget = hit.transform;
        else curTarget = null;
    }

    private void AimAtTarget() {
        if (curTarget == null || curAmmo <= 0) return;
        Vector2 dir = (curTarget.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, data.rotationSpeed * Time.deltaTime);
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
            bullet.Setup(dir, 8, Range * 1.2f, Damage, Accuracy);

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

    public void LevelUp() {
        if (curLevel < maxLevel) {
            curLevel++;
            curAmmo = MaxAmmo;
        }
    }
}
