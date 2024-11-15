using Fusion;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField, Header("�J�n�l��")]
    private int playerNum = 1;

    [SerializeField]
    private ShareNumbers share;

    [Networked]
    private float clearTime { get; set; }

    private bool isBattleActive = false;

    // �o�g���J�n����̃t���O
    private bool isReadyToStartBattle = false;

    public override void Spawned()
    {
        // �v���C���[�����m�F���A�o�g���J�n�̏���������
        CheckAndStartBattle();
    }

    public override void FixedUpdateNetwork()
    {
        if (isBattleActive)
        {
            // �o�g�����A�N�e�B�u�ȊԁA�o�ߎ��Ԃ��L�^
            clearTime += Time.deltaTime;
        }
        else
        {
            // �v���C���[�������I�Ɋm�F���A�o�g���J�n�������ł��邩���f
            CheckAndStartBattle();
        }
    }

    /// <summary>
    /// �o�g���J�n���̏���
    /// </summary>
    public void StartBattle()
    {
        clearTime = 0.0f;
        isBattleActive = true;
        Debug.Log("�o�g���J�n");
    }

    /// <summary>
    /// �o�g���I�����̏���
    /// </summary>
    public void EndBattle(int combo, int multiAttack)
    {
        isBattleActive = false;
        // �L�^�������Ԃ� ScoreManager �ɕۑ�
        ScoreManager.clearTime = clearTime;
        ScoreManager.maxCombo = combo;
        ScoreManager.maxMultiAttack = multiAttack;

        Debug.Log($"�o�g���I��: �N���A���� = {ScoreManager.clearTime}, �ő�R���{ = {ScoreManager.maxCombo}, �ő�A�� = {ScoreManager.maxMultiAttack}");
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_EndBattle(int combo, int multiAttack)
    {
        EndBattle(share.maxCombo, share.jumpAttackNum);
    }

    /// <summary>
    /// �v���C���[����������o�g�����J�n����
    /// </summary>
    private void CheckAndStartBattle()
    {
        if (isReadyToStartBattle || isBattleActive) return;

        // �v���C���[�����m�F (2 �l�������ꍇ)
        if (Runner.SessionInfo.PlayerCount >= playerNum)
        {
            isReadyToStartBattle = true;
            StartBattle();
        }
    }
}
