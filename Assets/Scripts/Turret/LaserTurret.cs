using System.Collections;
using System.Reflection;
using Game.Utils;
using UnityEngine;

public class LaserTurret : MonoBehaviour {
    [SerializeField] private Transform firePoint;
    [SerializeField] private Material laserMaterial;
    [SerializeField] private LaserTurretDefinition data;

    private bool isDischarged;
    private int curLevel;
    private int maxLevel;
    private float curcharge;
    private Transform curTarget;
    private Coroutine shootCoroutine;

    public int Damage => data.damage.EvaluateStat(curLevel, maxLevel);
    public int Accuracy => data.accuracy.EvaluateStat(curLevel, maxLevel);
    public float Range => data.range.EvaluateStat(curLevel, maxLevel);
    public float FireDelay => data.fireDelay.EvaluateStat(curLevel, maxLevel);
    public float RechargeRate => data.rechargeRate.EvaluateStat(curLevel, maxLevel);
    public float DischargeRate => data.dischargeRate.EvaluateStat(curLevel, maxLevel);


    private void Start() {
        curLevel = 1;
        maxLevel = data.maxLevel;
        InvokeRepeating("SearchForEnemies", 0, data.searchRate);
        InvokeRepeating("Recharge", 0, FireDelay);
    }

    private void Update() {
        if (shootCoroutine == null && curTarget != null)
            shootCoroutine = StartCoroutine(Shoot());
        AimAtTarget();
    }
    public void SearchForEnemies() {
        Vector2 centre = transform.position;

        int layerMask = data.seeThroughWalls ? LayerMask.GetMask("Enemy") : LayerMask.GetMask("Enemy", "BlockBullets");
        Collider2D hit = Physics2D.OverlapCircle(centre, Range, layerMask);

        if (hit != null && (LayerMask.GetMask("Enemy") & (1 << hit.gameObject.layer)) != 0) curTarget = hit.transform;
        else curTarget = null;
    }

    private void AimAtTarget() {
        if (curTarget == null || isDischarged) return;
        Vector2 dir = (curTarget.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, data.rotationSpeed * Time.deltaTime);
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
}
