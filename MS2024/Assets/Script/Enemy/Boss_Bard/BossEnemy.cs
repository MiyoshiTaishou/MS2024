using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : MonoBehaviour
{
    //�ő�HP�ƌ��݂�HP�B
    public float maxHp=10;
    float Hp;
    //Slider
    public Slider slider;

    void Start()
    {
        //Slider���ő�ɂ���B
        slider.value = 10;
        //HP���ő�HP�Ɠ����l�ɁB
        Hp = maxHp;
    }

   void Update()
    {
        if (Input.GetKeyDown("down"))
        {
            //HP����1������
            Hp = Hp - 1;

            //HP��Slider�ɔ��f�B
            slider.value = Hp;

            Debug.Log(slider.value);

        }
    }
}