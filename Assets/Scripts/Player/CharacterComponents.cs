using UnityEngine;

public class CharacterComponents : MonoBehaviour {
    public Transform visual;
    public Transform healthbar;
    public Rigidbody2D rb;
    public Collider2D collider2d;
    public Animator animator;

    public CharacterAnimation animations;
    public CharacterMovement movement;
    public CharacterAttack attack;
    public CharacterGunHandler gunHandler;
    public CharacterStats stats;
    public CharacterStatsManager statsManager;
}
