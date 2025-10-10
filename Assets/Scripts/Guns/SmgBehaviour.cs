using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

public class SmgBehaviour : MonoBehaviour, IGunBehaviour {
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SmgDefinition data;
    [SerializeField] private Transform muzzleFlashPref;
    [SerializeField] private Material shootMaterial;
    [SerializeField] private AudioClip shootAudio;

    private CharacterStatsData charStatData;
    private CharacterStatsManager charStatManager;
    private Coroutine delayShootCoroutine;
    private int maxLevel;

    // Runtime values
    private int exp;
    private int curAmmo;
    private int curLevel;

    // getters
    public string Name => "smg";
    public bool Shooting => GameInputManager.Instance.IsShooting() && curAmmo > 0;

    public int ExpThreshold => data.expThreshold.EvaluateStat(curLevel, maxLevel);
    public int Damage => data.damage.EvaluateStat(curLevel, maxLevel);
    public int Accuracy => data.accuracy.EvaluateStat(curLevel, maxLevel);
    public float Range => data.range.EvaluateStat(curLevel, maxLevel);
    public float Weight => data.weight.EvaluateStat(curLevel, maxLevel);
    public int MaxAmmo => data.maxAmmo.EvaluateStat(curLevel, maxLevel);
    public float FireDelay => data.fireDelay.EvaluateStat(curLevel, maxLevel);
    public float SpreadAngle => data.spreadAngle.EvaluateStat(curLevel, maxLevel);

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
            RaycastBullet(dir);
            curAmmo--;
            curAmmo = Mathf.Clamp(curAmmo, 0, MaxAmmo);
            delayShootCoroutine = StartCoroutine(DelayShoot());
        }
    }

    public void AbortShoot() { }

    private void RaycastBullet(Vector2 dir) {
        Vector2 start = transform.position;

        Transform flash = Instantiate(muzzleFlashPref, start, Quaternion.identity);
        flash.GetComponent<MuzzleFlash>().Setup(0.03f, dir);

        audioSource.PlayOneShot(shootAudio);

        dir = VectorHandler.GenerateRandomDir(dir, SpreadAngle);
        int layerMask = LayerMask.GetMask("BlockBullet", "Enemy");

        RaycastHit2D hit = Physics2D.Raycast(start, dir, Range, layerMask);
        Collider2D collider = hit.collider;

        if (collider != null) {
            Vector2 end = hit.point;
            MeshHandler.DrawLineMesh(start, end, 0.03f, 0.008f, 0.012f, shootMaterial);

            IStatsManager enemy = collider.GetComponent<IStatsManager>();
            if (enemy != null) {
                enemy.TakeDamage(Damage, charStatData.LUCK + Accuracy, charStatManager);
                AddExp(enemy.ExpDrop);
            }
        }
        else {
            Vector2 end = start + dir * Range;
            MeshHandler.DrawLineMesh(start, end, 0.03f, 0.008f, 0.012f, shootMaterial);
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
            curAmmo = MaxAmmo;
        }
    }
}
