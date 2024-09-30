using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerState))]
public class PlayerStateEditor : Editor
{
    // フォールドアウトの状態を保持する変数   
    private bool showMovementSettings = true;
    private bool showJumpSettings = true;

    public override void OnInspectorGUI()
    {
        // キャスト対象のスクリプト
        PlayerState playerState = (PlayerState)target;
       
        // 折りたたみ可能な「Movement設定」
        showMovementSettings = EditorGUILayout.Foldout(showMovementSettings, "プレイヤーの移動関連の項目");
        if (showMovementSettings)
        {
            EditorGUI.indentLevel++;
            playerState.moveSpeed = EditorGUILayout.FloatField("移動速度", playerState.moveSpeed);
            playerState.moveSpeedAcc = EditorGUILayout.FloatField("移動加速度", playerState.moveSpeedAcc);
            playerState.maxSpeed = EditorGUILayout.FloatField("限界速度", playerState.maxSpeed);
            EditorGUI.indentLevel--;
        }

        showJumpSettings = EditorGUILayout.Foldout(showJumpSettings, "プレイヤーのジャンプ関連の項目");
        if (showJumpSettings)
        {
            EditorGUI.indentLevel++;
            playerState.jumpForce = EditorGUILayout.FloatField("ジャンプの力", playerState.jumpForce);
            playerState.fallMultiplier = EditorGUILayout.FloatField("落下速度", playerState.fallMultiplier);
        }

        showJumpSettings = EditorGUILayout.Foldout(showJumpSettings, "プレイヤーのパリィ関連の項目");
        if (showJumpSettings)
        {
            EditorGUI.indentLevel++;
            playerState.parryradius = EditorGUILayout.FloatField("パリィ範囲", playerState.parryradius);
            playerState.ParryActivetime = EditorGUILayout.FloatField("パリィ効果時間", playerState.ParryActivetime);
            playerState.HitStop = EditorGUILayout.IntField("ヒットストップ時間", playerState.HitStop);
            playerState.KnockbackPower = EditorGUILayout.FloatField("ノックバック力", playerState.KnockbackPower);
        }

        // デフォルトのインスペクター部分を表示
        DrawDefaultInspector();
    }
}
