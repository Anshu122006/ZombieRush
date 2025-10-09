using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaBehaviour : MonoBehaviour, IGunBehaviour {
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform firePointS;
    [SerializeField] private Transform firePointT;
    [SerializeField] private Transform projectilePref;
    [SerializeField] private BazookaDefinition data;
    [SerializeField] private List<Sprite> sprites;

    private CharacterStatsData charStatData;
    private CharacterStatsManager charStatManager;
    private Coroutine delayShootCoroutine;
    private int maxLevel;

    // Runtime values
    private int exp;
    private int curAmmo;
    private int curLevel;

    // getters
    public bool CanShoot => curAmmo > 0;
    public string Name => "bazooka";

    public int ExpThreshold => data.expThreshold.EvaluateStat(curLevel, maxLevel);
    public int Damage => data.damage.EvaluateStat(curLevel, maxLevel);
    public int Accuracy => data.accuracy.EvaluateStat(curLevel, maxLevel);
    public float Range => data.range.EvaluateStat(curLevel, maxLevel);
    public float Weight => data.weight.EvaluateStat(curLevel, maxLevel);
    public int MaxAmmo => data.maxAmmo.EvaluateStat(curLevel, maxLevel);
    public float FireDelay => data.fireDelay.EvaluateStat(curLevel, maxLevel);
    public float ProjectileRange => data.projectileRange.EvaluateStat(curLevel, maxLevel);
    public float ProjectileSpeed => data.projectileSpeed.EvaluateStat(curLevel, maxLevel);

    public CharacterStatsData CharStatData { get => charStatData; set => charStatData = value; }
    public CharacterStatsManager CharStatManager { get => charStatManager; set => charStatManager = value; }
    public SpriteRenderer Renderer => spriteRenderer;
    public List<Sprite> Sprites => sprites;

    public void Start() {
        exp = 0;
        curLevel = data.startLevel;
        maxLevel = data.maxLevel;
        curAmmo = MaxAmmo;
    }

    public void Shoot(Vector2 dir) {
        if (curAmmo < 1) return;
        if (delayShootCoroutine == null) {
            ShootMissile(dir);
            delayShootCoroutine = StartCoroutine(DelayShoot());
        }
    }

    public void AbortShoot() { }

    private void ShootMissile(Vector2 dir) {
        float a = Vector2.Angle(dir, Vector2.right);
        if (a > 90) a = 180 - a;
        Vector2 start = a <= 45 ? firePointS.position : firePointT.position;

        Transform target = new GameObject("Target").transform;
        target.position = start + dir * Range;

        Missile missile = GameObject.Instantiate(projectilePref).GetComponent<Missile>();
        missile.transform.position = start;
        missile.Setup(target, ProjectileSpeed * 0.1f, ProjectileSpeed, Range, Damage, Accuracy, false);
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
            curAmmo = MaxAmmo;
        }
    }
}
