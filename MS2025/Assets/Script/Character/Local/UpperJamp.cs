using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class UpperJamp : MonoBehaviour
{
    [SerializeField,ReadOnly] PlayerParry parry;
    private Rigidbody rb;
    private float rayDistance = 1.5f;

    private Animator animator;

    private string groundTag = "Ground";

    // ジャンプ関連
    [SerializeField, Tooltip("ジャンプ距離")] float jumpForce = 10.0f;
    [SerializeField, Tooltip("落下速度")] float fallMultiplier = 2.5f; // 落下速度の強化

    LocalPlayerJump playerjump;


    /// <summary>
    /// ジャンプ可能かどうか
    /// </summary>
    [field: SerializeField, ReadOnly] public bool parryjump = true;


    // Start is called before the first frame update
    void Start()
    {
        //parry = GetComponent<PlayerParry>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerjump = GetComponent<LocalPlayerJump>();
    }

    // Update is called once per frame
    void Update()
    {
        if(parry != null )
        {
            if(parry.ParryJumpCheck() && playerjump.jumpnuw)
            {
                AnimatorStateInfo animStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                if (!animStateInfo.IsName("APlayerJumpUp"))
                {

                    animator.Play("APlayerJumpUp");
                }
                
                rb.AddForce(new Vector3(rb.velocity.x, jumpForce, rb.velocity.z), ForceMode.Impulse);


                //落下処理
                if (rb.velocity.y < 0)
                {


                    animStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                    if (animStateInfo.IsName("APlayerJumpUp"))
                    {

                        animator.Play("APlayerJumpDown");

                    }
                    rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

                }

                //ステージに着地したら
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
                {
                    if (hit.collider.CompareTag(groundTag))
                    {
                        AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                        if (landAnimStateInfo.IsName("APlayerJumpDown") && landAnimStateInfo.normalizedTime >= 1.0f)
                        {
                            animator.Play("APlayerIdle");
                            parryjump = false;
                        }
                    }
                }
                //パリィコンポーネントをnullにする
                parry = null;
            }
          
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(parryjump)
        {
            //パリィの範囲内に入った場合
            if (other.gameObject.name == "ParryArea")
            {
               // Debug.Log("パリィジャンプ");
                parry = other.gameObject.transform.parent.GetComponent<PlayerParry>();
            }
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        //地面に着地したらジャンプ可能にする
        if (collision.gameObject.tag == groundTag)
        {
            parryjump = true;
        }
    }

}
