using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

public class CharacterAttack : MonoBehaviour {
    [SerializeField] private CharacterStatsManager player;
    [SerializeField] private CharacterComponents components;
    private CharacterGunHandler gunHandler;
    private CharacterMovement movement;

    private void Start() {
        gunHandler = components.gunHandler;
        movement = components.movement;
    }

    private void Update() {
        if (gunHandler.Gun.Shooting) Shoot();
        else gunHandler.Gun.AbortShoot();
    }

    private void Shoot() {
        if ((Vector2)gunHandler.transform.right != movement.faceDir)
            gunHandler.transform.right = movement.faceDir;
        gunHandler.Gun.Shoot(movement.faceDir);
    }
}
