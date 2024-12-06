using UnityEngine;
using System.Collections;

public class SceneChange : MonoBehaviour {
    public float delayTime;
    [Header("スクリプト設定")]
	[Tooltip("スイッチアクティブを決めます")]
	[SerializeField]
    private SwitchActive swithcActive;
	[Tooltip("ゲームランチャーを決めます")]
	[SerializeField]
    private GameLauncher gameLauncher;

    private bool openFlag = false; //閉じてるならfalse 開いてるならtrue
    private bool animationFlag = false;
    private bool isCloseFlag = false;
    private bool isActiveFlag;
    private float time;

    private void Update() {
        if (time <= 0.0f && !gameLauncher.IsAnimation() && openFlag && isActiveFlag) {
            swithcActive.IsActive();
            gameLauncher.Open();
            openFlag = false;
        }
        if (time <= 0.0f && !gameLauncher.IsAnimation() && openFlag && !isActiveFlag) {
            swithcActive.DisActive();
            gameLauncher.Open();
            openFlag = false;
        }
        time -= Time.deltaTime;

        if (!gameLauncher.IsAnimation() && !openFlag) {
            // アニメーションが終わったらフラグを折る
            animationFlag = false;
        }
    }

    public void Cutaway() {
        if (openFlag && animationFlag) return;
        gameLauncher.Close();
        time = delayTime;
        openFlag = true;
        animationFlag = true;
        isActiveFlag = true;
    }
    public void Back() {
        if (openFlag && animationFlag) return;
        gameLauncher.Close();
        time = delayTime;
        openFlag = true;
        animationFlag = true;
        isActiveFlag = false;
    }

}
