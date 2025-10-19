using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

public class ShotgunBehaviour : MonoBehaviour, IGunBehaviour {
    [SerializeField] private ShotgunDefinition data;
    [SerializeField] private Material shootMaterial;
    [SerializeField] private AudioClip shootAudio;
    [SerializeField] private AudioClip reloadAudio;
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
    public string Name => "shotgun";
    public bool Shooting => GameInputManager.Instance.IsShooting() && curAmmo > 0;

    public int ExpThreshold => data.expThreshold.EvaluateStat(curLevel, maxLevel);
    public int Damage => data.damage.EvaluateStat(curLevel, maxLevel);
    public int Accuracy => data.accuracy.EvaluateStat(curLevel, maxLevel);
    public float Range => data.range.EvaluateStat(curLevel, maxLevel);
    public float Weight => data.weight.EvaluateStat(curLevel, maxLevel);
    public int MaxAmmo => data.maxPackets.EvaluateStat(curLevel, maxLevel) * AmmoPerPack;
    public int AmmoPerPack => data.ammoPerPacket.EvaluateStat(curLevel, maxLevel);
    public float ReloadTime => data.reloadTime.EvaluateStat(curLevel, maxLevel);
    public float FireDelay => data.fireDelay.EvaluateStat(curLevel, maxLevel);
    public int PelletsPerShot => data.pelletsPerShot.EvaluateStat(curLevel, maxLevel);
    public float SpreadAngle => data.spreadAngle.EvaluateStat(curLevel, maxLevel);

    public CharacterStatsData CharStatData { get => charStatData; set => charStatData = value; }
    public CharacterStatsManager CharStatManager { get => charStatManager; set => charStatManager = value; }

    public void Start() {
        exp = 0;
        curLevel = data.startLevel;
        maxLevel = data.maxLevel;
        curAmmo = MaxAmmo;
        packetAmmo = AmmoPerPack;
    }

    public void Shoot(Vector2 dir) {
        if (curAmmo < 1) return;
        Debug.Log(curAmmo);
        if (delayShootCoroutine == null) {
            int c = Mathf.Max(PelletsPerShot, packetAmmo);
            int n = Random.Range(1, c);
            for (int i = 0; i < n; i++) RaycastBullet(dir);
            curAmmo = Mathf.Clamp(curAmmo - n, 0, MaxAmmo);
            if (packetAmmo - n <= 0) {
                delayShootCoroutine = StartCoroutine(DelayShoot(true));
                packetAmmo = Mathf.Min(curAmmo, AmmoPerPack);
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
            MeshHandler.DrawLineMesh(start, end, 0.03f, 0.008f, 0.012f, shootMaterial);

            IStatsManager enemy = collider.GetComponent<IStatsManager>();
            if (enemy != null) {
                enemy.TakeDamage(Damage, charStatData.LUCK + Accuracy, out int expDrop);
                CharStatManager.AddExp(expDrop);
                AddExp((int)(expDrop * 1.5));
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

    private Vector3 GetAudioPosition() {
        float x = transform.position.x, y = transform.position.y;
        float z = Random.Range(-7, 0);
        return new Vector3(x, y, z);
    }
}
