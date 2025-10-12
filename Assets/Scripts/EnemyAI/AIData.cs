using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIData : MonoBehaviour {
    public List<Transform> targets = null;
    public Collider2D[] obstacles = null;
    public Transform currentTarget;
    public Vector2 curDir;
    public string curState;
    public string attackPhase;

    public int GetTargetCount => targets == null ? 0 : targets.Count;
    public void SetCurrentTarget() {
        if (GetTargetCount == 0) return;
        targets = targets.OrderBy(target => Vector2.Distance(target.position, transform.position)).ToList();
        for (int i = 0; i < targets.Count; i++) {
            if ((LayerMask.GetMask("Player") & (1 << targets[i].gameObject.layer)) != 0) {
                currentTarget = targets[i];
                Debug.Log(targets[i].gameObject.layer);
                return;
            }
        }
        currentTarget = targets[0];
    }
}
