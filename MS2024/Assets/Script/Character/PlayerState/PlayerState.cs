using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーステート管理クラス
/// </summary>
public class PlayerState : MonoBehaviour
{
    private IState currentState;
    [HideInInspector]public PlayerInput input;

    // インスペクターで調整可能な移動速度と加速度
    [HideInInspector] public float moveSpeed = 5.0f;
    [HideInInspector] public float moveSpeedAcc = 1.0f;
    [HideInInspector] public float maxSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();

        // 初期状態を移動状態にセット (他の状態にする場合は変更)
        currentState = new PlayerIdleState(this);
        currentState.Enter();        
    }

    // Update is called once per frame
    void Update()
    {
        //現在の状態のUpdate処理を実行
        currentState.Update();
        ChangeStateUpdate();
    }

    /// <summary>
    /// 状態の変更
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(IState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    // キャラクターのアニメーションを設定するメソッド
    public void SetAnimation(string animationName)
    {
        // アニメーションのセット処理
    }

    public void ChangeStateUpdate()
    {
        if(currentState is PlayerIdleState)
        {
            Vector2 moveInput = input.actions["Move"].ReadValue<Vector2>();
            if(moveInput != Vector2.zero)
            {
                ChangeState(new PlayerMoveState(this));
            }
        }

        if (currentState is PlayerMoveState)
        {
            Vector2 moveInput = input.actions["Move"].ReadValue<Vector2>();
            if (moveInput == Vector2.zero)
            {
                ChangeState(new PlayerIdleState(this));
            }
        }
    }
}
