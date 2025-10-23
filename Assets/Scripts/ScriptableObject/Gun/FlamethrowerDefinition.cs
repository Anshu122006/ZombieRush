using UnityEngine;

[CreateAssetMenu(fileName = "Flamethrower", menuName = "Guns/FlameThrower")]
public class FlamethrowerDefinition : GunDefinition {
    public StatField<int, int> burnDamage = new(5, 20, 1);
    public StatField<float, int> fuelConsumptionRate = new(1f, 0.5f, 1);
    public StatField<float, int> burnDuration = new(1.5f, 3f, 1);
}
