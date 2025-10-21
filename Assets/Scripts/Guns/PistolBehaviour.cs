using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

public class PistolBehaviour : MonoBehaviour, IGunBehaviour {
    [SerializeField] private PistolDefinition data;
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
    private int curLevel;
    private int packetAmmo;

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
    public int CurAmmo { get => -1; set { } }
    public int MaxAmmo => -1;
    public int PacketAmmo => packetAmmo;
    public bool Shooting => GameInputManager.Instance.IsShooting();

    public int ExpThreshold => data.expThreshold.EvaluateStat(CurLevel, maxLevel);
    public int Damage => data.damage.EvaluateStat(CurLevel, maxLevel);
    public int Accuracy => data.accuracy.EvaluateStat(CurLevel, maxLevel);
    public float Range => data.range.EvaluateStat(CurLevel, maxLevel);
    public int AmmoPerPack => data.ammoPerPacket.EvaluateStat(CurLevel, maxLevel);
    public float ReloadTime => data.reloadTime.EvaluateStat(CurLevel, maxLevel);
    public float Weight => data.weight.EvaluateStat(CurLevel, maxLevel);
    public float FireDelay => data.fireDelay.EvaluateStat(CurLevel, maxLevel);
    public float SpreadAngle => data.spreadAngle.EvaluateStat(CurLevel, maxLevel);

    public CharacterStatsData CharStatData { get => charStatData; set => charStatData = value; }
    public CharacterStatsManager CharStatManager { get => charStatManager; set => charStatManager = value; }

    public void Awake() {
        exp = 0;
        maxLevel = data.maxLevel;
        CurLevel = data.startLevel;
        packetAmmo = AmmoPerPack;
    }

    public void Refill(int amount) { }

    public void Shoot(Vector2 dir) {
        if (delayShootCoroutine == null) {
            ShowMuzzleFlash(dir);
            RaycastBullet(dir);

            Transform shell = Instantiate(shellPref);
            shell.position = transform.position;
            shell.GetComponent<BulletShell>().Setup();

            if (packetAmmo - 1 <= 0) {
                delayShootCoroutine = StartCoroutine(DelayShoot(true));
                packetAmmo = AmmoPerPack;
            }
            else {
                delayShootCoroutine = StartCoroutine(DelayShoot(false));
                packetAmmo--;
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
            MeshHandler.DrawLineMesh(start, end, 0.03f, 0.008f, 0.012f, shootMaterial);

            IStatsManager enemy = collider.GetComponent<IStatsManager>();
            if (enemy != null) {
                enemy.HandleHitEffects();
                enemy.TakeDamage(Damage, charStatData.LUCK + Accuracy, out int expDrop);
                CharStatManager.AddExp(expDrop);
                AddExp((int)(expDrop * 1.5f));
            }
        }
        else {
            Vector2 end = start + dir * Range;
            MeshHandler.DrawLineMesh(start, end, 0.03f, 0.008f, 0.012f, shootMaterial);
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
            packetAmmo = AmmoPerPack;
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
