using UnityEngine;

/// <summary>
/// シーン遷移に必要な情報クラス
/// </summary>
[CreateAssetMenu(fileName = "NewScene", menuName = "SceneScript/SceneInfo")]
public class SceneInfoObject : ScriptableObject
{
    [Header("シーン名")]
    public string SceneName;

    [Header("シーンの概要")]
    public string DescriptionText;  
   
    [Header("カスタムフェードインアニメーション")]
    public AnimationCurve FadeInCurve;

    [Header("カスタムフェードアウトアニメーション")]
    public AnimationCurve FadeOutCurve;
}
