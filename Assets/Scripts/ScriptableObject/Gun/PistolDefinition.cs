using UnityEngine;

[CreateAssetMenu(fileName = "Pistol", menuName = "Guns/Pistol")]
public class PistolDefinition : GunDefinition {
    public StatField<float, int> spreadAngle = new(3f, 1f, 1);
    public StatField<int, int> ammoPerPacket = new(12, 15, 1);
    public StatField<float, int> reloadTime = new(1.5f, 1f, 1);
}
