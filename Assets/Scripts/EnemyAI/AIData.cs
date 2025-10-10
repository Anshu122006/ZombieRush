using System.Collections.Generic;
using UnityEngine;

public class AIData : MonoBehaviour {
    public List<Transform> targets = null;
    public Collider2D[] obstacles = null;
    public Transform currentTarget;
    public Vector2 curDir;
    public string curState;
    public string attackPhase;

    public int GetTargetCount() => targets == null ? 0 : targets.Count;
}
