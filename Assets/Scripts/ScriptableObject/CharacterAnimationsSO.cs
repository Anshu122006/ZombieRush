using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAnimationsSO", menuName = "Animations/Character")]
public class CharacterAnimationsSO : ScriptableObject {
    public DirectionAnimation pistol;
    public DirectionAnimation smg;
    public DirectionAnimation shotgun;
    public DirectionAnimation grenade;
    public DirectionAnimation minigun;
    public DirectionAnimation flamethrower;
}
