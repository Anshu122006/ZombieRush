using System.Collections;
using Game.Utils;
using UnityEngine;

public class ShotgunBehaviour : MonoBehaviour, IGunBehaviour {
    [SerializeField] private ShotgunDefinition data;
    [SerializeField] private Transform firePoint;
    private Coroutine reloadCoroutine;
    private int maxLevel;

    // Runtime values
    private int exp;
    private int curAmmo;
    private int curLevel;

    // getters
    public Transform FirePoint => firePoint;
    public bool CanShoot => curAmmo > 0;
    public int ExpThreshold => (int)(data.expThreshold.init + (data.expThreshold.final - data.expThreshold.init) * Mathf.Pow((float)curLevel / maxLevel, data.expThreshold.pow));
    public int Damage => (int)(data.damage.init + (data.damage.final - data.damage.init) * Mathf.Pow((float)curLevel / maxLevel, data.damage.pow));
    public int Accuracy => (int)(data.accuracy.init + (data.accuracy.final - data.accuracy.init) * Mathf.Pow((float)curLevel / maxLevel, data.accuracy.pow));
    public float Range => (int)(data.range.init + (data.range.final - data.range.init) * Mathf.Pow((float)curLevel / maxLevel, data.range.pow));
    public float Weight => (int)(data.weight.init + (data.weight.final - data.weight.init) * Mathf.Pow((float)curLevel / maxLevel, data.weight.pow));
    public int MaxAmmo => (int)(data.maxAmmo.init + (data.maxAmmo.final - data.maxAmmo.init) * Mathf.Pow((float)curLevel / maxLevel, data.maxAmmo.pow));
    public float ReloadTime => data.reloadTime.init + (data.reloadTime.final - data.reloadTime.init) * Mathf.Pow((float)curLevel / maxLevel, data.reloadTime.pow);
    public int PelletsPerShot => (int)(data.pelletsPerShot.init + (data.pelletsPerShot.final - data.pelletsPerShot.init) * Mathf.Pow((float)curLevel / maxLevel, data.pelletsPerShot.pow));
    public float SpreadAngle => data.spreadAngle.init + (data.spreadAngle.final - data.spreadAngle.init) * Mathf.Pow((float)curLevel / maxLevel, data.spreadAngle.pow);

    public void Start() {
        exp = 0;
        curLevel = data.startLevel;
        maxLevel = data.maxLevel;
        curAmmo = MaxAmmo;
    }

    public void Shoot(Vector2 dir, IStatsManager player) {
        if (MaxAmmo > 0 && curAmmo < 1) return;
        if (reloadCoroutine == null) {
            Debug.Log("canShoot");
            int n = PelletsPerShot;
            if (MaxAmmo > 0) n = curAmmo > n ? n : curAmmo;

            for (int i = 0; i < n; i++) {
                RaycastBullet(dir, player);
                curAmmo--;
            }
            if (MaxAmmo > 0 && curAmmo < 0) curAmmo = 0;
            reloadCoroutine = StartCoroutine(Reload());
        }
    }

    private void RaycastBullet(Vector2 dir, IStatsManager player) {
        dir = VectorHandler.GenerateRandomDir(dir, SpreadAngle);
        Vector2 start = (Vector2)firePoint.position + dir * 0.1f;
        int layerMask = LayerMask.GetMask("BlockBullet", "Enemy");

        RaycastHit2D hit = Physics2D.Raycast(start, dir, Range, layerMask);
        Collider2D collider = hit.collider;

        if (collider != null) {
            Vector2 end = hit.point;
            MeshHandler.DrawLineMesh(start, end, 0.1f);

            IStatsManager enemy = collider.GetComponent<IStatsManager>();
            if (enemy != null) {
                enemy.TakeDamage(Damage, player);
                AddExp(enemy.ExpDrop);
            }
        }
        else {
            Vector2 end = start + dir * Range;
            MeshHandler.DrawLineMesh(start, end, 0.1f);
        }
    }

    private IEnumerator Reload() {
        yield return new WaitForSeconds(ReloadTime);
        reloadCoroutine = null;
        Debug.Log("Reload complete");
    }

    public void AddExp(int exp) {
        exp = Random.Range((int)(exp * 0.5f), exp);
        if (this.exp + exp < ExpThreshold) {
            this.exp += exp;
        }
        else {
            int gain = this.exp + exp;
            while (gain >= ExpThreshold) {
                gain -= ExpThreshold;
                LevelUp();
            }
            exp = gain;
        }
    }

    public void LevelUp() {
        if (curLevel < maxLevel) {
            curLevel++;
            curAmmo = MaxAmmo;
        }
    }
}
