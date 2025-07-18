using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

class Bazuca : GunBase {
    private bool canShoot;
    private int curAmmo;
    public StatField<int, int> maxAmmo;
    public StatField<float, int> particleSpeed;

    public int MaxAmmo => (int)(maxAmmo.init + (maxAmmo.final - maxAmmo.init) * Mathf.Pow((float)CurLevel / MaxLevel, maxAmmo.pow));
    public float ParticleSpeed => particleSpeed.init + (particleSpeed.final - particleSpeed.init) * Mathf.Pow((float)CurLevel / MaxLevel, particleSpeed.pow);


    public override void SetStats(GunSO gunSO) {
        particlePref = gunSO.particlePref;
        maxAmmo = gunSO.maxAmmo;
        particleSpeed = gunSO.particleSpeed;
        range = gunSO.range;
        deviation = gunSO.deviation;

        canShoot = true;
        curAmmo = MaxAmmo;
    }

    public override void Shoot(Transform firePoint, Vector2 dir) {
        if (curAmmo < 1) return;

        if (canShoot) {

            ShootMissile(firePoint, dir);
            curAmmo--;
            if (curAmmo < 0) curAmmo = 0;
            FunctionTimer.CreateSceneTimer(() => canShoot = true, ReloadTime, "GunReloadTimer");
            canShoot = false;
        }
    }

    private void ShootMissile(Transform firePoint, Vector2 dir) {

        Vector2 start = (Vector2)firePoint.position + dir * 0.1f;
        Transform target = new GameObject("Target").transform;
        target.position = start + dir * Range;

        Missile missile = GameObject.Instantiate(particlePref).GetComponent<Missile>();
        missile.transform.position = start;
        missile.Setup(target, ParticleSpeed * 0.1f, ParticleSpeed, Range, Damage, Accuracy, false);
    }

    public override void LevelUp() {
        base.LevelUp();
        curAmmo = MaxAmmo;
    }
}