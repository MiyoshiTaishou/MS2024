using UnityEngine;

public class LocalPlayer : MonoBehaviour
{
	// [Header("プレイヤー設定")]

	[Tooltip("プレイヤーの体力を決めます")]
	[SerializeField]
	public float HP;
	[Tooltip("ダメージを受けているときの点滅回数を決めます")]
	[SerializeField]
	private int flashCount = 5;
	private int nowCount;   // 現在の点滅回数
	[Tooltip("ダメージを受けているときの点滅する間隔を決めます")]
	[SerializeField]
	private float flashInterval = 0.2f;
	[Tooltip("ダメージを受けているときの色を決めます")]
	[SerializeField]
	private Color damageColor;
	private Color originalColor;
	private SpriteRenderer spriteRenderer;
	private FLASH_STATE flashState; // スプライトのカラー
	[Tooltip("プレイヤーがホストであるかを決めます\n(現在は手動 いずれ自動で切り替わるように)")]
	[SerializeField]
	public bool isHost = true;
	private float nowTime;

	private void Awake() {}
	
	private void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		originalColor = spriteRenderer.color;
		nowCount = flashCount;
	}
	private void Update() {
		// 点滅処理
		if(nowCount < flashCount) {
			if (nowTime >= flashInterval && flashState == FLASH_STATE.DAMAGE){
				spriteRenderer.color = damageColor;
				flashState = FLASH_STATE.ORIGINAL;
				nowTime = 0;
			}
			else if (nowTime >= flashInterval && flashState == FLASH_STATE.ORIGINAL){
				spriteRenderer.color = originalColor;
				flashState = FLASH_STATE.DAMAGE;
				nowTime = 0;
				nowCount++;
			}
		}
		nowTime += Time.deltaTime;
	}

	public void FlashReset() {
		if (nowCount == flashCount) {
			spriteRenderer.color = damageColor;
			flashState = FLASH_STATE.ORIGINAL;
			nowTime = 0;
		}
		nowCount = 0;
	}
}
