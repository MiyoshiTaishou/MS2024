using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryDisplay : MonoBehaviour
{
    // �v���p�̃^�C�}�[
    private float timer = 0.0f;

    PlayerParry player;

    private void Start()
    {
        player = transform.parent.GetComponent<PlayerParry>();
        Init();
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
            player.SetParryflg(false);
            gameObject.SetActive(false);
           
        }
    }
}
