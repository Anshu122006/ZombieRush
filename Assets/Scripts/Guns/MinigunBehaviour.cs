using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

public class MinigunBehaviour : MonoBehaviour, IGunBehaviour {
    [SerializeField] private Transform bulletPref;
    [SerializeField] private MinigunDefinition data;
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
    public string Name => "minigun";
    public bool Shooting => GameInputManager.Instance.IsShooting() && curAmmo > 0;

    public int ExpThreshold => data.expThreshold.EvaluateStat(curLevel, maxLevel);
    public int Damage => data.damage.EvaluateStat(curLevel, maxLevel);
    public int Accuracy => data.accuracy.EvaluateStat(curLevel, maxLevel);
    public float Range => data.range.EvaluateStat(curLevel, maxLevel);
    public float Weight => data.weight.EvaluateStat(curLevel, maxLevel);
    public int AmmoPerPack => data.ammoPerPacket.EvaluateStat(curLevel, maxLevel);
    public int MaxAmmo => data.maxPackets.EvaluateStat(curLevel, maxLevel) * AmmoPerPack;
    public int AmmoPerSpin => data.ammoPerPacket.EvaluateStat(curLevel, maxLevel);
    public float ProjectileSpeed => data.projectileSpeed.EvaluateStat(curLevel, maxLevel);
    public float FireDelay => data.fireDelay.EvaluateStat(curLevel, maxLevel);
    public float ReloadTime => data.reloadTime.EvaluateStat(curLevel, maxLevel);
    public float Offset => data.offset;

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
        if (delayShootCoroutine == null) {
            Vector2 start = GetFirePosition(dir);
            Vector2 off = new Vector2(dir.y, -dir.x).normalized * Random.Range(-Offset, Offset);
            Vector2 spawnPoint = start + off;

            Transform bullet = Instantiate(bulletPref);
            bullet.position = spawnPoint;
            bullet.GetComponent<Bullet>().Setup(dir, ProjectileSpeed, Range, Damage, Accuracy);

            curAmmo = Mathf.Clamp(curAmmo - 1, 0, MaxAmmo);
            if (packetAmmo - 1 <= 0) {
                delayShootCoroutine = delayShootCoroutine = StartCoroutine(DelayShoot(true));
                packetAmmo = Mathf.Min(curAmmo, AmmoPerPack);
            }
            else {
                delayShootCoroutine = delayShootCoroutine = StartCoroutine(DelayShoot(false));
                packetAmmo--;
            }
        }
    }

    public void AbortShoot() { }

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
            this.exp = gain;
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
