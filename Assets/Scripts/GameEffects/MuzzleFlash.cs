using System.Collections;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour {
    public void Setup(float destroyTime, Vector2 dir) {
        StartCoroutine(DestroyCoroutine(destroyTime));
        if (dir == Vector2.up) GetComponent<SpriteRenderer>().sortingLayerName = "BelowChar";
        transform.right = dir;
    }

    private IEnumerator DestroyCoroutine(float time) {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
