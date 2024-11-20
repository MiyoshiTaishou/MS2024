using Fusion;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField, Header("�J�n�l��")]
    private int playerNum = 1;

    [SerializeField, Header("�g�����W�V����")]
    private TransitionManager transitionManager;

    [SerializeField]
    private ShareNumbers share;

    [Networked]
    private float clearTime { get; set; }

    private bool isBattleActive = false;

    private bool isPlayed = false;

    public bool GetBattleActive() { return  isBattleActive; }

    // �o�g���J�n����̃t���O
    private bool isReadyToStartBattle = false;

    private bool isGameOver = false;

    public override void Spawned()
    {
        // �v���C���[�����m�F���A�o�g���J�n�̏���������
        CheckAndStartBattle();
    }

    public override void FixedUpdateNetwork()
    {
        if (isGameOver)
        {
            // �Q�[���I����͉������Ȃ�
            return;
        }

        if (isBattleActive)
        {
            // �o�g�����A�N�e�B�u�ȊԁA�o�ߎ��Ԃ��L�^
            clearTime += Time.deltaTime;
        }
        else
        {
            if (!isPlayed && !isGameOver)
            {
                // �Q�[���I����ł͂Ȃ��A�܂��o�g�����J�n����Ă��Ȃ��ꍇ�̂ݍ��G����
                CheckAndStartBattle();
            }
        }

        Debug.Log(isPlayed + "�o�O�m�F�I�I�I�I�I�I");
    }

    /// <summary>
    /// �o�g���J�n���̏���
    /// </summary>
    public void StartBattle()
    {
        clearTime = 0.0f;
        isBattleActive = true;
        isPlayed = true;
        Debug.Log("�o�g���J�n");
        transitionManager.TransitionStartReverse();
    }

    /// <summary>
    /// �o�g���I�����̏���
    /// </summary>
    public void EndBattle(int combo, int multiAttack)
    {
        isBattleActive = false;
        isGameOver = true; // �o�g���I����ɍ��G���~�߂�
        isReadyToStartBattle = false; // ���G�t���O�����Z�b�g
        isPlayed = false; // �Ăуo�g���J�n�ł��Ȃ��悤�ɂ���

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

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_StartBattle()
    {
        StartBattle();
    }

    private void CheckAndStartBattle()
    {
        // �o�g�����܂��͏I����̏ꍇ�A�������X�L�b�v
        if (isReadyToStartBattle || isBattleActive || isGameOver) return;

        // �v���C���[�����m�F (�w�肵���l�����������ꍇ)
        if (Runner.SessionInfo.PlayerCount >= playerNum)
        {
            isReadyToStartBattle = true;
            RPC_StartBattle();
        }
    }
}
