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

    //ジャンプ関連
    [HideInInspector] public float jumpForce = 5.0f;
    [HideInInspector] public float fallMultiplier = 2.5f; // 落下速度の強化

    [HideInInspector] public float currentSpeed = 0.0f;

    //向き関連
    [HideInInspector] public Vector3 initScale;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();   
        animator = GetComponent<Animator>();
        initScale = transform.localScale;

        // 初期状態を移動状態にセット (他の状態にする場合は変更)
        currentState = new PlayerIdleState(this);
        currentState.Enter();

        // ジャンプボタンが押された瞬間の処理
        input.actions["Jump"].performed += OnJumpPerformed;
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
        // Animatorのアニメーションをトリガーで切り替える
        animator.Play(animationName);
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        // 状態がIdleまたはMoveの場合にジャンプへの遷移を行う
        if (currentState is PlayerIdleState || currentState is PlayerMoveState)
        {
            ChangeState(new PlayerJumpState(this));
        }
    }

    public void ChangeStateUpdate()
    {
        if (currentState is PlayerIdleState)
        {
            Vector2 moveInput = input.actions["Move"].ReadValue<Vector2>();
            if (moveInput != Vector2.zero)
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
