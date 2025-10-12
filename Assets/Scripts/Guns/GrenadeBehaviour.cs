using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBehaviour : MonoBehaviour, IGunBehaviour {
    [SerializeField] private Transform grenadePref;
    [SerializeField] private GrenadeDefinition data;
    [SerializeField] private GunFirePoint firePoint;

    [SerializeField] private AnimationCurve trajectoryAnimationCurve;
    [SerializeField] private AnimationCurve axisCorrectionAnimationCurve;
    [SerializeField] private AnimationCurve speedAnimationCurve;
    [SerializeField] private float trajectoryHeight = 0.1f;

    private CharacterStatsData charStatData;
    private CharacterStatsManager charStatManager;
    private Coroutine delayShootCoroutine;
    private int maxLevel;

    // Runtime values
    private int exp;
    private int curAmmo;
    private int curLevel;

    // getters
    public string Name => "grenade";
    public bool Shooting => GameInputManager.Instance.IsShooting() && curAmmo > 0;

    public int ExpThreshold => data.expThreshold.EvaluateStat(curLevel, maxLevel);
    public int Damage => data.damage.EvaluateStat(curLevel, maxLevel);
    public int Accuracy => data.accuracy.EvaluateStat(curLevel, maxLevel);
    public int MaxAmmo => data.maxAmmo.EvaluateStat(curLevel, maxLevel);
    public float Range => data.range.EvaluateStat(curLevel, maxLevel);
    public float Weight => data.weight.EvaluateStat(curLevel, maxLevel);
    public float FireDelay => data.fireDelay.EvaluateStat(curLevel, maxLevel);
    public float ExplosionRadius => data.explosionRadius.EvaluateStat(curLevel, maxLevel);
    public float ProjectileSpeed => data.projectileSpeed.EvaluateStat(curLevel, maxLevel);

    public CharacterStatsData CharStatData { get => charStatData; set => charStatData = value; }
    public CharacterStatsManager CharStatManager { get => charStatManager; set => charStatManager = value; }

    public void Start() {
        exp = 0;
        curLevel = data.startLevel;
        maxLevel = data.maxLevel;
        curAmmo = MaxAmmo;
    }

    public void Shoot(Vector2 dir) {
        if (curAmmo < 1) return;
        if (delayShootCoroutine == null) {
            ShootGrenade(dir);
            curAmmo = Mathf.Clamp(curAmmo - 1, 0, MaxAmmo);
            delayShootCoroutine = StartCoroutine(DelayShoot());
        }
    }

    public void AbortShoot() { }

    private void ShootGrenade(Vector2 dir) {
        Vector2 start = GetFirePosition(dir);

        Vector2 target = start + dir * Range;
        if (dir == Vector2.left || dir == Vector2.right) target += Vector2.down * 1;

        Grenade grenade = Instantiate(grenadePref, start, Quaternion.identity).GetComponent<Grenade>();
        grenade.InitializeProperties(target, ProjectileSpeed, trajectoryHeight, Damage, Accuracy, ExplosionRadius);
        grenade.InitializeAnimationCurves(trajectoryAnimationCurve, axisCorrectionAnimationCurve, speedAnimationCurve);
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

    private Vector2 GetFirePosition(Vector2 dir) {
        if (dir == Vector2.up) return firePoint.up.position;
        else if (dir == Vector2.down) return firePoint.down.position;
        else if (dir == Vector2.left) return firePoint.left.position;
        else return firePoint.right.position;
    }
}
