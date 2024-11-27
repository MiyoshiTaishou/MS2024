using UnityEngine;
using Fusion;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class PlayerMove : NetworkBehaviour
{
    private Rigidbody rb;
    Animator animator;

    [SerializeField, Header("加速度")]
    private float acceleration = 10f; // 加速度
    [SerializeField, Header("最大速度")]
    private float maxSpeed = 5f; // 最大速度

    private Vector3 currentVelocity;

    private Vector3 scale;

    [Networked] public bool isReflection { get; set; } = false;

    GameObject comboCountObject;

    [Networked] Vector3 dir { get; set; }

    [Networked]
    public bool isMove { get; set; }

    private HitStop hitstop;
    PlayerFreeze freeze;

    public override void Spawned()
    {
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Rigidbodyの設定
        rb.constraints = RigidbodyConstraints.FreezeRotation; // 回転を固定
        //rb.useGravity = false; // 重力を使わない場合
        scale = transform.localScale;

        comboCountObject = GameObject.Find("Networkbox");
        hitstop = GetComponent<HitStop>();
        freeze = GetComponent<PlayerFreeze>();
    }
  
    public override void FixedUpdateNetwork()
    {
        

        if(comboCountObject.GetComponent<ShareNumbers>().isSpecial||hitstop.IsHitStopActive)
        {
            return;
        }

        AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        //パリィ中は動かせないようにする
        if (freeze.GetIsFreeze()||GetComponent<PlayerChargeAttack>().isCharge)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            return;
        }

        // ネットワークインプットデータを受け取り計算する
        if (GetInput(out NetworkInputData data))
        {
            dir = data.Direction;
            // 入力方向のベクトルを正規化する
            data.Direction.Normalize();

            // Y軸の速度は現在のRigidbodyのY軸速度を保持する
            float currentYVelocity = rb.velocity.y;

            // 加速の計算を行う（XとZ軸のみ計算）
            Vector3 targetVelocity = new Vector3(data.Direction.x * maxSpeed, currentYVelocity, data.Direction.z * maxSpeed);
            currentVelocity = Vector3.MoveTowards(new Vector3(rb.velocity.x, 0, rb.velocity.z), targetVelocity, acceleration * Time.deltaTime);

            // Y軸速度を維持しながら、XとZ軸の移動を反映させる
            currentVelocity.y = currentYVelocity;

            Vector3 nomaldata = Vector3.Normalize(data.Direction);
            Vector3 nomalvel = Vector3.Normalize(currentVelocity);
            float speed = Vector3.Magnitude(currentVelocity);
            currentVelocity.x = nomaldata.x * speed;
            currentVelocity.z = nomaldata.z * speed;
            
            if(Mathf.Abs(currentVelocity.x) > maxSpeed)
            {
                currentVelocity.x = (currentVelocity.x>0)?maxSpeed:-maxSpeed;
            }
            if(Mathf.Abs(currentVelocity.z) > maxSpeed)
            {
                currentVelocity.z = (currentVelocity.z > 0) ? maxSpeed : -maxSpeed;
            }

            if ((data.Direction.x > 0.0f && currentVelocity.x < 0.0f)|| (data.Direction.x < 0.0f && currentVelocity.x > 0.0f))
            {
                currentVelocity.x = -currentVelocity.x;
            }
            if ((data.Direction.z > 0.0f && currentVelocity.z < 0.0f) || (data.Direction.z < 0.0f && currentVelocity.z > 0.0f))
            {
                currentVelocity.z = -currentVelocity.z;
            }

            // 物理的な移動を行う
            rb.velocity = currentVelocity;
            if (!GetComponent<PlayerAttack>().isAttack)
            {
                if (data.Direction.x > 0.0f)
                {
                    isReflection = false;
                }
                else if (data.Direction.x < 0.0f)
                {
                    isReflection = true;
                }
            }

            //速度制限処理
            if(rb.velocity.x > 10)
            {
                rb.velocity = new Vector3(10, rb.velocity.y, rb.velocity.z);
            }

            if(rb.velocity .x < -10)
            {
                rb.velocity = new Vector3(-10, rb.velocity.y, rb.velocity.z);
            }

            if(rb.velocity.z > 10)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 10);
            }

            if(rb.velocity.z < -10)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -10);
            }
        }
    }

    public override void Render()
    {
        
        AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (landAnimStateInfo.IsName("APlayerJumpUp") || landAnimStateInfo.IsName("APlayerJumpDown") ||
            landAnimStateInfo.IsName("APlayerParry")||landAnimStateInfo.IsName("APlayerCounter")||
            landAnimStateInfo.IsName("APlayerAttack")|| landAnimStateInfo.IsName("APlayerAttack2")|| landAnimStateInfo.IsName("APlayerAttack3")||
            landAnimStateInfo.IsName("APlayerCoordinatedAttack"))
        {
            return;
        }

        if(hitstop.IsHitStopActive) 
        {
            return;
        }

        if (dir.magnitude == 0 && !landAnimStateInfo.IsName("APlayerIdle"))
        {
            animator.Play("APlayerIdle");
        }
        else if(dir.magnitude!=0)
        {
            if (isReflection == false)
            {
                animator.Play("APlayerWalk");
                transform.localScale = scale;
            }
            else if (isReflection == true)
            {
                animator.Play("APlayerWalk");
        
                Vector3 temp = scale;
                temp.x = -scale.x;
                transform.localScale = temp;
            }
            else
            {
                animator.Play("APlayerWalk");
            }
        }
        

    }
}
