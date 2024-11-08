using UnityEngine;

public class TextureAnimation : MonoBehaviour
{
    [Header("表示エフェクト設定")]
	[Tooltip("アニメーション画像を決めます")]
	[SerializeField]
    private Texture2D[] textures;
	[Tooltip("アニメーションのフレームレートを決めます")]
	[SerializeField]
    private float frameRate = 0.1f;
	[Tooltip("ループの有無を決めます")]
	[SerializeField]
    private bool isLoop = true;
	[Tooltip("開始時に再生するかを決めます")]
	[SerializeField]
    private bool playOnStart = true;

    private Renderer rend;
    private int currentFrame;
    private float timer;
    private bool isPlaying = false;

    void Start() {
        rend = GetComponent<Renderer>();
        if (playOnStart) {
            StartAnimation();
        }
    }

    void Update() {
        if (!isPlaying || textures.Length == 0) return;

        timer += Time.deltaTime;

        // フレームを切り替える
        if (timer >= frameRate) {
            timer -= frameRate;
            currentFrame++;

            // 最後のフレームに達した場合の処理
            if (currentFrame >= textures.Length) {
                if (isLoop) {
                    currentFrame = 0; // ループする場合は最初に戻る
                }
                else {
                    StopAnimation(); // ループしない場合は停止
                    return;
                }
            }

            // 現在のフレームのテクスチャを設定
            rend.material.mainTexture = textures[currentFrame];
        }
    }

    // アニメーションの再生開始
    public void StartAnimation() {
        isPlaying = true;
        currentFrame = 0;
        timer = 0;
        rend.material.mainTexture = textures[currentFrame];
    }

    // アニメーションの停止
    public void StopAnimation() {
        isPlaying = false;
    }
}
