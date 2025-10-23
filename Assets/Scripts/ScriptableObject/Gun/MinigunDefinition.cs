using UnityEngine;

[CreateAssetMenu(fileName = "Minigun", menuName = "Guns/Minigun")]
public class MinigunDefinition : GunDefinition {
    public StatField<int, int> maxPackets = new(5, 10, 1);
    public StatField<int, int> ammoPerPacket = new(100, 200, 1);
    public StatField<float, int> reloadTime = new(3f, 1.5f, 1);
    public StatField<float, int> projectileSpeed = new(15f, 25f, 1);
    public float offset = 0.2f;
}
