using UnityEngine;
[CreateAssetMenu(menuName = "Definitions/Gun Definition", fileName = "GunDefinition")]
public abstract class GunDefinition : ScriptableObject {
    [Header("Base Properties")]
    public string gunName;
    public Transform effectPref;
    public Transform particlePref;
    public Sprite visual;

    [Header("Progression")]
    public int startLevel;
    public int maxLevel;

    [Header("Core Stats")]
    public StatField<int, int> expThreshold;
    public StatField<int, int> damage;
    public StatField<int, int> accuracy;
    public StatField<int, int> range;
    public StatField<float, int> weight;
}

[CreateAssetMenu(fileName = "PistolDefinition", menuName = "Scriptable Objects/Guns/Pistol")]
public class PistolDefinition : GunDefinition {
    public StatField<int, int> maxAmmo;
    public StatField<float, int> reloadTime;
    public StatField<float, int> fireRate;
}

[CreateAssetMenu(fileName = "SMGDefinition", menuName = "Scriptable Objects/Guns/SMG")]
public class SMGDefinition : GunDefinition {
    public StatField<int, int> maxAmmo;
    public StatField<float, int> reloadTime;
    public StatField<float, int> fireRate;
}

[CreateAssetMenu(fileName = "ShotgunDefinition", menuName = "Scriptable Objects/Guns/Shotgun")]
public class ShotgunDefinition : GunDefinition {
    public StatField<int, int> maxAmmo;
    public StatField<float, int> reloadTime;
    public StatField<int, int> pelletsPerShot;
    public StatField<float, int> spreadAngle;
}

[CreateAssetMenu(fileName = "BazookaDefinition", menuName = "Scriptable Objects/Guns/Bazooka")]
public class BazookaDefinition : GunDefinition {
    public StatField<int, int> maxAmmo;
    public StatField<float, int> reloadTime;
    public StatField<float, int> projectileSpeed;
    public StatField<float, int> explosionRadius;
}

[CreateAssetMenu(fileName = "MinigunDefinition", menuName = "Scriptable Objects/Guns/Minigun")]
public class MinigunDefinition : GunDefinition {
    public StatField<int, int> maxAmmo;
    public StatField<float, int> reloadTime;
    public StatField<float, int> spinUpTime;
    public StatField<float, int> fireRate;
}

[CreateAssetMenu(fileName = "LaserGunDefinition", menuName = "Scriptable Objects/Guns/LaserGun")]
public class LaserGunDefinition : GunDefinition {
    public StatField<float, int> rechargeRate;
    public StatField<float, int> dischargeRate;
    public StatField<float, int> maxBeamDuration;
}


