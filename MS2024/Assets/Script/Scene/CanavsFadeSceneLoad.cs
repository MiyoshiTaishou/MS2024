using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanavsFadeSceneLoad : BaseSceneManager
{
    public CanvasGroup fadeCanvasGroup;   

    protected override IEnumerator FadeOut(SceneInfoObject sceneInfo)
    {
        if (fadeCanvasGroup != null)
        {
            float curveLength = sceneInfo.FadeOutCurve[sceneInfo.FadeOutCurve.length - 1].time;
            float time = 0;
            while (time < curveLength)
            {
                time += Time.deltaTime;
                float alpha = sceneInfo.FadeOutCurve.Evaluate(time);
                fadeCanvasGroup.alpha = alpha;
                yield return null;
            }
            fadeCanvasGroup.alpha = sceneInfo.FadeOutCurve.Evaluate(curveLength); // �ŏI�I�ȃA���t�@�l��ݒ�
        }
        else
        {
            yield return base.FadeOut(sceneInfo); // �e�N���X�̃f�t�H���g����
        }
    }

    protected override IEnumerator FadeIn(SceneInfoObject sceneInfo)
    {
        if (fadeCanvasGroup != null)
        {
            float curveLength = sceneInfo.FadeInCurve[sceneInfo.FadeInCurve.length - 1].time;
            float time = 0;
            while (time < curveLength)
            {
                time += Time.deltaTime;
                float alpha = sceneInfo.FadeInCurve.Evaluate(time);
                fadeCanvasGroup.alpha = alpha;
                yield return null;
            }
            fadeCanvasGroup.alpha = sceneInfo.FadeInCurve.Evaluate(curveLength); // �ŏI�I�ȃA���t�@�l��ݒ�
        }
        else
        {
            yield return base.FadeIn(sceneInfo); // �e�N���X�̃f�t�H���g����
        }
    }
}
