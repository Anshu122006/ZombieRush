using UnityEngine;
public abstract class TurretDefinition : ScriptableObject {
    [Header("Base Properties")]
    public Transform effectPref;

    [Header("Progression")]
    public int maxLevel;
    public float searchRate;
    public float rotationSpeed;
    public bool seeThroughWalls;

    [Header("Core Stats")]
    public StatField<int, int> damage;
    public StatField<int, int> accuracy;
    public StatField<int, int> range;
}

[CreateAssetMenu(fileName = "ProjectileTurret", menuName = "Turrets/ProjectileTurret")]
public class ProjectileTurretDefinition : TurretDefinition {
    public StatField<float, int> reloadRate;
    public StatField<int, int> maxAmmo;
    public StatField<float, int> shootDelay;
    public StatField<float, int> fireDelay;
    public StatField<float, int> projectileSpeed;
    public StatField<int, int> pelletsPerShot;
}

[CreateAssetMenu(fileName = "SonicTurret", menuName = "Turrets/SonicTurret")]
public class SonicTurretDefinition : TurretDefinition {
    public StatField<int, int> ringsPerBoom;
    public StatField<float, int> ringSpeed;
    public StatField<float, int> fireDelay;
    public StatField<float, int> shootDelay;
    public StatField<float, int> pushbackForce;
}

[CreateAssetMenu(fileName = "MissileTurret", menuName = "Turrets/MissileTurret")]
public class MissileTurretDefinition : TurretDefinition {
    public StatField<float, int> reloadRate;
    public StatField<int, int> maxAmmo;
    public StatField<float, int> shootDelay;
    public StatField<float, int> projectileSpeed;
    public StatField<float, int> projectileRange;
}

[CreateAssetMenu(fileName = "LaserTurret", menuName = "Turrets/LaserTurret")]
public class LaserTurretDefinition : TurretDefinition {
    public StatField<float, int> dischargeRate;
    public StatField<float, int> rechargeRate;
    public StatField<float, int> fireDelay;
}

[CreateAssetMenu(fileName = "ElectricTurret", menuName = "Turrets/ElectricTurret")]
public class ElectricTurretDefinition : TurretDefinition {
    public StatField<int, int> maxCharge;
    public StatField<int, int> chargesPerShot;
    public StatField<float, int> reloadTime;
    public StatField<float, int> shootDelay;
}
