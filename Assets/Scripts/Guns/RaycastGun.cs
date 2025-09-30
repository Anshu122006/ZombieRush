// using System.Collections.Generic;
// using Game.Utils;
// using UnityEngine;

// class RaycastGun : GunBase {
//     private bool canShoot;
//     private int curAmmo;
//     public StatField<int, int> maxAmmo;
//     public StatField<int, int> particlesPerShot;

//     public int MaxAmmo => (int)(maxAmmo.init + (maxAmmo.final - maxAmmo.init) * Mathf.Pow((float)CurLevel / MaxLevel, maxAmmo.pow));
//     public int ParticlesPerShot => (int)(particlesPerShot.init + (particlesPerShot.final - particlesPerShot.init) * Mathf.Pow((float)CurLevel / MaxLevel, particlesPerShot.pow));


    // public override void SetStats(GunSO gunSO) {
    //     maxAmmo = gunSO.maxAmmo;
    //     particlesPerShot = gunSO.particlesPerShot;
    //     range = gunSO.range;
    //     deviation = gunSO.deviation;

    //     canShoot = true;
    //     curAmmo = MaxAmmo;
    // }

    // public override void Shoot(Transform firePoint, Vector2 dir) {
    //     if (MaxAmmo > 0 && curAmmo < 1) return;

    //     if (canShoot) {
    //         int n = ParticlesPerShot;
    //         if (MaxAmmo > 0) n = curAmmo > n ? n : curAmmo;

    //         for (int i = 0; i < n; i++) {
    //             RaycastBullet(firePoint, dir);
    //             curAmmo--;
    //         }
    //         if (MaxAmmo > 0 && curAmmo < 0) curAmmo = 0;
    //         FunctionTimer.CreateSceneTimer(() => canShoot = true, ReloadTime, "GunReloadTimer");
    //         canShoot = false;
    //     }
    // }

    // private void RaycastBullet(Transform firePoint, Vector2 dir) {
    //     dir = VectorHandler.GenerateRandomDir(dir, Deviation);
    //     Vector2 start = (Vector2)firePoint.position + dir * 0.1f;
    //     int layerMask = LayerMask.GetMask("BlockBullet", "Enemy");

    //     RaycastHit2D hit = Physics2D.Raycast(start, dir, Range, layerMask);
    //     Collider2D collider = hit.collider;

    //     if (collider != null) {
    //         Vector2 end = hit.point;
    //         MeshHandler.DrawLineMesh(start, end, 0.1f);

    //         IStats enemy = collider.GetComponent<IStats>();
    //         if (enemy != null) {
    //             enemy.TakeDamage(Damage, Accuracy);
    //         }
    //     }
    //     else {
    //         Vector2 end = start + dir * Range;
    //         MeshHandler.DrawLineMesh(start, end, 0.1f);
    //     }
    // }

//     public override void LevelUp() {
//         base.LevelUp();
//         curAmmo = MaxAmmo;
//     }
// }