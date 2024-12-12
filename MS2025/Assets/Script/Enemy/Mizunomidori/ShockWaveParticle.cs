using UnityEngine;

class ShockWaveParticle : MonoBehaviour{
	[Tooltip("攻撃発生までの遅延時間を決めます")]
	[SerializeField]
    private Animator parentAnimator;     // 親オブジェクトのAnimator
	[Tooltip("パーティクル発生するアニメーションステートを決めます")]
	[SerializeField]
    private string targetStateName;      // 監視したいステート名
    private int layerIndex = 0;          // 監視するレイヤーのインデックス

    private  TextureAnimation textureAnimation;
    private Animator anim;
    private bool isInTargetState = false;

    private void Start() {
        textureAnimation = GetComponent<TextureAnimation>();
        anim = GetComponent<Animator>();
    }

    private void Update() {
        // 現在のアニメーションステートを取得
        AnimatorStateInfo stateInfo = parentAnimator.GetCurrentAnimatorStateInfo(layerIndex);

        // 特定のステートに入ったことを検知
        if (stateInfo.IsName(targetStateName)) {
            if (!isInTargetState) {
                if (textureAnimation != null) {
                    textureAnimation.StartAnimation();
                    isInTargetState = true;
                }
                else if (anim != null) {
                    anim.SetTrigger("StartAnimation");
                    isInTargetState = true;
                }
            }
        }
        else {
            // gameObject.SetActive(false);
            isInTargetState = false;
        }
    }
}