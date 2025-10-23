using UnityEngine;

[CreateAssetMenu(fileName = "Grenade", menuName = "Guns/Grenade")]
public class GrenadeDefinition : GunDefinition {
    public StatField<int, int> maxAmmo = new(3, 8, 1);
    public StatField<float, int> projectileSpeed = new(10f, 20f, 1);
    public StatField<float, int> explosionRadius = new(2f, 4f, 1);
}
