using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour{
    public CanvasGroup canvasGroup;
    public float FadeTime = 0.0f;

    protected float duration = 1.0f; // フェード時間
    protected enum Phase{
        Waiting,
        Beginning,
        Now,
        End
    }
    protected Phase phase = Phase.Waiting;

    void FixedUpdate(){
        FixedUpdateLogic();
    }

    protected virtual void FixedUpdateLogic(){
        switch(phase){
            case Phase.Waiting:
                if(FadeTime > 0.0f){
                    phase = Phase.Beginning;
                    FadeTime = FadeTime * 50 + duration * 50;
                }
            break;
            case Phase.Beginning:
                StartCoroutine(FadeOut());
                phase = Phase.Now;
            break;
            case Phase.Now:
                FadeTime--;
                if(FadeTime <= 0.0f){
                    phase = Phase.End;
                }
            break;
            case Phase.End:
                StartCoroutine(FadeIn());
                phase = Phase.Waiting;
            break;
        }
    }

    protected IEnumerator FadeIn(){
        float startTime = Time.time; // 開始時間
        while (Time.time - startTime < duration){
            float t = (Time.time - startTime) / duration;
            canvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, t);
            yield return null;
        }
        canvasGroup.alpha = 0.0f; // 确保完全透明
    }

    protected IEnumerator FadeOut(){
        float startTime = Time.time; // 開始時間
        while (Time.time - startTime < duration){
            float t = (Time.time - startTime) / duration;
            canvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, t);
            yield return null;
        }
        canvasGroup.alpha = 1.0f; // 确保完全不透明
    }
}
