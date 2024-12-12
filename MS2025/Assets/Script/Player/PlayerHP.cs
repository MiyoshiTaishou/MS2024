using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : NetworkBehaviour
{
    GameObject box;
    [Networked]public bool isDamage { get; set; }
    [SerializeField,Header("�d���t���[��")]int damageFrame;
    [SerializeField, Header("�m�b�N�o�b�N����")] float knockbackDistance;
    [SerializeField, Header("�����m�b�N�o�b�N����")] float knockbackSlowDistance;
    int frame4_3;
    int frame4_1;
    int Count;
    GameObject boss;
    [Networked] public int inbisibleFrame { get; set; }
    SpriteRenderer sprite;
    
    public override void Spawned()
    {
        inbisibleFrame = 0;
        box = GameObject.Find("Networkbox");
        boss = GameObject.Find("Boss2D");
        if(!boss)
        {
            Debug.LogError("�{�X�Ȃ���");
        }
        sprite=GetComponent<SpriteRenderer>();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_DamageAnim()
    {
        if (inbisibleFrame == 0)
        {
            Count = damageFrame;
            inbisibleFrame = damageFrame * 3;
            isDamage = true;
            GetComponent<PlayerAnimChange>().RPC_InitAction("APlayerHurt");
            GetComponent<PlayerFreeze>().Freeze(damageFrame);
            GetComponent<PlayerDamageReceived>().DamageReceived();
            frame4_3 = (damageFrame / 4) * 3;
            frame4_1 = damageFrame / 4;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if(inbisibleFrame!=0)
        {
            inbisibleFrame--;
            if (inbisibleFrame%4==0) 
            {
                Color color = sprite.color;
                color.a = 0.0f;
                sprite.color = color;
            }
            else if (inbisibleFrame % 4 == 0)
            {
                Color color = sprite.color;
                color.a = 0.5f;
                sprite.color = color;
            }
            else 
            {
                Color color = sprite.color;
                color.a = 1.0f;
                sprite.color = color;
            }
        }
        if(isDamage)
        {
            Vector3 bosspos=boss.transform.position;
            Vector3 pos= transform.position;
            frame4_1 = damageFrame / 4;
            frame4_3 = frame4_1* 3;
            bool isRight = pos.x < bosspos.x ? true : false;
            float knockback=0;
            if ((frame4_3+frame4_1>Count)&&(frame4_1<Count))
            {
                knockback = knockbackDistance / frame4_3;
            }
            else
            {
                knockback = knockbackSlowDistance/frame4_1;
            }
            pos.x += isRight ? -knockback : knockback;
            transform.position = pos;
            Count--;
        }
        if(!GetComponent<PlayerFreeze>().GetIsFreeze()) 
        {
            Count = 0;
            isDamage = false;
        }
    }

    /// <summary>
    /// �Q�X�g���ɑޏo���߂𑗐M
    /// </summary>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_ExitGameForGuests()
    {
        if (!Object.HasStateAuthority)
        {
            // �Q�X�g�����[����ޏo���ăV�[����ύX����
            Runner.Shutdown();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
        }
        else
        {
            // �z�X�g�͏����҂��Ă��玩�g�̏��������s
            StartCoroutine(HandleHostShutdown());
        }
    }

    /// <summary>
    /// �z�X�g���I�����鏈��
    /// </summary>
    private IEnumerator HandleHostShutdown()
    {
        // �Q�X�g���ޏo����̂�҂i1�b���x�̒x��������j
        yield return new WaitForSeconds(1.0f);

        // �z�X�g���ޏo���ăV�[����ύX����
        Runner.Shutdown();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }
}
