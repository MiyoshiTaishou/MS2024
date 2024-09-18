using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    // �J�����ւ̎Q��
    private Camera mainCamera;

    void Start()
    {
        // ���C���J�������擾
        mainCamera = Camera.main;
    }

    void Update()
    {
        // �J�����̕����������悤�ɃI�u�W�F�N�g�̉�]��ݒ�
        Vector3 direction = mainCamera.transform.position - transform.position;
        direction.x = direction.z = 0.0f; // �I�u�W�F�N�g�����Y���݂̂���]����悤�ɒ���
        transform.LookAt(mainCamera.transform.position - direction); // �I�u�W�F�N�g�̌������J�����̕����ɐݒ�
    }
}
