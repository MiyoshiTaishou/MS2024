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
    
    private bool openFlag = false;
    private bool closeFlag = false;
    private bool animationFlag = false;
    private bool isCloseFlag = false;
    private float time;

    private void Update() {
        // if (time <= 0.0f && gameLauncher.IsAnimation() && openFlag) {
        if (time <= 0.0f && openFlag) {
            // Debug.LogWarning("ドアを開けて-");
            swithcActive.IsActive();
            gameLauncher.Open();
            openFlag = false;
            time = delayTime;
            isCloseFlag = false;
            // Coroutine();
        }
        // if (time <= 0.0f && gameLauncher.IsAnimation() && closeFlag) {
        if (time <= 0.0f && closeFlag) {
            // Debug.LogWarning("ドアを開けて-");
            swithcActive.DisActive();
            gameLauncher.Open();
            closeFlag = false;
            time = delayTime;
            isCloseFlag = false;
            // Coroutine();
        }
        if (!(time <= 0.0f) && (!openFlag && !closeFlag && !isCloseFlag) && animationFlag) {
            // ドアが閉じて開くまでの間にAボタンを押されるとバグる
            animationFlag = false;
        }
        time -= Time.deltaTime;
    }

    public void Cutaway() {
        if (openFlag && closeFlag && animationFlag == true) return;
        openFlag = true;
        time = delayTime;
        gameLauncher.Close();
        animationFlag = true;
        isCloseFlag = true;
        Coroutine();
    }
    public void Back() {
        if (openFlag && closeFlag && animationFlag == true) return;
        closeFlag = true;
        time = delayTime;
        gameLauncher.Close();
        animationFlag = true;
        Coroutine();
    }

    public IEnumerator Coroutine() {
        yield return new WaitForSeconds(1.5f);
        openFlag = false;
        closeFlag = false;
        gameLauncher.Open();
        animationFlag = false;
    }
}
