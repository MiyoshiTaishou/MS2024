using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GekiDisplay : NetworkBehaviour
{
    // �v���p�̃^�C�}�[
    [SerializeField,ReadOnly] private float timer = 0.0f;

    [SerializeField] private float Limit = 10.0f;

    


    public override void FixedUpdateNetwork()
    {
        Debug.Log("������geki");

        // ���Ԃ��v��
        timer += Time.deltaTime;

        // �w�肵���b���𒴂�����I�u�W�F�N�g���\���ɂ���
        if (timer >= Limit)
        {
            Destroy(gameObject);

        }
    }
}
