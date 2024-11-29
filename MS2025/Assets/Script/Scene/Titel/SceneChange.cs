using UnityEngine;

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
    private float time;

    private void Update() {
        if (time <= 0.0f /*&& gameLauncher.IsAnimation()*/ && openFlag) {
            swithcActive.IsActive();
            gameLauncher.Open();
            openFlag = false;
            time = delayTime;
        }
        if (time <= 0.0f && /*gameLauncher.IsAnimation() &&*/ closeFlag) {
            swithcActive.DisActive();
            gameLauncher.Open();
            closeFlag = false;
            time = delayTime;
        }
        time -= Time.deltaTime;
    }

    public void Cutaway() {
        openFlag = true;
        time = delayTime;
        gameLauncher.Close();
    }
    public void Back() {
        closeFlag = true;
        time = delayTime;
        gameLauncher.Close();
    }
}
