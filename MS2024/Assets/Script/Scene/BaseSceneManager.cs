using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseSceneManager : MonoBehaviour
{
    public SceneInfoObject currentSceneInfo;

    private void Start()
    {
        StartCoroutine(FadeIn(currentSceneInfo));
    }

    public void LoadScene(SceneInfoObject newSceneInfo)
    {
        StartCoroutine(HandleSceneTransition(newSceneInfo));
    }

    private IEnumerator HandleSceneTransition(SceneInfoObject newSceneInfo)
    {
        // 現在のシーンのフェードアウトを開始
        yield return StartCoroutine(FadeOut(currentSceneInfo));

        // 次のシーン情報を設定
        currentSceneInfo = newSceneInfo;

        // シーンの変更
        SceneManager.LoadScene(newSceneInfo.SceneName);      
    }

    // フェードアウト処理
    protected virtual IEnumerator FadeOut(SceneInfoObject sceneInfo)
    {
        // デフォルトのフェードアウト処理
        yield return new WaitForSeconds(1.0f);
    }

    // フェードイン処理
    protected virtual IEnumerator FadeIn(SceneInfoObject sceneInfo)
    {
        // デフォルトのフェードイン処理
        yield return new WaitForSeconds(1.0f);
    }
}
