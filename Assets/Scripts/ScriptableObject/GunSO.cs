using UnityEngine;
using Game.Utils;

[CreateAssetMenu(fileName = "GunSO", menuName = "Scriptable Objects/GunSO")]
public class GunSO : ScriptableObject {
    public GunType gunType;
    public Transform effectPref;
    public Transform particlePref;
    public Sprite visual;
    public int curLevel;
    public int maxLevel;
    public StatField<int, int> damage;
    public StatField<int, int> accuracy;
    public StatField<int, int> maxAmmo;
    public StatField<int, int> particlesPerShot;
    public StatField<float, int> range;
    public StatField<float, int> deviation;
    public StatField<float, int> reloadTime;
    public StatField<float, int> particleSpeed;
    public StatField<float, int> dischargeRate;
    public StatField<float, int> rechargeRate;
    public StatField<float, int> weight;
}
