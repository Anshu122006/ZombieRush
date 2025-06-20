using System;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    private GameInputManager input;
    private PlayerShared shared;
    private List<Gun> gunList;
    private int currGun;
    private float moveSpeedWithGun;
    [SerializeField] private GunListSO initGunList;
    [SerializeField] private GameObject pfBullet;

    private void Start() {
        input = GameInputManager.Instance;
        shared = GetComponent<PlayerShared>();
        input.OnTestPerformed += OnTest;
        input.OnPrevWeaponPerformed += OnPrevWeapon;
        input.OnNextWeaponPerformed += OnNextWeapon;

        gunList = new List<Gun>();
        gunList.Add(Gun.Create(initGunList.Pistol));
        gunList.Add(Gun.Create(initGunList.Smg));
        gunList.Add(Gun.Create(initGunList.Shotgun));
        moveSpeedWithGun = shared.baseMoveSpeed - gunList[currGun].weight;
        shared.moveSpeed = moveSpeedWithGun;
    }

    private void Update() {
        if (input.IsShooting()) {
            if (shared.moveSpeed == moveSpeedWithGun)
                shared.moveSpeed = (moveSpeedWithGun) * 0.6f;
            Shoot();
        }
        else {
            if (shared.moveSpeed != moveSpeedWithGun)
                shared.moveSpeed = moveSpeedWithGun;
        }
        if (Input.GetKeyDown(KeyCode.U)) {
            gunList[currGun].LevelUp();
        }
    }

    private void Shoot() {
        PlayerMovement data = GetComponent<PlayerMovement>();
        gunList[currGun].Shoot(shared.firePoint, shared.playerDir);
    }

    private void OnPrevWeapon(System.Object sender, EventArgs e) {
        if (currGun > 0) {
            currGun--;
            shared.SetGunImage(gunList[currGun]);
            Debug.Log(gunList[currGun].gunType);
            moveSpeedWithGun = shared.baseMoveSpeed - gunList[currGun].weight;
            shared.moveSpeed = moveSpeedWithGun;
        }
    }

    private void OnNextWeapon(System.Object sender, EventArgs e) {
        if (currGun < gunList.Count - 1) {
            currGun++;
            shared.SetGunImage(gunList[currGun]);
            Debug.Log(gunList[currGun].gunType);
            moveSpeedWithGun = shared.baseMoveSpeed - gunList[currGun].weight;
            shared.moveSpeed = moveSpeedWithGun;
        }
    }

    private void OnTest(System.Object sender, EventArgs e) {
        
    }
}
