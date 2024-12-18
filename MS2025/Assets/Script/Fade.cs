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
                phase = Phase.Now;
            break;
            case Phase.Now:
                FadeTime--;
                if(FadeTime <= 0.0f){
                    phase = Phase.End;
                }
                UpdateFadeOut();
            break;
            case Phase.End:
                phase = Phase.Waiting;
                UpdateFadeIn();
            break;
        }
    }

    private void UpdateFadeIn(){
        float elapsed = duration - FadeTime / 50;
        if (elapsed < duration) {
            float t = elapsed / duration;
            canvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, t);
        } else {
            canvasGroup.alpha = 0.0f; // 確保完全透明
        }
    }

    private void UpdateFadeOut(){
        float elapsed = duration - FadeTime / 50;
        if (elapsed < duration) {
            float t = elapsed / duration;
            canvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, t);
        } else {
            canvasGroup.alpha = 1.0f; // 確保完全不透明
        }
    }
}
