using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
   public Transform player;  // �v���C���[��Transform���w�肷�邽�߂̕ϐ�
    [SerializeField] private Vector3 offset;    // �J�����ƃv���C���[�̈ʒu�����w�肷�邽�߂̕ϐ�

    void Update()
    {
        // �v���C���[�̈ʒu�ɃI�t�Z�b�g���������ʒu�ɃJ�������ړ�������
        transform.position = player.position + offset;
    }
}
