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
        if (components.statsManager.InReviveStage) return;
        if (gunHandler.Gun.Shooting) Shoot();
        else gunHandler.Gun.AbortShoot();
    }

    private void Shoot() {
        gunHandler.Gun.Shoot(movement.faceDir);
    }
}
