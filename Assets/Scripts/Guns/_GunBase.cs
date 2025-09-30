// using System.Collections.Generic;
// using Game.Utils;
// using UnityEngine;

// public abstract class GunBase {
//     public GunType gunType { get; protected set; }
//     public Transform effectPref;
//     public Transform particlePref;
//     public Sprite visual;

//     public int CurLevel { get; protected set; }
//     public int MaxLevel { get; protected set; }
//     protected StatField<int, int> damage;
//     protected StatField<int, int> accuracy;
//     protected StatField<float, int> range;
//     protected StatField<float, int> deviation;
//     protected StatField<float, int> reloadTime;
//     protected StatField<float, int> weight;

//     public int Damage => (int)(damage.init + (damage.final - damage.init) * Mathf.Pow((float)CurLevel / MaxLevel, damage.pow));
//     public int Accuracy => (int)(accuracy.init + (accuracy.final - accuracy.init) * Mathf.Pow((float)CurLevel / MaxLevel, accuracy.pow));
//     public float Range => range.init + (range.final - range.init) * Mathf.Pow((float)CurLevel / MaxLevel, range.pow);
//     public float Deviation => deviation.init + (deviation.final - deviation.init) * Mathf.Pow((float)CurLevel / MaxLevel, deviation.pow);
//     public float ReloadTime => reloadTime.init + (reloadTime.final - reloadTime.init) * Mathf.Pow((float)CurLevel / MaxLevel, reloadTime.pow);
//     public float Weight => weight.init + (weight.final - weight.init) * Mathf.Pow((float)CurLevel / MaxLevel, weight.pow);

//     public static GunBase Create(GunDefinition gunSO) {
//         if (gunSO == null)
//             return null;

//         GunBase gun = null;
//         switch (gunSO.gunType) {
//             case GunType.LaserGun:
//                 gun = new LaserGun();
//                 break;
//             case GunType.MiniGun:
//                 gun = new MiniGun();
//                 break;
//             case GunType.Bazuca:
//                 gun = new Bazuca();
//                 break;
//             default:
//                 gun = new RaycastGun();
//                 break;
//         }

//         if (gun != null) {
//             gun.gunType = gunSO.gunType;
//             gun.effectPref = gunSO.effectPref;
//             gun.visual = gunSO.visual;
//             gun.CurLevel = gunSO.curLevel;
//             gun.MaxLevel = gunSO.maxLevel;
//             gun.damage = gunSO.damage;
//             gun.accuracy = gunSO.accuracy;
//             gun.reloadTime = gunSO.reloadTime;

//             gun.SetStats(gunSO);
//         }
//         return gun;
//     }

//     public virtual void LevelUp() {
//         if (CurLevel < MaxLevel) CurLevel++;
//     }

//     public virtual void Shoot(Transform firePoint, Vector2 playerDir) { }
//     public virtual void SetStats(GunSO gunSO) { }
// }