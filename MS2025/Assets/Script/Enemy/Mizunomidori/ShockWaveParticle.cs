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
    private bool isInTargetState = false;

    private void Start() {
        textureAnimation = GetComponent<TextureAnimation>();
    }

    private void Update() {
        // 現在のアニメーションステートを取得
        AnimatorStateInfo stateInfo = parentAnimator.GetCurrentAnimatorStateInfo(layerIndex);

        // 特定のステートに入ったことを検知
        if (stateInfo.IsName(targetStateName)) {
            if (!isInTargetState) {
                // gameObject.SetActive(true);
                textureAnimation.StartAnimation();
                isInTargetState = true;
            }
        }
        else {
            // gameObject.SetActive(false);
            isInTargetState = false;
        }
    }
}