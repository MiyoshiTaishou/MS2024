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
       // Debug.Log("ストップ");

        // hitStopDuration秒待機 (実際の時間での待機)
        yield return new WaitForSecondsRealtime(hitStopDuration);

        //Debug.Log("再開");
    }
}
