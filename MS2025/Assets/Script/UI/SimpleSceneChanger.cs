using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneChanger : MonoBehaviour
{
    [Header("設定")]
    public string targetScene; // 切り替えるシーン名
    public bool useAButtonInput = true; // Submitで反応するかどうか
    public bool useBButtonInput = false; // Cancelで反応するかどうか

    void Update() {
        // Submitボタンでシーン変更
        if (useAButtonInput && Input.GetButtonDown("Submit")) {
            ChangeScene();
        }

        // Cancelボタンでシーン変更
        if (useBButtonInput && Input.GetButtonDown("Cancel")) {
            ChangeScene();
        }
    }

    // シーン変更を実行する
    private void ChangeScene() {
        if (!string.IsNullOrEmpty(targetScene)) {
            SceneManager.LoadScene(targetScene);
        }
        else {
            Debug.LogError("ターゲットシーンが設定されていません。");
        }
    }
}
