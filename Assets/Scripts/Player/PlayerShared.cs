using UnityEngine;

public class PlayerShared : MonoBehaviour {
    public Transform visual;
    public Transform healthbar;
    public Rigidbody2D rb;
    public Collider2D collider2d;
    public Animator animator;

    public PlayerMovement move;
    public PlayerAnimation anim;
    public PlayerAttack attack;


    private void Start() {
        move = GetComponent<PlayerMovement>();
        anim = GetComponent<PlayerAnimation>();
        attack = GetComponent<PlayerAttack>();
    }
}
