using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class GrenadeVisual : MonoBehaviour {
    [SerializeField] private Transform visual;
    [SerializeField] private Transform shadow;
    [SerializeField] private Grenade grenade;

    private float shadowHeightDivider = 6;

    private void Update() {
        UpdateProjectileDirection();
        UpdateShadowPos();
    }

    private void UpdateShadowPos() {
        Vector2 newPos = transform.position;
        Vector2 trajectoryRange = grenade.targetPos - grenade.start;
        if (Mathf.Abs(trajectoryRange.normalized.x) > Mathf.Abs(trajectoryRange.normalized.y))
            newPos.y = grenade.start.y + grenade.GetNextYPos() / shadowHeightDivider + grenade.GetNextYPosCorrectionAbsolute();
        else
            newPos.x = grenade.start.x + grenade.GetNextXPos() / shadowHeightDivider + grenade.GetNextXPosCorrectionAbsolute();
        shadow.position = newPos;
    }

    private void UpdateProjectileDirection() {
        Vector2 moveDIr = grenade.GetMoveDir();
        visual.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(moveDIr.y, moveDIr.x) * Mathf.Rad2Deg);
        shadow.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(moveDIr.y, moveDIr.x) * Mathf.Rad2Deg);
    }
}
