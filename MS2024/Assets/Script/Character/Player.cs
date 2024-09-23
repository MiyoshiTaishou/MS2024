using Fusion;
using UnityEngine;

enum FlashState{
    ORIGINAL,
    DAMAGE
}

public class Player : NetworkBehaviour
{
    private NetworkCharacterController characterController;
    private Quaternion initialRotation;  // 最初の回転

    //[Header("プレイヤー設定")]
    [Tooltip("プレイヤーの体力を決めます")]
    [Networked] public float HP { get; set; }
    
    [Tooltip("ダメージを与えられたときの点滅回数を決めます")]
    [SerializeField]
    private int flashCount = 3;
    private int nowCount;   // 現在の点滅回数
    [Tooltip("ダメージを与えられたときの点滅する間隔を決めます")]
    [SerializeField]
    private float flashInterval;

    [Tooltip("ダメージを与えられたときの色を決めます")]
    [SerializeField]
    private Color damageColor;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private FlashState flashState; // スプライトのカラー
    private float nowTime;

    private void Awake()
    {
        characterController = GetComponent<NetworkCharacterController>();
        initialRotation = transform.rotation;  // 初期の回転を保存
    }
    
    private void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        nowCount = flashCount;
    }
    private void FixedUpdate(){
        // 点滅処理
        if(nowCount < flashCount){
            if (nowTime * 10 >= flashInterval && flashState == FlashState.DAMAGE){
                spriteRenderer.color = damageColor;
                flashState = FlashState.ORIGINAL;
                nowTime = 0;
            }
            else if (nowTime * 10 >= flashInterval && flashState == FlashState.ORIGINAL){
                spriteRenderer.color = originalColor;
                flashState = FlashState.DAMAGE;
                nowTime = 0;
                nowCount++;
            }
        }
        nowTime += Time.deltaTime;
    }

    public override void FixedUpdateNetwork()
    {
        //if (GetInput(out NetworkInputData data))
        //{
        //    // 入力方向のベクトルを正規化する
        //    data.direction.Normalize();
        //    // 入力方向を移動方向としてそのまま渡す
        //    characterController.Move(data.direction);

        //    if (data.buttons.IsSet(NetworkInputButtons.Jump))
        //    {
        //        characterController.Jump();
        //    }

        //    // プレイヤーの回転を固定
        //    transform.rotation = initialRotation;
        //}
    }

    public void FlashReset(){
        spriteRenderer.color = damageColor;
        flashState = FlashState.ORIGINAL;
        nowTime = 0;
        nowCount = 0;
    }
}
