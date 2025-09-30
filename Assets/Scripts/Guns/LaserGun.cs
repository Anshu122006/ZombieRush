// using System.Collections.Generic;
// using Game.Utils;
// using UnityEngine;

// class LaserGun : GunBase {
//     private bool canShoot;
//     private bool charging;
//     private float curCharge;
//     private float maxCharge;
//     private float rechargeTime;
//     public StatField<float, int> dischargeRate;
//     public StatField<float, int> rechargeRate;

//     public float RechargeRate => rechargeRate.init + (rechargeRate.final - rechargeRate.init) * Mathf.Pow((float)CurLevel / MaxLevel, rechargeRate.pow);
//     public float DischargeRate => dischargeRate.init + (dischargeRate.final - dischargeRate.init) * Mathf.Pow((float)CurLevel / MaxLevel, dischargeRate.pow);


//     public override void SetStats(GunSO gunSO) {
//         particlePref = gunSO.particlePref;
//         rechargeRate = gunSO.rechargeRate;
//         dischargeRate = gunSO.dischargeRate;

//         canShoot = true;
//         maxCharge = 100;
//         curCharge = maxCharge;
//     }

//     public override void Shoot(Transform firePoint, Vector2 dir) {
//         if (curCharge < DischargeRate) return;

//         if (canShoot) {
//             ShootLaser(firePoint, dir);
//             if (curCharge < 0) curCharge = 0;
//             FunctionTimer.CreateSceneTimer(() => canShoot = true, ReloadTime, "GunReloadTimer");
//             canShoot = false;
//         }
//     }

//     private void ShootLaser(Transform firePoint, Vector2 dir) {
//         Vector2 start = (Vector2)firePoint.position + dir * 0.1f;

//         Laser laser = GameObject.Instantiate(particlePref).GetComponent<Laser>();
//         laser.transform.position = start;
//         laser.Setup(dir, Damage, Accuracy);
//         curCharge -= DischargeRate;
//         if (curCharge < maxCharge && !charging) StartCharging();
//     }

//     private void StartCharging() {
//         if (curCharge > maxCharge) {
//             if (curCharge != maxCharge) curCharge = maxCharge;
//             if (charging) charging = false;
//             return;
//         }

//         if (!charging) charging = true;
//         FunctionTimer.CreateSceneTimer(() => {
//             curCharge += RechargeRate;
//             StartCharging();
//         }, rechargeTime);
//     }

//     public override void LevelUp() {
//         base.LevelUp();
//         curCharge = maxCharge;
//     }
// }