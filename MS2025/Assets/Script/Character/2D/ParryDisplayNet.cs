using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ParryDisplayNet : NetworkBehaviour
{
    // �v���p�̃^�C�}�[
    private float timer = 0.0f;

    [SerializeField] PlayerParryNet player;

    [Networked] public bool Hit {  get; private set; } =false;

    public override void Spawned()
    {

        player = transform.parent.GetComponent<PlayerParryNet>();
        Init();
    }

    public void Init()
    {
        timer = 0.0f;
        gameObject.SetActive(false);
    }

    public override void FixedUpdateNetwork()
    {
        // ���Ԃ��v��
        timer += Time.deltaTime;

        // �w�肵���b���𒴂�����I�u�W�F�N�g���\���ɂ���
        if (timer >= player.GetParryActiveTime())
        {
            timer = 0.0f;
            Hit = false;
            player.SetParryflg(false);
            gameObject.SetActive(false);
           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Debug.Log("�Ђ���");
            Hit = true;
        }

    }
}
