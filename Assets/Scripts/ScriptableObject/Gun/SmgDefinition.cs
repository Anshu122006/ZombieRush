using UnityEngine;

[CreateAssetMenu(fileName = "SMG", menuName = "Guns/SMG")]
public class SmgDefinition : GunDefinition {
    public StatField<int, int> maxPackets = new(4, 6, 1);
    public StatField<int, int> ammoPerPacket = new(25, 40, 1);
    public StatField<float, int> reloadTime = new(2f, 1.2f, 1);
    public StatField<float, int> spreadAngle = new(6f, 3f, 1);
}
