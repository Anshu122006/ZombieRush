using System.Security.Cryptography;

using UnityEngine;

public class PlayerAttack : AttackBase {
    [SerializeField] private bool isRanged;
    [SerializeField] private float waitTime;
    [SerializeField] private GameObject arrowPref;

    [SerializeField] private Transform shoot_up;
    [SerializeField] private Transform shoot_down;
    [SerializeField] private Transform shoot_left;
    [SerializeField] private Transform shoot_right;

    private PlayerShared shared;
    private int state = 1;

    private void Start() {
        shared = GetComponent<PlayerShared>();
    }
}
