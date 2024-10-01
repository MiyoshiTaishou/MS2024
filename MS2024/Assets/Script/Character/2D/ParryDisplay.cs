using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ParryDisplay : MonoBehaviour
{
    // �v���p�̃^�C�}�[
    private float timer = 0.0f;

    PlayerState player;

    private void Start()
    {
        player = transform.parent.GetComponent<PlayerState>();
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

        Debug.Log(timer);

        // �w�肵���b���𒴂�����I�u�W�F�N�g���\���ɂ���
        if (timer >= player.ParryActivetime / 60)
        {
            timer = 0.0f;
            gameObject.SetActive(false);
           
        }
    }
}
