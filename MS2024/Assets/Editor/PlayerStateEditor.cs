using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerState))]
public class PlayerStateEditor : Editor
{
    // �t�H�[���h�A�E�g�̏�Ԃ�ێ�����ϐ�   
    private bool showMovementSettings = true;
    private bool showJumpSettings = true;

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

        showJumpSettings = EditorGUILayout.Foldout(showJumpSettings, "�v���C���[�̃W�����v�֘A�̍���");
        if (showJumpSettings)
        {
            EditorGUI.indentLevel++;
            playerState.jumpForce = EditorGUILayout.FloatField("�W�����v�̗�", playerState.jumpForce);
            playerState.fallMultiplier = EditorGUILayout.FloatField("�������x", playerState.fallMultiplier);
        }

        showJumpSettings = EditorGUILayout.Foldout(showJumpSettings, "�v���C���[�̃p���B�֘A�̍���");
        if (showJumpSettings)
        {
            EditorGUI.indentLevel++;
            playerState.parryradius = EditorGUILayout.FloatField("�p���B�͈�", playerState.parryradius);
            playerState.ParryActivetime = EditorGUILayout.FloatField("�p���B���ʎ���", playerState.ParryActivetime);
            playerState.HitStop = EditorGUILayout.IntField("�q�b�g�X�g�b�v����", playerState.HitStop);
            playerState.KnockbackPower = EditorGUILayout.FloatField("�m�b�N�o�b�N��", playerState.KnockbackPower);
        }

        // �f�t�H���g�̃C���X�y�N�^�[������\��
        DrawDefaultInspector();
    }
}
