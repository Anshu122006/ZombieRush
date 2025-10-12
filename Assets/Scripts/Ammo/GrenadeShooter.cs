using UnityEngine;

public class GrenadeShooter : MonoBehaviour {
    [SerializeField] private Transform grenadePref;
    [SerializeField] private Transform tempTarget;
    [SerializeField] private float shootRate = 1;
    [SerializeField] private float projectileSpeed = 10;
    [SerializeField] private float baseTrajectoryHeight = 0.3f;

    [SerializeField] private AnimationCurve trajectoryAnimationCurve;
    [SerializeField] private AnimationCurve axisCorrectionAnimationCurve;
    [SerializeField] private AnimationCurve speedAnimationCurve;

    private float elapsed = 0;
    void Update() {
        if (elapsed > shootRate) {
            // Grenade grenade = Instantiate(grenadePref, transform.position, Quaternion.identity).GetComponent<Grenade>();
            // grenade.InitializeProperties(tempTarget.position, projectileSpeed, baseTrajectoryHeight);
            // grenade.InitializeAnimationCurves(trajectoryAnimationCurve, axisCorrectionAnimationCurve, speedAnimationCurve);
            // elapsed = 0;
        }
        else {
            elapsed += Time.deltaTime;
        }
    }
}
