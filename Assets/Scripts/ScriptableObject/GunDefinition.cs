using UnityEngine;

public abstract class GunDefinition : ScriptableObject {
    [Header("Base Properties")]
    public string gunName = "Unnamed Gun";
    public Sprite gunSprite;

    [Header("Progression")]
    public int startLevel = 1;
    public int maxLevel = 10;

    [Header("Core Stats")]
    public StatField<int, int> expThreshold = new(100, 2000, 1);
    public StatField<int, int> damage = new(10, 100, 1);
    public StatField<int, int> accuracy = new(60, 95, 1);
    public StatField<int, int> range = new(20, 100, 1);
    public StatField<float, int> fireDelay = new(0.6f, 0.2f, 1);
    public StatField<float, int> weight = new(5f, 3f, 1);
}

[CreateAssetMenu(fileName = "Pistol", menuName = "Guns/Pistol")]
public class PistolDefinition : GunDefinition {
    public StatField<float, int> spreadAngle = new(3f, 1f, 1);
    public StatField<int, int> ammoPerPacket = new(12, 15, 1);
    public StatField<float, int> reloadTime = new(1.5f, 1f, 1);
}

[CreateAssetMenu(fileName = "SMG", menuName = "Guns/SMG")]
public class SmgDefinition : GunDefinition {
    public StatField<int, int> maxPackets = new(4, 6, 1);
    public StatField<int, int> ammoPerPacket = new(25, 40, 1);
    public StatField<float, int> reloadTime = new(2f, 1.2f, 1);
    public StatField<float, int> spreadAngle = new(6f, 3f, 1);
}

[CreateAssetMenu(fileName = "Shotgun", menuName = "Guns/Shotgun")]
public class ShotgunDefinition : GunDefinition {
    public StatField<int, int> maxPackets = new(3, 5, 1);
    public StatField<int, int> ammoPerPacket = new(6, 8, 1);
    public StatField<float, int> reloadTime = new(2.5f, 1.5f, 1);
    public StatField<int, int> pelletsPerShot = new(6, 12, 1);
    public StatField<float, int> spreadAngle = new(10f, 5f, 1);
}

[CreateAssetMenu(fileName = "Grenade", menuName = "Guns/Grenade")]
public class GrenadeDefinition : GunDefinition {
    public StatField<int, int> maxAmmo = new(3, 8, 1);
    public StatField<float, int> projectileSpeed = new(10f, 20f, 1);
    public StatField<float, int> explosionRadius = new(2f, 4f, 1);
}

[CreateAssetMenu(fileName = "Minigun", menuName = "Guns/Minigun")]
public class MinigunDefinition : GunDefinition {
    public StatField<int, int> maxPackets = new(5, 10, 1);
    public StatField<int, int> ammoPerPacket = new(100, 200, 1);
    public StatField<float, int> reloadTime = new(3f, 1.5f, 1);
    public StatField<float, int> projectileSpeed = new(15f, 25f, 1);
    public float offset = 0.2f;
}

[CreateAssetMenu(fileName = "FlameThrower", menuName = "Guns/FlameThrower")]
public class FlameThrowerDefinition : GunDefinition {
    public StatField<int, int> burnDamage = new(5, 20, 1);
    public StatField<float, int> fuelConsumptionRate = new(1f, 0.5f, 1);
    public StatField<float, int> fuelReplenishRate = new(0.3f, 0.6f, 1);
    public StatField<float, int> burnDuration = new(1.5f, 3f, 1);
}
