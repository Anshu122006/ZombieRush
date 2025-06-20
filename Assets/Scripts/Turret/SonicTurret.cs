using System.Collections;
using Game.Utils;
using Unity.VisualScripting;
using UnityEngine;

public class SonicTurret : MonoBehaviour {
    private bool isReloaded;
    private bool searched;
    private int ringsBoomed;
    private float searchRate;

    [SerializeField] Transform sonicRingPref;
    [SerializeField] Transform centre;

    private Transform curTarget;
    private TurretType turretType;

    private bool seeThroughWalls;
    private float ringsPerBoom;
    private int damage;
    private int accuracy;
    private float pushBackForce;
    private float reloadSpeed;
    private float expandTime;
    private float range;

    private void Start() {
        seeThroughWalls = true;
        isReloaded = true;
        searched = false;
        searchRate = 0.1f;
        ringsBoomed = 0;
        ringsPerBoom = 1;
        reloadSpeed = 2f;
        damage = 4;
        pushBackForce = 3;
        accuracy = 10;
        expandTime = 2;
        range = 3;
    }

    private void Update() {
        if (curTarget != null)
            Shoot();
        SearchForEnemies();
    }
    public void SearchForEnemies() {
        if (!searched) {
            FunctionTimer.CreateSceneTimer(() => {
                Vector2 centre = transform.position;
                int layerMask = seeThroughWalls ? LayerMask.GetMask("Enemy") : LayerMask.GetMask("Enemy", "BlockBullets");

                Collider2D hit = Physics2D.OverlapCircle(centre, range, layerMask);
                if (hit != null && hit.CompareTag("Enemy")) {
                    curTarget = hit.transform;
                }
                else {
                    curTarget = null;
                    Debug.Log("No hit");
                }
                searched = false;
            }, searchRate);
            searched = true;
        }
    }

    private void Shoot() {
        if (isReloaded) {
            float time = reloadSpeed;
            if (ringsBoomed < ringsPerBoom - 1)
                time = 0.1f * reloadSpeed;

            FunctionTimer.CreateSceneTimer(() => {
                Transform sonicRing = Instantiate(sonicRingPref);
                StartCoroutine(SonicBoom(sonicRing));
                ringsBoomed = ringsBoomed < ringsPerBoom ? ringsBoomed + 1 : 0;
                isReloaded = true;
            }, time);
            isReloaded = false;
        }
    }

    private IEnumerator SonicBoom(Transform ring) {
        float elapsed = 0;
        float duration = expandTime;
        float sizeFactor = 0.25f;
        float start_alpha = 1, end_alpha = 0;
        Vector2 start_size = Vector2.zero, end_size = Vector2.one * (range + 1) * sizeFactor;
        ring.transform.position = centre.transform.position;
        ring.GetComponent<SonicRing>().Setup(damage, accuracy, pushBackForce);

        while (elapsed < duration) {
            float t = elapsed / duration;
            Vector2 scale = start_size + (end_size - start_size) * Mathf.Sqrt(t);
            float alpha = start_alpha + (end_alpha - start_alpha) * Mathf.Sqrt(t);
            ring.localScale = scale;
            ring.GetComponent<SpriteRenderer>().color = Color.white.WithAlpha(alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(ring.gameObject);
    }
}
