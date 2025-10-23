using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAnimationsSO", menuName = "Animations/Character")]
public class CharacterAnimationsSO : ScriptableObject {
    public DirectionAnimation pistol_idle;
    public DirectionAnimation smg_idle;
    public DirectionAnimation shotgun_idle;
    public DirectionAnimation grenade_idle;
    public DirectionAnimation minigun_idle;
    public DirectionAnimation flamethrower_idle;

    public DirectionAnimation pistol_walk;
    public DirectionAnimation smg_walk;
    public DirectionAnimation shotgun_walk;
    public DirectionAnimation grenade_walk;
    public DirectionAnimation minigun_walk;
    public DirectionAnimation flamethrower_walk;

    public AnimationClip death_left;
    public AnimationClip death_right;

    public Dictionary<string, DirectionAnimation> GetIdleAnimations() {
        Dictionary<string, DirectionAnimation> anims = new();
        anims["pistol"] = pistol_idle;
        anims["smg"] = smg_idle;
        anims["shotgun"] = shotgun_idle;
        anims["grenade"] = grenade_idle;
        anims["minigun"] = minigun_idle;
        anims["flamethrower"] = flamethrower_idle;

        return anims;
    }

    public Dictionary<string, DirectionAnimation> GetWalkAnimations() {
        Dictionary<string, DirectionAnimation> anims = new();
        anims["pistol"] = pistol_walk;
        anims["smg"] = smg_walk;
        anims["shotgun"] = shotgun_walk;
        anims["grenade"] = grenade_walk;
        anims["minigun"] = minigun_walk;
        anims["flamethrower"] = flamethrower_walk;

        return anims;
    }
}
