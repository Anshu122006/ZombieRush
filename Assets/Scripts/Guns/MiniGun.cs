using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

class MiniGun : GunBase {
    private bool canShoot;
    private int curAmmo;
    private int sign;
    public StatField<int, int> maxAmmo;
    public StatField<float, int> particleSpeed;

    public int MaxAmmo => (int)(maxAmmo.init + (maxAmmo.final - maxAmmo.init) * Mathf.Pow((float)CurLevel / MaxLevel, maxAmmo.pow));
    public float ParticleSpeed => particleSpeed.init + (particleSpeed.final - particleSpeed.init) * Mathf.Pow((float)CurLevel / MaxLevel, particleSpeed.pow);


    public override void SetStats(GunSO gunSO) {
        particlePref = gunSO.particlePref;
        maxAmmo = gunSO.maxAmmo;
        range = gunSO.range;
        deviation = gunSO.deviation;
        particleSpeed = gunSO.particleSpeed;

        canShoot = true;
        sign = 1;
        curAmmo = MaxAmmo;
    }

    public override void Shoot(Transform firePoint, Vector2 dir) {
        if (curAmmo < 1) return;

        if (canShoot) {
            ShootBullet(firePoint, dir);
            curAmmo--;
            if (curAmmo < 0) curAmmo = 0;
            FunctionTimer.CreateSceneTimer(() => canShoot = true, ReloadTime, "GunReloadTimer");
            canShoot = false;
        }
    }

    private void ShootBullet(Transform firePoint, Vector2 dir) {
        Vector2 offset = Vector3.Cross(Vector3.forward, dir).normalized * Random.Range(0, Deviation) * sign;
        Vector2 start = (Vector2)firePoint.position + offset + dir * 0.1f;

        Bullet bullet = GameObject.Instantiate(particlePref).GetComponent<Bullet>();
        bullet.transform.position = start;
        bullet.Setup(dir, ParticleSpeed, Range, Damage, Accuracy);
        sign *= -1;
    }

    public override void LevelUp() {
        base.LevelUp();
        curAmmo = MaxAmmo;
    }
}