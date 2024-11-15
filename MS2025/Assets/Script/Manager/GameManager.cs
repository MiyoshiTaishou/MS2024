using Fusion;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField, Header("開始人数")]
    private int playerNum = 1;

    [SerializeField]
    private ShareNumbers share;

    [Networked]
    private float clearTime { get; set; }

    private bool isBattleActive = false;

    // バトル開始判定のフラグ
    private bool isReadyToStartBattle = false;

    public override void Spawned()
    {
        // プレイヤー数を確認し、バトル開始の準備をする
        CheckAndStartBattle();
    }

    public override void FixedUpdateNetwork()
    {
        if (isBattleActive)
        {
            // バトルがアクティブな間、経過時間を記録
            clearTime += Time.deltaTime;
        }
        else
        {
            // プレイヤー数を定期的に確認し、バトル開始準備ができるか判断
            CheckAndStartBattle();
        }
    }

    /// <summary>
    /// バトル開始時の処理
    /// </summary>
    public void StartBattle()
    {
        clearTime = 0.0f;
        isBattleActive = true;
        Debug.Log("バトル開始");
    }

    /// <summary>
    /// バトル終了時の処理
    /// </summary>
    public void EndBattle(int combo, int multiAttack)
    {
        isBattleActive = false;
        // 記録した時間を ScoreManager に保存
        ScoreManager.clearTime = clearTime;
        ScoreManager.maxCombo = combo;
        ScoreManager.maxMultiAttack = multiAttack;

        Debug.Log($"バトル終了: クリア時間 = {ScoreManager.clearTime}, 最大コンボ = {ScoreManager.maxCombo}, 最大連撃 = {ScoreManager.maxMultiAttack}");
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_EndBattle(int combo, int multiAttack)
    {
        EndBattle(share.maxCombo, share.jumpAttackNum);
    }

    /// <summary>
    /// プレイヤーが揃ったらバトルを開始する
    /// </summary>
    private void CheckAndStartBattle()
    {
        if (isReadyToStartBattle || isBattleActive) return;

        // プレイヤー数を確認 (2 人揃った場合)
        if (Runner.SessionInfo.PlayerCount >= playerNum)
        {
            isReadyToStartBattle = true;
            StartBattle();
        }
    }
}
