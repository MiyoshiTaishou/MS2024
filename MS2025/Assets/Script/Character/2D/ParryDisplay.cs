using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ParryDisplay : MonoBehaviour
{
    // �v���p�̃^�C�}�[
    private float timer = 0.0f;

    PlayerParry player;

    public bool Hit {  get; private set; } =false;

    private void Start()
    {
        player = transform.parent.GetComponent<PlayerParry>();
        //Init();
    }

    public void Init()
    {
        timer = 0.0f;
        gameObject.SetActive(false);
    }

    void Update()
    {
        // ���Ԃ��v��
        timer += Time.deltaTime;

        // �w�肵���b���𒴂�����I�u�W�F�N�g���\���ɂ���
        if (timer >= player.GetParryActiveTime())
        {
            timer = 0.0f;
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
