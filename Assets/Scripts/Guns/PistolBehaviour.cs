using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

public class PistolBehaviour : MonoBehaviour, IGunBehaviour {
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private PistolDefinition data;
    [SerializeField] private Transform firePointR;
    [SerializeField] private Transform firePointL;
    [SerializeField] private Transform firePointT;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private Transform muzzleFlashPref;

    private CharacterStatsData charStatData;
    private CharacterStatsManager charStatManager;
    private Coroutine delayShootCoroutine;
    private int maxLevel;

    // Runtime values
    private int exp;
    private int curLevel;

    // getters
    public bool CanShoot => true;
    public string Name => "pistol";

    public int ExpThreshold => data.expThreshold.EvaluateStat(curLevel, maxLevel);
    public int Damage => data.damage.EvaluateStat(curLevel, maxLevel);
    public int Accuracy => data.accuracy.EvaluateStat(curLevel, maxLevel);
    public float Range => data.range.EvaluateStat(curLevel, maxLevel);
    public float Weight => data.weight.EvaluateStat(curLevel, maxLevel);
    public float FireDelay => data.fireDelay.EvaluateStat(curLevel, maxLevel);
    public float SpreadAngle => data.spreadAngle.EvaluateStat(curLevel, maxLevel);

    public CharacterStatsData CharStatData { get => charStatData; set => charStatData = value; }
    public CharacterStatsManager CharStatManager { get => charStatManager; set => charStatManager = value; }
    public SpriteRenderer Renderer => spriteRenderer;
    public List<Sprite> Sprites => sprites;

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
        Vector2 start = Vector2.zero;
        if (Vector2.Angle(dir, Vector2.right) <= 45) start = firePointR.position;
        else if (Vector2.Angle(dir, Vector2.left) <= 45) start = firePointL.position;
        else start = firePointT.position;

        Transform flash = Instantiate(muzzleFlashPref, start, Quaternion.identity);
        flash.GetComponent<MuzzleFlash>().Setup(0.03f, dir);

        dir = VectorHandler.GenerateRandomDir(dir, SpreadAngle);
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
            MeshHandler.DrawLineMesh(start, end, 0.03f);
        }
    }

    private IEnumerator DelayShoot() {
        yield return new WaitForSeconds(FireDelay);
        delayShootCoroutine = null;
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
