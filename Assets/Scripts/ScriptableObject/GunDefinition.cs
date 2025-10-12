using UnityEngine;
public abstract class GunDefinition : ScriptableObject {
    [Header("Base Properties")]
    public string gunName;

    [Header("Progression")]
    public int startLevel;
    public int maxLevel;

    [Header("Core Stats")]
    public StatField<int, int> expThreshold;
    public StatField<int, int> damage;
    public StatField<int, int> accuracy;
    public StatField<int, int> range;
    public StatField<float, int> fireDelay;
    public StatField<float, int> weight;
}

[CreateAssetMenu(fileName = "Pistol", menuName = "Guns/Pistol")]
public class PistolDefinition : GunDefinition {
    public StatField<float, int> spreadAngle;
    public StatField<int, int> ammoPerPacket;
    public StatField<float, int> reloadTime;
}

[CreateAssetMenu(fileName = "SMG", menuName = "Guns/SMG")]
public class SmgDefinition : GunDefinition {
    public StatField<int, int> maxPackets;
    public StatField<int, int> ammoPerPacket;
    public StatField<float, int> reloadTime;
    public StatField<float, int> spreadAngle;
}

[CreateAssetMenu(fileName = "Shotgun", menuName = "Guns/Shotgun")]
public class ShotgunDefinition : GunDefinition {
    public StatField<int, int> maxPackets;
    public StatField<int, int> ammoPerPacket;
    public StatField<float, int> reloadTime;
    public StatField<int, int> pelletsPerShot;
    public StatField<float, int> spreadAngle;
}

[CreateAssetMenu(fileName = "Grenade", menuName = "Guns/Grenade")]
public class GrenadeDefinition : GunDefinition {
    public StatField<int, int> maxAmmo;
    public StatField<float, int> projectileSpeed;
    public StatField<float, int> explosionRadius;
}

[CreateAssetMenu(fileName = "Minigun", menuName = "Guns/Minigun")]
public class MinigunDefinition : GunDefinition {
    public StatField<int, int> maxPackets;
    public StatField<int, int> ammoPerPacket;
    public StatField<float, int> reloadTime;
    public StatField<float, int> projectileSpeed;
    public float offset;
}

[CreateAssetMenu(fileName = "FlameThrower", menuName = "Guns/FlameThrower")]
public class FlameThrowerDefinition : GunDefinition {
    public StatField<int, int> burnDamage;
    public StatField<float, int> fuelConsumptionRate;
    public StatField<float, int> fuelReplenishRate;
    public StatField<float, int> burnDuration;
}


