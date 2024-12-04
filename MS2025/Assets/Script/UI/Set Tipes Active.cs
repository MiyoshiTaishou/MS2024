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
            //Panelを非表示にする。
            ExpoPanel.SetActive(false);
            TextNo++;
        }

        switch (TextNo) 
        {

            case 1:
                KeyText.text = "ここに操作説明2";
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
            //ボタン等を設置したPanelを表示させる。
            ExpoPanel.SetActive(true);
            Count = 0;
        }
    }
    void OnTriggerExit(Collider other)
    {
     
    }
}
