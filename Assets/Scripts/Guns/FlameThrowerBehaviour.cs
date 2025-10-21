using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerBehaviour : MonoBehaviour, IGunBehaviour {
    [SerializeField] private Transform emitter;
    [SerializeField] private FlameThrowerDefinition data;
    [SerializeField] private GunFirePoint firePoint;

    private CharacterStatsData charStatData;
    private CharacterStatsManager charStatManager;
    private Coroutine delayShootCoroutine;
    private int maxLevel;

    // Runtime values
    private int exp;
    private int curLevel;
    private float curFuel;

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
            return (int)curFuel;
        }
        set {
            curFuel = Mathf.Clamp(curFuel + value, 0, MaxAmmo);
            HudManager.Instance?.UpdateAmmo(this);
        }
    }
    public bool Shooting => GameInputManager.Instance.IsShooting() && CurAmmo > 0;

    public int MaxAmmo => 100;
    public int ExpThreshold => data.expThreshold.EvaluateStat(CurLevel, maxLevel);
    public int Damage => data.damage.EvaluateStat(CurLevel, maxLevel);
    public int BurnDamage => data.burnDamage.EvaluateStat(CurLevel, maxLevel);
    public int Accuracy => data.accuracy.EvaluateStat(CurLevel, maxLevel);
    public float Range => data.range.EvaluateStat(CurLevel, maxLevel);
    public float Weight => data.weight.EvaluateStat(CurLevel, maxLevel);
    public float FuelConsumptionRate => data.fuelConsumptionRate.EvaluateStat(CurLevel, maxLevel);
    public float FuelReplenishRate => data.fuelReplenishRate.EvaluateStat(CurLevel, maxLevel);
    public float FireDelay => data.fireDelay.EvaluateStat(CurLevel, maxLevel);
    public float BurnDuration => data.burnDuration.EvaluateStat(CurLevel, maxLevel);

    public CharacterStatsData CharStatData { get => charStatData; set => charStatData = value; }
    public CharacterStatsManager CharStatManager { get => charStatManager; set => charStatManager = value; }

    public void Start() {
        exp = 0;
        maxLevel = data.maxLevel;
        CurLevel = data.startLevel;
        CurAmmo = 100;
        emitter.GetComponent<Emitter>().UpdateStats(Damage, Accuracy, FireDelay, Range, (expDrop) => {
            AddExp((int)(expDrop * 1.5f));
            CharStatManager.AddExp(expDrop);
        });
        // InvokeRepeating("Refuel", 0, FireDelay);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.U)) {
            LevelUp();
        }
    }

    public void Refill(int amount) => CurAmmo = Mathf.Clamp(CurAmmo + amount, 0, 100);

    public void Shoot(Vector2 dir) {
        if (CurAmmo <= 5) {
            emitter.GetComponent<Emitter>().StopEmitting();
            return;
        }
        Vector2 start = GetFirePosition(dir);
        emitter.GetComponent<Emitter>().StartEmitting(start, dir);
        emitter.GetComponent<Emitter>().UpdateDirecttion(start, dir);
        if (delayShootCoroutine == null) {
            curFuel = Mathf.Clamp(CurAmmo - FuelConsumptionRate, 0, 100);
            CurAmmo = (int)curFuel;
            delayShootCoroutine = StartCoroutine(DelayShoot());
        }
    }

    public void AbortShoot() {
        emitter.GetComponent<Emitter>().StopEmitting();
    }

    private IEnumerator DelayShoot() {
        yield return new WaitForSeconds(FireDelay);
        delayShootCoroutine = null;
    }

    // private void Refuel() {
    //     if (CurAmmo < 100) {
    //         curFuel = Mathf.Clamp(CurAmmo + FuelReplenishRate, 0, 100);
    //         CurAmmo = (int)curFuel;
    //     }
    // }

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
            CurAmmo = 100;
            emitter.GetComponent<Emitter>().UpdateStats(Damage, Accuracy, FireDelay, Range, (expDrop) => {
                AddExp(expDrop);
                CharStatManager.AddExp(expDrop);
            });
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
