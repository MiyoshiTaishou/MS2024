using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;

/// <summary>
/// プレイヤーステート管理クラス
/// </summary>
public class PlayerState : NetworkBehaviour
{
    private IState currentState;
    [HideInInspector] public PlayerInput input;

    // インスペクターで調整可能な移動速度と加速度
    [HideInInspector] public float moveSpeed = 5.0f;
    [HideInInspector] public float moveSpeedAcc = 1.0f;
    [HideInInspector] public float maxSpeed = 10.0f;

    // ジャンプ関連
    [HideInInspector] public float jumpForce = 5.0f;
    [HideInInspector] public float fallMultiplier = 2.5f; // 落下速度の強化

    [HideInInspector] public Vector3 initScale;

    // パリィ範囲
    [HideInInspector, Tooltip("パリィ範囲")] public float parryradius = 3;

    // パリィの効果時間
    [HideInInspector, Tooltip("パリィ効果時間")] public float ParryActivetime = 30;

    // ヒットストップ時間
    [HideInInspector, Tooltip("ヒットストップ時間")] public int HitStop = 3;

    // ノックバック
    [HideInInspector, Tooltip("ノックバック力")] public float KnockbackPower = 10;

    //攻撃の値
    //攻撃が発生するまでの時間
    [SerializeField, Tooltip("攻撃の発生フレーム")] public int AttackStartupFrame = 25;
    //攻撃の効果時間
    [SerializeField, Tooltip("攻撃の持続フレーム")] public int AttackActiveFrame = 50;
    //攻撃の硬直時間
    [SerializeField, Tooltip("攻撃の硬直フレーム")] public int AttackRecoveryFrame = 100;
    [SerializeField, ReadOnly] public bool isAttack = false;
    [SerializeField, ReadOnly] public int AttackCount = 0;

    private Animator animator;
    [HideInInspector] public float currentSpeed = 0.0f;

    public Vector2 moveInput;


    public IState GetNumState()
    {
        return currentState;
    }

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
       
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasInputAuthority) // ローカルプレイヤーの場合のみ更新
        {
            // 現在の状態のUpdate処理を実行
            currentState.Update();
            ChangeStateUpdate();
        }
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
        if (GetInput(out NetworkInputData data))
        {
            moveInput = data.direction;
            Debug.Log(data.direction);
        }

        if (currentState is PlayerIdleState)
        {           
            if (moveInput != Vector2.zero)
            {
                ChangeState(new PlayerMoveState(this));
            }

            // パリィ
            var buttonInput = input.actions["Parry"].ReadValue<float>();
            if (buttonInput != 0)
            {
                ChangeState(new PlayerParry(this));
            }
        }

        if (currentState is PlayerMoveState)
        {           
            if (moveInput == Vector2.zero)
            {
                ChangeState(new PlayerIdleState(this));
            }

            // パリィ
            var buttonInput = input.actions["Parry"].ReadValue<float>();
            if (buttonInput != 0)
            {
                ChangeState(new PlayerParry(this));
            }

        }

        if (currentState is PlayerParry)
        {            
            if (moveInput != Vector2.zero)
            {
                ChangeState(new PlayerMoveState(this));
            }
            else if (moveInput == Vector2.zero)
            {
                ChangeState(new PlayerIdleState(this));
            }

            // パリィ
            var buttonInput = input.actions["Parry"].ReadValue<float>();
            if (buttonInput != 0)
            {
                ChangeState(new PlayerParry(this));
            }
        }
    }
}
