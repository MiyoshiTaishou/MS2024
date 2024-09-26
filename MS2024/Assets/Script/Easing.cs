// イージング関数
// https://easings.net/ja

using System;
using UnityEngine;

/*
 * 24/9/24現在
 * InCirc InOutCirc? InOutBounce
 * 関数で正常とは異なる動作をする
 */

public static class Easing
{
    public static float InSine(float t, float totaltime, float min, float max)
    {
        float range = max - min;
        return -range * Mathf.Cos(t * (Mathf.PI / 2) / totaltime) + range + min;
    }
    public static float OutSine(float t, float totaltime, float min, float max)
    {
        float range = max - min;
        return range * Mathf.Sin(t * (Mathf.PI / 2) / totaltime) + min;
    }
    public static float InOutSine(float t, float totaltime, float min, float max)
    {
        float range = max - min;
        return -range / 2 * (Mathf.Cos(t * Mathf.PI / totaltime) - 1) + min;
    }
    public static float InQuad(float t, float totaltime, float min, float max)
    {
        float range = max - min;
        t /= totaltime;
        return range * t * t + min;
    }
    public static float OutQuad(float t, float totaltime, float min, float max)
    {
        float range = max - min;
        t /= totaltime;
        return -range * t * (t - 2) + min;
    }
    public static float InOutQuad(float t, float totaltime, float min, float max)
    {
        float range = max - min;
        t /= (totaltime / 2);
        if (t < 1)
            return range / 2 * t * t + min;
        t -= 1;
        return -range / 2 * (t * (t - 2) - 1) + min;
    }
    public static float InCubic(float t, float totaltime, float min, float max)
    {
        float range = max - min;
        t /= totaltime;
        return range * t * t * t + min;
    }
    public static float OutCubic(float t, float totaltime, float min, float max)
    {
        float range = max - min;
        t = t / totaltime - 1;
        return range * (t * t * t + 1) + min;
    }
    public static float InOutCubic(float t, float totaltime, float min, float max)
    {
        float range = max - min;
        t /= (totaltime / 2);
        if (t < 1)
            return range / 2 * t * t * t + min;
        t -= 2;
        return range / 2 * (t * t * t + 2) + min;
    }
    public static float InQuart(float t, float totaltime, float min, float max)
    {
        float range = max - min;
        t /= totaltime;
        return range * t * t * t * t + min;
    }
    public static float OutQuart(float t, float totaltime, float min, float max)
    {
        float range = max - min;
        t = t / totaltime - 1;
        return -range * (t * t * t * t - 1) + min;
    }
    public static float InOutQuart(float t, float totaltime, float min, float max)
    {
        float range = max - min;
        t /= (totaltime / 2);
        if (t < 1)
            return range / 2 * t * t * t * t + min;
        t -= 2;
        return -range / 2 * (t * t * t * t - 2) + min;
    }
    public static float InQuint(float t, float totaltime, float min, float max)
    {
        float range = max - min;
        t /= totaltime;
        return range * t * t * t * t * t + min;
    }
    public static float OutQuint(float t, float totaltime, float min, float max)
    {
        float range = max - min;
        t = t / totaltime - 1;
        return range * (t * t * t * t * t + 1) + min;
    }
    public static float InOutQuint(float t, float totaltime, float min, float max)
    {
        float range = max - min;
        t /= (totaltime / 2);
        if (t < 1)
            return range / 2 * t * t * t * t * t + min;
        t -= 2;
        return range / 2 * (t * t * t * t * t + 2) + min;
    }
    public static float InExp(float t, float totaltime, float min, float max)
    {
        float range = max - min;
        return t == 0f ? min : range * Mathf.Pow(2, 10 * (t / totaltime - 1)) + min;
    }
    public static float OutExp(float t, float totaltime, float min, float max)
    {
        float range = max - min;
        return t == totaltime ? range + min : range * (-Mathf.Pow(2, -10 * t / totaltime) + 1) + min;
    }
    public static float InOutExp(float t, float totaltime, float min, float max)
    {
        if (t == 0f)
            return min;
        if (t == totaltime)
            return max + min;
        float range = max - min;
        t /= (totaltime / 2);
        if (t < 1)
            return range / 2 * Mathf.Pow(2, 10 * (t - 1)) + min;
        t -= 1;
        return range / 2 * (-Mathf.Pow(2, -10 * t) + 2) + min;
    }
    public static float InCirc(float t, float totaltime, float min, float max)
    {
        float range = max - min;
        t /= totaltime;
        return -range * (Mathf.Sqrt(1 - t * t) - 1) + min;
    }
    public static float OutCirc(float t, float totaltime, float min, float max)
    {
        float range = max - min;
        t = t / totaltime - 1;
        return range * Mathf.Sqrt(1 - t * t) + min;
    }
    public static float InOutCirc(float t, float totaltime, float min, float max)
    {
        float x = t / totaltime;
        float range = max - min;
        if (x < 0.5f)
            return (range / 2) * (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) + min;
        else
            return (range / 2) * (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) + min;
    }
    public static float InBack(float t, float totaltime, float min, float max)
    {
        const float c1 = 1.70158f;
        float x = t / totaltime;
        float range = max - min;
        return range * x * x * ((c1 + 1) * x - c1) + min;
    }
    public static float OutBack(float t, float totaltime, float min, float max)
    {
        const float c1 = 1.70158f;
        float x = t / totaltime - 1;
        float range = max - min;
        return range * (x * x * ((c1 + 1) * x + c1) + 1) + min;
    }
    public static float InOutBack(float t, float totaltime, float min, float max)
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;
        float range = max - min;
        t /= (totaltime / 2);
        if (t < 1)
            return range / 2 * (t * t * ((c2 + 1) * t - c2)) + min;
        t -= 2;
        return range / 2 * (t * t * ((c2 + 1) * t + c2) + 2) + min;
    }
    public static float InElastic(float t, float totaltime, float min, float max)
    {
        const float c4 = (2 * Mathf.PI) / 3;
        float range = max - min;
        t /= totaltime;
        if (t == 0f)
            return min;
        if (t == 1f)
            return range + min;
        return -range * Mathf.Pow(2, 10 * (t - 1)) * Mathf.Sin((t * 10 - 10.75f) * c4) + min;
    }
    public static float OutElastic(float t, float totaltime, float min, float max)
    {
        const float c4 = (2 * Mathf.PI) / 3;
        float x = t / totaltime;
        float range = max - min;
        if (x == 0f)
            return min;
        else if (x == 1f)
            return range + min;
        else
            return range * (Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * c4) + 1) + min;
    }
    public static float InOutElastic(float t, float totaltime, float min, float max)
    {
        const float c5 = (2 * Mathf.PI) / 4.5f;
        float range = max - min;
        t /= (totaltime / 2);
        if (t == 0f)
            return min;
        if (t == 2f)
            return range + min;
        if (t < 1)
            return -range / 2 * Mathf.Pow(2, 10 * (t - 1)) * Mathf.Sin((t * totaltime - 0.075f) * c5) + min;
        t -= 1;
        return range / 2 * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * totaltime - 0.075f) * c5) + range + min;
    }
    public static float OutBounce(float t, float totaltime, float min, float max)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;
        float range = max - min;
        t /= totaltime;
        if (t < 1f / d1)
        {
            return range * n1 * t * t + min;
        }
        else if (t < 2f / d1)
        {
            t -= 1.5f / d1;
            return range * (n1 * t * t + 0.75f) + min;
        }
        else if (t < 2.5f / d1)
        {
            t -= 2.25f / d1;
            return range * (n1 * t * t + 0.9375f) + min;
        }
        else
        {
            t -= 2.625f / d1;
            return range * (n1 * t * t + 0.984375f) + min;
        }
    }
    public static float InBounce(float t, float totaltime, float min, float max)
    {
        float range = max - min;
        return range - OutBounce(totaltime - t, totaltime, min, max) + min;
    }
    public static float InOutBounce(float t, float totaltime, float min, float max)
    {
        float x = t / totaltime;
        float range = max - min;
        if (x < 0.5f)
            return (1f - OutBounce(1f - 2f * x, totaltime, 1f, 0f)) * (range / 2) + min;
        return (OutBounce(2f * x - 1f, totaltime, 1f, 0f) * 0.5f + 0.5f) * range + min;
    }

    public static float Linear(float t, float totaltime, float min, float max)
    {
        return (max - min) * t / totaltime + min;
    }
}
