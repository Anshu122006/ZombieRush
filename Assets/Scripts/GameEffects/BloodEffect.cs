using UnityEngine;

public class BloodEffect : MonoBehaviour {
    private void Start() {
        Animator animator = GetComponent<Animator>();
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        AnimationClip clip = clipInfo[0].clip;

        Destroy(gameObject, clip.length - 0.03f);
    }
}
