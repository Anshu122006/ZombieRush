using System.Collections;
using Game.Utils;
using UnityEngine;

public class PistolBehaviour : MonoBehaviour, IGunBehaviour {
    [SerializeField] private PistolDefinition data;
    [SerializeField] private Transform firePoint;

    private CharacterStatsData charStatData;
    private CharacterStatsManager charStatManager;
    private Coroutine delayShootCoroutine;
    private int maxLevel;

    // Runtime values
    private int exp;
    private int curLevel;

    // getters
    public Transform FirePoint => firePoint;
    public bool CanShoot => true;

    public int ExpThreshold => data.expThreshold.EvaluateStat(curLevel, maxLevel);
    public int Damage => data.damage.EvaluateStat(curLevel, maxLevel);
    public int Accuracy => data.accuracy.EvaluateStat(curLevel, maxLevel);
    public float Range => data.range.EvaluateStat(curLevel, maxLevel);
    public float Weight => data.weight.EvaluateStat(curLevel, maxLevel);
    public float FireDelay => data.fireDelay.EvaluateStat(curLevel, maxLevel);
    public float SpreadAngle => data.spreadAngle.EvaluateStat(curLevel, maxLevel);

    public CharacterStatsData CharStatData { get => charStatData; set => charStatData = value; }
    public CharacterStatsManager CharStatManager { get => charStatManager; set => charStatManager = value; }

    public void Start() {
        exp = 0;
        curLevel = data.startLevel;
        maxLevel = data.maxLevel;
    }

    public void Shoot(Vector2 dir) {
        if (delayShootCoroutine == null) {
            RaycastBullet(dir);
            delayShootCoroutine = StartCoroutine(DelayShoot());
        }
    }

    public void AbortShoot() { }

    private void RaycastBullet(Vector2 dir) {
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
                enemy.TakeDamage(Damage, charStatData.LUCK + Accuracy, charStatManager);
                AddExp(enemy.ExpDrop);
            }
        }
        else {
            Vector2 end = start + dir * Range;
            MeshHandler.DrawLineMesh(start, end, 0.1f);
        }
    }

    private IEnumerator DelayShoot() {
        yield return new WaitForSeconds(FireDelay);
        delayShootCoroutine = null;
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
        }
    }
}
