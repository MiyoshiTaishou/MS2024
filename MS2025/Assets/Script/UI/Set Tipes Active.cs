using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;
using Image = UnityEngine.UI.Image;

public class Tipes : MonoBehaviour
{
    [SerializeField] GameObject ExpoPanel;
    [SerializeField] Text KeyText;
    private int TextNo = 0;
    private float Count=0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      

        if (Input.GetKeyDown("joystick button 0"))
        {
            //Panel���\���ɂ���B
            ExpoPanel.SetActive(false);
            TextNo++;
        }

        switch (TextNo) 
        {

            case 1:
                KeyText.text = "�����ɑ������2";
                break;
        }
    }

    private void FixedUpdate()
    {
        Count++;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")&&Count>=90)
        {
            //�{�^������ݒu����Panel��\��������B
            ExpoPanel.SetActive(true);
            Count = 0;
        }
    }
    void OnTriggerExit(Collider other)
    {
     
    }
}
