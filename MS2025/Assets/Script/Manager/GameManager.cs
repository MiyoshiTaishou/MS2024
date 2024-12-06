using Fusion;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    //[SerializeField, Header("開始人数")]
    //private int playerNum = 1;

    [SerializeField, Header("トランジション")]
    private TransitionManager transitionManager;

    [SerializeField]
    private ShareNumbers share;

    [Networked]
    private float clearTime { get; set; }

    [Networked]
    private bool isBattleActive { get; set; }

    [Networked]
    private bool isPlayed { get; set; }

    public bool GetBattleActive() { return  isBattleActive; }

    // バトル開始判定のフラグ
    [Networked]
    private bool isReadyToStartBattle { get; set; }

    [Networked]
    private bool isGameOver { get; set; }

    public override void Spawned()
    {
        // プレイヤー数を確認し、バトル開始の準備をする
        CheckAndStartBattle();
    }

    public override void FixedUpdateNetwork()
    {
        if (isGameOver)
        {
            // ゲーム終了後は何もしない
            return;
        }

        if (isBattleActive)
        {
            // バトルがアクティブな間、経過時間を記録
            clearTime += Time.deltaTime;
        }
        else
        {
            if (!isPlayed && !isGameOver)
            {
                // ゲーム終了後ではなく、まだバトルが開始されていない場合のみ索敵する
                CheckAndStartBattle();
            }
        }

        Debug.Log(isPlayed + "バグ確認！！！！！！");
    }

    /// <summary>
    /// バトル開始時の処理
    /// </summary>
    public void StartBattle()
    {
        clearTime = 0.0f;
        isBattleActive = true;
        isPlayed = true;
        Debug.Log("バトル開始");
        //transitionManager.TransitionStartReverse();
    }

    /// <summary>
    /// バトル終了時の処理
    /// </summary>
    /// <summary>
    /// バトル終了時の処理
    /// </summary>
    public void EndBattle(int combo, int multiAttack)
    {
        isBattleActive = false;
        isGameOver = true; // バトル終了後に索敵を止める
        isReadyToStartBattle = false; // 索敵フラグをリセット

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

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_StartBattle()
    {
        StartBattle();
    }

    private void CheckAndStartBattle()
    {
        // すでにゲームを開始している場合、またはバトル中・終了後の場合は処理をスキップ
        if (isPlayed || isReadyToStartBattle || isBattleActive || isGameOver) return;

        isReadyToStartBattle = true;
        RPC_StartBattle();
        //// プレイヤー数を確認 (指定した人数が揃った場合)
        //if (Runner.SessionInfo.PlayerCount >= playerNum)
        //{
        //    isReadyToStartBattle = true;
        //    RPC_StartBattle();
        //}
    }
}
