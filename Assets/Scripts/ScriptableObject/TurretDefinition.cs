using UnityEngine;

public abstract class TurretDefinition : ScriptableObject {
    [Header("Base Properties")]
    public string turretName;
    public Sprite sprite;
    [TextArea] public string turretDesc;
    public Transform effectPref;

    [Header("Progression")]
    public int maxLevel = 10;
    public int cost = 100;
    public float searchRate = 1.0f;
    public float rotationSpeed = 180f;

    [Header("Core Stats")]
    public StatField<int, int> damage = new(10, 50, 1);
    public StatField<int, int> accuracy = new(70, 95, 1);
    public StatField<int, int> mhp = new(100, 300, 1);
    public StatField<int, int> def = new(5, 30, 1);
    public StatField<float, int> range = new(3f, 8f, 1);
    public StatField<int, int> upgradeCost = new(100, 500, 1);
    public StatField<int, int> expThreshold = new(10, 30, 1);
    public StatField<int, int> expDrop = new(5, 20, 1);
}

[CreateAssetMenu(fileName = "ProjectileTurret", menuName = "Turrets/ProjectileTurret")]
public class ProjectileTurretDefinition : TurretDefinition {
    public StatField<float, int> reloadRate = new(1.5f, 0.5f, 1);
    public StatField<int, int> maxAmmo = new(6, 12, 1);
    public StatField<float, int> shootDelay = new(0.3f, 0.1f, 1);
    public StatField<float, int> fireDelay = new(0.5f, 0.2f, 1);
    public StatField<float, int> projectileSpeed = new(6f, 15f, 1);
    public StatField<int, int> pelletsPerShot = new(1, 5, 1);
}

[CreateAssetMenu(fileName = "SonicTurret", menuName = "Turrets/SonicTurret")]
public class SonicTurretDefinition : TurretDefinition {
    public StatField<int, int> ringsPerBoom = new(3, 8, 1);
    public StatField<float, int> ringSpeed = new(5f, 12f, 1);
    public StatField<float, int> fireDelay = new(0.8f, 0.3f, 1);
    public StatField<float, int> shootDelay = new(0.4f, 0.15f, 1);
    public StatField<float, int> pushbackForce = new(3f, 10f, 1);
}

[CreateAssetMenu(fileName = "MissileTurret", menuName = "Turrets/MissileTurret")]
public class MissileTurretDefinition : TurretDefinition {
    public StatField<float, int> reloadRate = new(2.0f, 0.6f, 1);
    public StatField<int, int> maxAmmo = new(2, 6, 1);
    public StatField<float, int> shootDelay = new(1.0f, 0.4f, 1);
    public StatField<float, int> projectileSpeed = new(8f, 20f, 1);
    public StatField<float, int> projectileRange = new(5f, 15f, 1);
}

[CreateAssetMenu(fileName = "LaserTurret", menuName = "Turrets/LaserTurret")]
public class LaserTurretDefinition : TurretDefinition {
    public StatField<float, int> dischargeRate = new(1.0f, 3.0f, 1);
    public StatField<float, int> rechargeRate = new(0.5f, 2.0f, 1);
    public StatField<float, int> fireDelay = new(0.2f, 0.05f, 1);
}

[CreateAssetMenu(fileName = "ElectricTurret", menuName = "Turrets/ElectricTurret")]
public class ElectricTurretDefinition : TurretDefinition {
    public StatField<int, int> maxCharge = new(3, 10, 1);
    public StatField<int, int> chargesPerShot = new(1, 3, 1);
    public StatField<float, int> reloadTime = new(1.5f, 0.5f, 1);
    public StatField<float, int> shootDelay = new(0.3f, 0.1f, 1);
}
