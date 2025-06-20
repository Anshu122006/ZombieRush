using UnityEngine;

public class PlayerShared : MonoBehaviour {
    [SerializeField] private Transform firePointRight;
    [SerializeField] private Transform firePointLeft;
    [SerializeField] private Transform firePointUp;
    [SerializeField] private Transform firePointDown;
    [SerializeField] private SpriteRenderer gunImageRight;
    [SerializeField] private SpriteRenderer gunImageLeft;
    [SerializeField] private SpriteRenderer gunImageUp;
    [SerializeField] private SpriteRenderer gunImageDown;
    public Transform visual;
    public Transform healthbar;
    public Collider2D pCollider;
    public Vector3 playerDir;
    public float baseMoveSpeed = 8;
    public float moveSpeed = 8;

    public Transform firePoint {
        get {
            Vector2 dir = playerDir;
            if (dir == Vector2.right)
                return firePointRight;
            else if (dir == Vector2.left)
                return firePointLeft;
            else if (dir == Vector2.up)
                return firePointUp;
            else
                return firePointDown;
        }
        private set { }
    }

    private void Start() {
        playerDir = Vector2.right;
    }

    public void SetGunImage(Gun gun) {
        firePointRight.gameObject.SetActive(true);
        firePointLeft.gameObject.SetActive(true);
        firePointUp.gameObject.SetActive(true);
        firePointDown.gameObject.SetActive(true);

        gunImageUp.sprite = gun.imageUp;
        gunImageRight.sprite = gun.imageRight;
        gunImageDown.sprite = gun.imageDown;
        gunImageLeft.sprite = gun.imageLeft;

        firePointRight.gameObject.SetActive(false);
        firePointLeft.gameObject.SetActive(false);
        firePointUp.gameObject.SetActive(false);
        firePointDown.gameObject.SetActive(false);
    }

    public void UpdateGunDirection() {
        firePointRight.gameObject.SetActive(false);
        firePointLeft.gameObject.SetActive(false);
        firePointUp.gameObject.SetActive(false);
        firePointDown.gameObject.SetActive(false);

        Vector2 dir = playerDir;
        if (dir == Vector2.right)
            firePointRight.gameObject.SetActive(true);
        else if (dir == Vector2.left)
            firePointLeft.gameObject.SetActive(true);
        else if (dir == Vector2.up)
            firePointUp.gameObject.SetActive(true);
        else
            firePointDown.gameObject.SetActive(true);
    }
}
