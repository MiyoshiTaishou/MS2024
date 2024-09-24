using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : MonoBehaviour
{
    //最大HPと現在のHP。
    public float maxHp=10;
    float Hp;
    //Slider
    public Slider slider;

    void Start()
    {
        //Sliderを最大にする。
        slider.value = 10;
        //HPを最大HPと同じ値に。
        Hp = maxHp;
    }

   void Update()
    {
        if (Input.GetKeyDown("down"))
        {
            //HPから1を引く
            Hp = Hp - 1;

            //HPをSliderに反映。
            slider.value = Hp;

            Debug.Log(slider.value);

        }
    }
}