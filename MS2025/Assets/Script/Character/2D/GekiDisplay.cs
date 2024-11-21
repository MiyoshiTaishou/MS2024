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

    [Networked] Vector3 parentpos { get; set; }

    public void SetPos(Vector3 pos)
    {
        parentpos = pos;
    }

    public override void Spawned()
    {
        transform.position = parentpos; 
    }

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

    public override void Render()
    {
        transform.position = parentpos;

    }
}
