using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;
using Image = UnityEngine.UI.Image;

public class Tipes : MonoBehaviour
{
    [SerializeField] GameObject ExpoPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //�v���C���[���S���������ɂ���Ƃ��̏���
            //�{�^������ݒu����Panel��\��������B
            ExpoPanel.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //�v���C���[���S������������o�����̏���
            //Panel���\���ɂ���B
            ExpoPanel.SetActive(false);
        }
    }
}
