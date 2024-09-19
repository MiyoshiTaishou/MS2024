using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    // ヒットストップを発動するメソッド
    public void ApplyHitStop(float hitStopDuration)
    {
        StartCoroutine(DoHitStop(hitStopDuration));
    }

    private IEnumerator DoHitStop(float hitStopDuration)
    {
        // 現在のタイムスケールを保存
        float originalTimeScale = Time.timeScale;

        // タイムスケールを0にして停止状態を作る
        Time.timeScale = 0.0f;

        // hitStopDuration秒待機 (実際の時間での待機)
        yield return new WaitForSecondsRealtime(hitStopDuration);

        // タイムスケールを元に戻す
        Time.timeScale = originalTimeScale;
    }
}
