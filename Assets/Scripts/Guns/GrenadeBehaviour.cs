using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBehaviour : MonoBehaviour, IGunBehaviour {
    [SerializeField] private Transform grenadePref;
    [SerializeField] private GrenadeDefinition data;
    [SerializeField] private AudioClip shootAudio;
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
    public string Name => data.gunName;
    public int CurLevel {
        get {
            return curLevel;
        }
        set {
            curLevel = Mathf.Clamp(value, 1, maxLevel);
            HudManager.Instance?.UpdateGunLevel(this);
        }
    }
    public int CurAmmo {
        get {
            return curAmmo;
        }
        set {
            curAmmo = Mathf.Clamp(value, 0, MaxAmmo);
            HudManager.Instance?.UpdateAmmo(this);
        }
    }
    public bool Shooting => GameInputManager.Instance.IsShooting() && CurAmmo > 0;

    public int ExpThreshold => data.expThreshold.EvaluateStat(CurLevel, maxLevel);
    public int Damage => data.damage.EvaluateStat(CurLevel, maxLevel);
    public int Accuracy => data.accuracy.EvaluateStat(CurLevel, maxLevel);
    public int MaxAmmo => data.maxAmmo.EvaluateStat(CurLevel, maxLevel);
    public float Range => data.range.EvaluateStat(CurLevel, maxLevel);
    public float Weight => data.weight.EvaluateStat(CurLevel, maxLevel);
    public float FireDelay => data.fireDelay.EvaluateStat(CurLevel, maxLevel);
    public float ExplosionRadius => data.explosionRadius.EvaluateStat(CurLevel, maxLevel);
    public float ProjectileSpeed => data.projectileSpeed.EvaluateStat(CurLevel, maxLevel);

    public CharacterStatsData CharStatData { get => charStatData; set => charStatData = value; }
    public CharacterStatsManager CharStatManager { get => charStatManager; set => charStatManager = value; }

    public void Start() {
        exp = 0;
        maxLevel = data.maxLevel;
        CurLevel = data.startLevel;
        CurAmmo = MaxAmmo;
    }

    public void Shoot(Vector2 dir) {
        if (CurAmmo < 1) return;
        if (delayShootCoroutine == null) {
            ShootGrenade(dir);
            CurAmmo--;
            delayShootCoroutine = StartCoroutine(DelayShoot());
        }
    }

    public void Refill(int amount) => CurAmmo += amount;

    public void AbortShoot() { }

    private void ShootGrenade(Vector2 dir) {
        Vector2 start = GetFirePosition(dir);

        Vector2 target = start + dir * Range;
        if (dir == Vector2.left || dir == Vector2.right) target += Vector2.down * 1;

        Grenade grenade = Instantiate(grenadePref, start, Quaternion.identity).GetComponent<Grenade>();
        grenade.InitializeProperties(target, ProjectileSpeed, trajectoryHeight, Damage, Accuracy, ExplosionRadius, (expDrop) => {
            AddExp((int)(expDrop * 1.5f));
            CharStatManager.AddExp(expDrop);
        });
        grenade.InitializeAnimationCurves(trajectoryAnimationCurve, axisCorrectionAnimationCurve, speedAnimationCurve);
    }

    private IEnumerator DelayShoot() {
        GameAudioManager.Instance.PlaySound(shootAudio, GetAudioPosition());
        yield return new WaitForSeconds(FireDelay);

        delayShootCoroutine = null;
    }

    public void AddExp(int exp) {
        if (ExpThreshold == 0) return;
        exp = Random.Range((int)(exp * 0.5f), exp);
        int totalExp = this.exp + exp;

        if (CurLevel >= maxLevel) {
            this.exp = Mathf.Min(totalExp, ExpThreshold);
            return;
        }

        while (totalExp >= ExpThreshold && CurLevel < maxLevel) {
            totalExp -= ExpThreshold;
            LevelUp();
        }
        this.exp = totalExp;
    }

    public void LevelUp() {
        if (CurLevel < maxLevel) {
            CurLevel++;
            CurAmmo = MaxAmmo;
        }
    }

    private Vector2 GetFirePosition(Vector2 dir) {
        if (dir == Vector2.up) return firePoint.up.position;
        else if (dir == Vector2.down) return firePoint.down.position;
        else if (dir == Vector2.left) return firePoint.left.position;
        else return firePoint.right.position;
    }

    private Vector3 GetAudioPosition() {
        float x = transform.position.x, y = transform.position.y;
        float z = Random.Range(-7, 0);
        return new Vector3(x, y, z);
    }
}
