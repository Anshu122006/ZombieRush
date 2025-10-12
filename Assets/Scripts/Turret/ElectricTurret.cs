using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTurret : ITurretBehaviour {
    [SerializeField] private Transform effect1;
    [SerializeField] private Transform effect2;
    [SerializeField] private Transform enemyEffect;
    [SerializeField] private Transform centre;
    [SerializeField] private LayerMask losLayers;
    [SerializeField] private LayerMask targetLayers;

    private int curcharge;
    private List<Transform> curTargets;
    private Coroutine shootCoroutine;
    public int MaxCharge => ((ElectricTurretDefinition)data).maxCharge.EvaluateStat(curLevel, maxLevel);
    public int ChargesPerShot => ((ElectricTurretDefinition)data).chargesPerShot.EvaluateStat(curLevel, maxLevel);
    public float ShootDelay => ((ElectricTurretDefinition)data).shootDelay.EvaluateStat(curLevel, maxLevel);
    public float ReloadTime => ((ElectricTurretDefinition)data).reloadTime.EvaluateStat(curLevel, maxLevel);

    private void Start() {
        curLevel = 1;
        maxLevel = data.maxLevel;
        curcharge = MaxCharge;
        curHp = MHP;
        InvokeRepeating("SearchForEnemies", 0, data.searchRate);
        InvokeRepeating("ReloadCharges", 0, ReloadTime);
    }

    private void Update() {
        if (shootCoroutine == null && curTargets != null && curTargets.Count > 0 && curcharge > 0)
            shootCoroutine = StartCoroutine(Shoot());
    }

    public void SearchForEnemies() {
        Vector2 centre = visual.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(centre, Range, losLayers);
        if (hits != null) {
            curTargets = new List<Transform>();
            foreach (Collider2D hit in hits)
                if ((targetLayers & (1 << hit.gameObject.layer)) != 0)
                    curTargets.Add(hit.transform);
        }
        else {
            curTargets = null;
        }
    }

    private IEnumerator Shoot() {
        Transform elec1 = Instantiate(effect1), elec2 = Instantiate(effect2);
        elec1.position = centre.position;
        elec2.position = centre.position;
        elec1.localScale = Vector3.one * Range * 0.6f;
        elec2.localScale = Vector3.one * Range * 0.8f;

        foreach (Transform target in curTargets) {
            Transform enemyElec = Instantiate(enemyEffect, target);
            enemyElec.localPosition = new Vector3(0, 0, -6);
            IStatsManager enemy = target.GetComponent<IStatsManager>();
            enemy.TakeDamage(Damage, Accuracy);
            curcharge--;
            curcharge = Mathf.Clamp(curcharge, 0, MaxCharge);
            if (curcharge == 0) break;
        }

        yield return new WaitForSeconds(ShootDelay);
        shootCoroutine = null;
    }

    private void ReloadCharges() {
        if (curcharge < MaxCharge)
            curcharge++;
    }

    public override void LevelUp() {
        if (curLevel < maxLevel) {
            curLevel++;
            curcharge = MaxCharge;
        }
    }
}
