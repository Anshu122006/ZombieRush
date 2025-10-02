using System.Collections;
using Game.Utils;
using Unity.VisualScripting;
using UnityEngine;

public class SonicTurret : MonoBehaviour,ITurretBehaviour {
    [SerializeField] private Transform center;
    [SerializeField] private GameObject ringPref;
    [SerializeField] private SonicTurretDefinition data;

    private int curLevel;
    private int maxLevel;
    private Transform curTarget;
    private Coroutine shootCoroutine;

    public int CurLevel => curLevel;
    public int UpgradeCost => data.upgradeCost.EvaluateStat(curLevel, maxLevel);
    public string Name => data.turretName;

    public int Damage => data.damage.EvaluateStat(curLevel, maxLevel);
    public int Accuracy => data.accuracy.EvaluateStat(curLevel, maxLevel);
    public int RingsPerBoom => data.ringsPerBoom.EvaluateStat(curLevel, maxLevel);
    public float RingSpeed => data.ringSpeed.EvaluateStat(curLevel, maxLevel);
    public float PushbackForce => data.pushbackForce.EvaluateStat(curLevel, maxLevel);
    public float Range => data.range.EvaluateStat(curLevel, maxLevel);
    public float ShootDelay => data.shootDelay.EvaluateStat(curLevel, maxLevel);
    public float FireDelay => data.fireDelay.EvaluateStat(curLevel, maxLevel);


    private void Start() {
        curLevel = 1;
        maxLevel = data.maxLevel;
        InvokeRepeating("SearchForEnemies", 0, data.searchRate);
    }

    private void Update() {
        if (shootCoroutine == null && curTarget != null)
            shootCoroutine = StartCoroutine(Shoot());
    }
    public void SearchForEnemies() {
        Vector2 center = transform.position;

        int layerMask = data.seeThroughWalls ? LayerMask.GetMask("Enemy") : LayerMask.GetMask("Enemy", "BlockBullets");
        Collider2D hit = Physics2D.OverlapCircle(center, Range, layerMask);

        if (hit != null && (LayerMask.GetMask("Enemy") & (1 << hit.gameObject.layer)) != 0) curTarget = hit.transform;
        else curTarget = null;
    }

    private IEnumerator Shoot() {
        for (int i = 0; i < RingsPerBoom; i++) {
            Transform ring = Instantiate(ringPref).transform;
            StartCoroutine(SonicBoom(ring));

            yield return new WaitForSeconds(FireDelay);
        }

        yield return new WaitForSeconds(ShootDelay);
        shootCoroutine = null;
    }

    private IEnumerator SonicBoom(Transform ring) {
        float sizeFactor = 0.25f;
        float start_alpha = 1, end_alpha = 0;
        float cur_size = 0;
        float end_size = Range * sizeFactor;

        ring.position = center.position;
        ring.GetComponent<SonicRing>().Setup(Damage, Accuracy, PushbackForce);

        while (cur_size < end_size) {
            float t = cur_size / end_size;
            Vector2 scale = Vector2.one * end_size * Mathf.Sqrt(t);
            float alpha = start_alpha + (end_alpha - start_alpha) * Mathf.Sqrt(t);

            ring.localScale = scale;
            ring.GetComponent<SpriteRenderer>().color = Color.white.WithAlpha(alpha);

            cur_size += Time.deltaTime * RingSpeed;
            yield return null;
        }

        Destroy(ring.gameObject);
    }
    public void LevelUp() {
        if (curLevel < maxLevel) {
            curLevel++;
        }
    }
}
