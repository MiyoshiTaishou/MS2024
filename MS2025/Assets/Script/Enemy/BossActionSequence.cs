using UnityEngine;

[CreateAssetMenu(fileName = "BossActionSequence", menuName = "Boss/Action Sequence")]
public class BossActionSequence : ScriptableObject
{
    public BossActionData[] actions; // ボスのアクションのリスト
}
