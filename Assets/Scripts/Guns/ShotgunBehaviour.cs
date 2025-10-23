using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

public class ShotgunBehaviour : MonoBehaviour, IGunBehaviour {
    [SerializeField] private ShotgunDefinition data;
    [SerializeField] private Transform shellPref;
    [SerializeField] private Material shootMaterial;
    [SerializeField] private AudioClip shootAudio;
    [SerializeField] private AudioClip reloadAudio;
    [SerializeField] private Transform flashPref;
    [SerializeField] private GunFirePoint firePoint;

    private CharacterStatsData charStatData;
    private CharacterStatsManager charStatManager;
    private Coroutine delayShootCoroutine;
    private int maxLevel;

    // Runtime values
    private int exp;
    private int curAmmo;
    private int packetAmmo;
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
    public float Range => data.range.EvaluateStat(CurLevel, maxLevel);
    public float Weight => data.weight.EvaluateStat(CurLevel, maxLevel);
    public int MaxAmmo => data.maxPackets.EvaluateStat(CurLevel, maxLevel) * AmmoPerPack;
    public int AmmoPerPack => data.ammoPerPacket.EvaluateStat(CurLevel, maxLevel);
    public float ReloadTime => data.reloadTime.EvaluateStat(CurLevel, maxLevel);
    public float FireDelay => data.fireDelay.EvaluateStat(CurLevel, maxLevel);
    public int PelletsPerShot => data.pelletsPerShot.EvaluateStat(CurLevel, maxLevel);
    public float SpreadAngle => data.spreadAngle.EvaluateStat(CurLevel, maxLevel);

    public CharacterStatsData CharStatData { get => charStatData; set => charStatData = value; }
    public CharacterStatsManager CharStatManager { get => charStatManager; set => charStatManager = value; }

    public void Awake() {
        exp = 0;
        maxLevel = data.maxLevel;
        CurLevel = data.startLevel;
        CurAmmo = MaxAmmo;
        packetAmmo = AmmoPerPack;
    }

    public void Refill(int amount) {
        CurAmmo = Mathf.Clamp(CurAmmo + amount, 0, MaxAmmo);
        Debug.Log(CurAmmo);
    }

    public void Shoot(Vector2 dir) {
        if (CurAmmo < 1) return;
        Debug.Log(CurAmmo);
        if (delayShootCoroutine == null) {
            int c = Mathf.Max(PelletsPerShot, packetAmmo);
            int n = Random.Range(1, c);
            ShowMuzzleFlash(dir);
            for (int i = 0; i < n; i++) {
                RaycastBullet(dir);
                Transform shell = Instantiate(shellPref);
                shell.position = transform.position;
                shell.GetComponent<BulletShell>().Setup();
            }
            CurAmmo = Mathf.Clamp(CurAmmo - n, 0, MaxAmmo);
            if (packetAmmo - n <= 0) {
                delayShootCoroutine = StartCoroutine(DelayShoot(true));
                packetAmmo = Mathf.Min(CurAmmo, AmmoPerPack);
            }
            else {
                delayShootCoroutine = StartCoroutine(DelayShoot(false));
                packetAmmo -= n;
            }
        }
    }

    public void AbortShoot() { }

    private void RaycastBullet(Vector2 dir) {
        Vector2 start = GetFirePosition(dir);

        dir = VectorHandler.GenerateRandomDir(dir, SpreadAngle);
        int layerMask = LayerMask.GetMask("BlockBullet", "Enemy");

        RaycastHit2D hit = Physics2D.Raycast(start, dir, Range, layerMask);
        Collider2D collider = hit.collider;

        if (collider != null) {
            Vector2 end = hit.point;
            MeshHandler.DrawLineMesh(start, end, 0.03f, 0.01f, 0.016f, shootMaterial, "OnChar");

            IStatsManager enemy = collider.GetComponent<IStatsManager>();
            if (enemy != null) {
                enemy.HandleHitEffects();
                enemy.TakeDamage(Damage, charStatData.LUCK + Accuracy, out int expDrop, transform);
                CharStatManager.AddExp(expDrop);
                AddExp((int)(expDrop * 1.5));
            }
        }
        else {
            Vector2 end = start + dir * Range;
            MeshHandler.DrawLineMesh(start, end, 0.03f, 0.01f, 0.016f, shootMaterial, "OnChar");
        }
    }

    private IEnumerator DelayShoot(bool reload) {
        GameAudioManager.Instance.PlaySound(shootAudio, GetAudioPosition());
        yield return new WaitForSeconds(FireDelay);

        if (reload) {
            GameAudioManager.Instance.PlaySound(reloadAudio, GetAudioPosition(), 1, reloadAudio.length / ReloadTime);
            yield return new WaitForSeconds(ReloadTime);
        }
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
            HudManager.Instance?.ShowLog(data.gunName + " upgraded to Lv" + CurLevel);
        }
    }

    private void ShowMuzzleFlash(Vector2 dir) {
        Vector2 start = GetFirePosition(dir);
        Transform flash = Instantiate(flashPref, start, Quaternion.identity);
        flash.GetComponent<MuzzleFlash>().Setup(0.03f, dir);
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
