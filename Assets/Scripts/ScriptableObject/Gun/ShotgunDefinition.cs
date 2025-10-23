using UnityEngine;

[CreateAssetMenu(fileName = "Shotgun", menuName = "Guns/Shotgun")]
public class ShotgunDefinition : GunDefinition {
    public StatField<int, int> maxPackets = new(3, 5, 1);
    public StatField<int, int> ammoPerPacket = new(6, 8, 1);
    public StatField<float, int> reloadTime = new(2.5f, 1.5f, 1);
    public StatField<int, int> pelletsPerShot = new(6, 12, 1);
    public StatField<float, int> spreadAngle = new(10f, 5f, 1);
}
