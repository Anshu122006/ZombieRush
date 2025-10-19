using System.Collections;
using Game.Utils;
using UnityEngine;

public class MissileTurret : ITurretBehaviour {
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject missilePref;
    [SerializeField] private LayerMask losLayers;
    [SerializeField] private LayerMask targetLayers;

    private int curAmmo;
    private Transform curTarget;
    private Coroutine shootCoroutine;

    public float ReloadRate => ((MissileTurretDefinition)data).reloadRate.EvaluateStat(curLevel, maxLevel);
    public int MaxAmmo => ((MissileTurretDefinition)data).maxAmmo.EvaluateStat(curLevel, maxLevel);
    public float ShootDelay => ((MissileTurretDefinition)data).shootDelay.EvaluateStat(curLevel, maxLevel);
    public float ProjectileSpeed => ((MissileTurretDefinition)data).projectileSpeed.EvaluateStat(curLevel, maxLevel);
    public float ProjectileRange => ((MissileTurretDefinition)data).projectileRange.EvaluateStat(curLevel, maxLevel);



    private void Start() {
        curLevel = 1;
        maxLevel = ((MissileTurretDefinition)data).maxLevel;
        curHp = MHP;
        curAmmo = MaxAmmo;
        InvokeRepeating("SearchForEnemies", 0, ((MissileTurretDefinition)data).searchRate);
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
        Collider2D hit = Physics2D.OverlapCircle(center, Range, losLayers);

        if (hit != null && (targetLayers & (1 << hit.gameObject.layer)) != 0) curTarget = hit.transform;
        else curTarget = null;
    }

    private void AimAtTarget() {
        if (curTarget == null || curAmmo <= 0) return;
        Vector2 dir = (curTarget.position - visual.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        visual.rotation = Quaternion.RotateTowards(visual.rotation, targetRotation, ((ProjectileTurretDefinition)data).rotationSpeed * Time.deltaTime);
    }

    private IEnumerator Shoot() {
        Vector2 dir = (curTarget.position - transform.position).normalized;
        Missile missile = Instantiate(missilePref).GetComponent<Missile>();
        missile.transform.position = firePoint.position;
        missile.Setup(curTarget, ProjectileSpeed * 0.8f, ProjectileSpeed * 1.2f, ProjectileRange, Damage, Accuracy, true, (expDrop) => GlobalTurretData.Instance.AddExp(Name, expDrop));
        curAmmo--;

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
