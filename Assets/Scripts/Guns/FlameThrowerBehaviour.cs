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
    public string Name => "flamethrower";
    public bool Shooting => GameInputManager.Instance.IsShooting() && curFuel > 0;

    public int ExpThreshold => data.expThreshold.EvaluateStat(curLevel, maxLevel);
    public int Damage => data.damage.EvaluateStat(curLevel, maxLevel);
    public int BurnDamage => data.burnDamage.EvaluateStat(curLevel, maxLevel);
    public int Accuracy => data.accuracy.EvaluateStat(curLevel, maxLevel);
    public float Range => data.range.EvaluateStat(curLevel, maxLevel);
    public float Weight => data.weight.EvaluateStat(curLevel, maxLevel);
    public float FuelConsumptionRate => data.fuelConsumptionRate.EvaluateStat(curLevel, maxLevel);
    public float FuelReplenishRate => data.fuelReplenishRate.EvaluateStat(curLevel, maxLevel);
    public float FireDelay => data.fireDelay.EvaluateStat(curLevel, maxLevel);
    public float BurnDuration => data.burnDuration.EvaluateStat(curLevel, maxLevel);

    public CharacterStatsData CharStatData { get => charStatData; set => charStatData = value; }
    public CharacterStatsManager CharStatManager { get => charStatManager; set => charStatManager = value; }

    public void Start() {
        exp = 0;
        curLevel = data.startLevel;
        maxLevel = data.maxLevel;
        curFuel = 100;
        emitter.GetComponent<Emitter>().UpdateStats(Damage, Accuracy, FireDelay, Range, (expDrop) => {
            AddExp((int)(expDrop * 1.5f));
            CharStatManager.AddExp(expDrop);
        });
        InvokeRepeating("Refuel", 0, FireDelay);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.U)) {
            LevelUp();
        }
    }

    public void Shoot(Vector2 dir) {
        if (curFuel <= 5) {
            emitter.GetComponent<Emitter>().StopEmitting();
            return;
        }
        Vector2 start = GetFirePosition(dir);
        emitter.GetComponent<Emitter>().StartEmitting(start, dir);
        if (delayShootCoroutine == null) {
            curFuel = Mathf.Clamp(curFuel - FuelConsumptionRate, 0, 100);
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

    private void Refuel() {
        if (curFuel < 100) curFuel = Mathf.Clamp(curFuel + FuelReplenishRate, 0, 100);
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
            curFuel = 100;
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
}
