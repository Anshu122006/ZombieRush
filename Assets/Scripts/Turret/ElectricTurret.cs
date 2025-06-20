using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricTurret : MonoBehaviour {
    private bool searched;
    private bool isReloaded;
    private bool hasTargets;
    private int curCharges;
    private float searchRate;

    [SerializeField] Transform effect1;
    [SerializeField] Transform effect2;
    [SerializeField] Transform enemyEffect;
    [SerializeField] Transform centre;

    private List<Transform> curTargets;
    private TurretType turretType;

    private bool seeThroughWalls;
    private int maxCharges;
    private int damage;
    private int accuracy;
    private float reloadSpeed;
    private float range;

    private void Start() {
        seeThroughWalls = true;
        isReloaded = true;
        hasTargets = false;
        searched = false;
        searchRate = 0.1f;
        reloadSpeed = 2f;
        maxCharges = 12;
        curCharges = 12;
        damage = 20;
        accuracy = 10;
        range = 4;
    }

    private void Update() {
        if (hasTargets) Shoot();
        SearchForEnemies();
    }
    public void SearchForEnemies() {
        if (!searched) {
            FunctionTimer.CreateSceneTimer(() => {
                Vector2 centre = transform.position;
                int layerMask = seeThroughWalls ? LayerMask.GetMask("Enemy") : LayerMask.GetMask("Enemy", "BlockBullets");

                Collider2D[] hits = Physics2D.OverlapCircleAll(centre, range, layerMask);
                if (hits != null) {
                    hasTargets = false;
                    curTargets = new List<Transform>();
                    foreach (Collider2D hit in hits) {
                        if (hit.CompareTag("Enemy")) {
                            hasTargets = true;
                            curTargets.Add(hit.transform);
                        }
                    }
                }
                else {
                    hasTargets = false;
                    Debug.Log("No hit");
                }
                searched = false;
            }, searchRate);
            searched = true;
        }
    }

    private void Shoot() {
        if (curCharges <= 0)
            return;

        if (isReloaded) {
            FunctionTimer.CreateSceneTimer(() => {
                Transform elec1 = Instantiate(effect1);
                Transform elec2 = Instantiate(effect2);
                elec1.position = centre.position;
                elec2.position = centre.position;
                elec1.localScale = Vector3.one * range * 0.6f;
                elec2.localScale = Vector3.one * range * 0.8f;

                foreach (Transform target in curTargets) {
                    Transform enemyElec = Instantiate(enemyEffect, target);
                    enemyElec.localPosition = new Vector3(0, 0, -6);
                    Stats enemy = target.GetComponent<Stats>();
                    enemy.TakeDamage(damage, accuracy);
                    curCharges--;
                }

                curCharges = curCharges < 0 ? 0 : curCharges;
                isReloaded = true;
            }, reloadSpeed);
            isReloaded = false;
        }
    }
}
