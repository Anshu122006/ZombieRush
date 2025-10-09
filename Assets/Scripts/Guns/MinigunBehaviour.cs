using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

public class MinigunBehaviour : MonoBehaviour, IGunBehaviour {
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform firePointR;
    [SerializeField] private Transform firePointL;
    [SerializeField] private Transform firePointT;
    [SerializeField] private Transform bulletPref;
    [SerializeField] private MinigunDefinition data;
    [SerializeField] private List<Sprite> sprites;

    private CharacterStatsData charStatData;
    private CharacterStatsManager charStatManager;
    private Coroutine delayShootCoroutine;
    private int maxLevel;

    // Runtime values
    private int exp;
    private int curAmmo;
    private int spinAmmoUsed;
    private int curLevel;

    // getters
    public bool CanShoot => curAmmo > 0;
    public string Name => "minigun";

    public int ExpThreshold => data.expThreshold.EvaluateStat(curLevel, maxLevel);
    public int Damage => data.damage.EvaluateStat(curLevel, maxLevel);
    public int Accuracy => data.accuracy.EvaluateStat(curLevel, maxLevel);
    public float Range => data.range.EvaluateStat(curLevel, maxLevel);
    public float Weight => data.weight.EvaluateStat(curLevel, maxLevel);
    public int MaxAmmo => data.maxAmmo.EvaluateStat(curLevel, maxLevel);
    public int AmmoPerSpin => data.ammoPerSpin.EvaluateStat(curLevel, maxLevel);
    public float ProjectileSpeed => data.projectileSpeed.EvaluateStat(curLevel, maxLevel);
    public float FireDelay => data.fireDelay.EvaluateStat(curLevel, maxLevel);
    public float SpinUpTime => data.spinUpTime.EvaluateStat(curLevel, maxLevel);
    public float Offset => data.offset;

    public CharacterStatsData CharStatData { get => charStatData; set => charStatData = value; }
    public CharacterStatsManager CharStatManager { get => charStatManager; set => charStatManager = value; }
    public SpriteRenderer Renderer => spriteRenderer;
    public List<Sprite> Sprites => sprites;

    public void Start() {
        exp = 0;
        curLevel = data.startLevel;
        maxLevel = data.maxLevel;
        spinAmmoUsed = 0;
        curAmmo = MaxAmmo;
    }

    public void Shoot(Vector2 dir) {
        if (curAmmo < 1) return;
        if (delayShootCoroutine == null) {
            dir = dir.normalized;

            Vector2 start = Vector2.zero;
            if (Vector2.Angle(dir, Vector2.right) <= 45) start = firePointR.position;
            else if (Vector2.Angle(dir, Vector2.left) <= 45) start = firePointL.position;
            else start = firePointT.position;

            Vector2 off = new Vector2(dir.y, -dir.x).normalized * Random.Range(-Offset, Offset);
            Vector2 spawnPoint = start + off;

            Transform bullet = Instantiate(bulletPref);
            bullet.position = spawnPoint;
            bullet.GetComponent<Bullet>().Setup(dir, ProjectileSpeed, Range, Damage, Accuracy);

            spinAmmoUsed++;
            curAmmo = Mathf.Clamp(curAmmo - 1, 0, MaxAmmo);
            delayShootCoroutine = StartCoroutine(DelayShoot());
        }
    }

    public void AbortShoot() { }

    private IEnumerator DelayShoot() {
        if (spinAmmoUsed < AmmoPerSpin) {
            yield return new WaitForSeconds(FireDelay);
        }
        else {
            spinAmmoUsed = 0;
            yield return new WaitForSeconds(SpinUpTime);
        }
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
            this.exp = gain;
        }
    }

    public void LevelUp() {
        if (curLevel < maxLevel) {
            curLevel++;
            curAmmo = MaxAmmo;
        }
    }
}
