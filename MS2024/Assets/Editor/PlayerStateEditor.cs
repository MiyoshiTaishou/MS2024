using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerState))]
public class PlayerStateEditor : Editor
{
    // �t�H�[���h�A�E�g�̏�Ԃ�ێ�����ϐ�   
    private bool showMovementSettings = true;

    public override void OnInspectorGUI()
    {
        // �L���X�g�Ώۂ̃X�N���v�g
        PlayerState playerState = (PlayerState)target;
       
        // �܂肽���݉\�ȁuMovement�ݒ�v
        showMovementSettings = EditorGUILayout.Foldout(showMovementSettings, "�v���C���[�̈ړ��֘A�̍���");
        if (showMovementSettings)
        {
            EditorGUI.indentLevel++;
            playerState.moveSpeed = EditorGUILayout.FloatField("�ړ����x", playerState.moveSpeed);
            playerState.moveSpeedAcc = EditorGUILayout.FloatField("�ړ������x", playerState.moveSpeedAcc);
            playerState.maxSpeed = EditorGUILayout.FloatField("���E���x", playerState.maxSpeed);
            EditorGUI.indentLevel--;
        }

        // �f�t�H���g�̃C���X�y�N�^�[������\��
        DrawDefaultInspector();
    }
}
