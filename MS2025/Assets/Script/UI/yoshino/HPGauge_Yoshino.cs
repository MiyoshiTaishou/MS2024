using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGauge_Yoshino : MonoBehaviour
{
    

    [SerializeField,Header("HPゲージ")] Slider slider;
    //最大HPと現在のHP。
    [SerializeField,Header("最大体力")] int nMaxHp = 150;

    [SerializeField, Header("コンボ受付時間")] float fDamageHitTime = 0.5f;

    [SerializeField, Header("ダメージ消す時間")] float fDamageDeleteTime = 0.5f;

    [SerializeField, Header("最大体力")] bool IsPlayer = true;

    //現在のHP
    int currentHp;

    //ダメージを食らい終わった時間
    float starttime = 0;

    //ダメージを受けているかどうか
    bool damege = false;

    //受けたダメージを消す時間計測開始用
    bool deletedamege = false;

    void Start()
    {
        //Sliderを満タンにする。
        slider.value = 1;
        //現在のHPを最大HPと同じに。
        currentHp = nMaxHp;
        
    }

    private void Update()
    {
        //とりあえずスペースを押したらダメージをくらうようにする
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //ダメージは1～100の中でランダムに決める。
            int damage = Random.Range(1, 10);
            Damage(damage);

        }

        if(damege)
        {
            //最大HPにおける現在のHPをSliderに反映。
            //int同士の割り算は小数点以下は0になるので、
            //(float)をつけてfloatの変数として振舞わせる。
            slider.value = (float)currentHp / (float)nMaxHp;
            if (Time.time - starttime >= fDamageHitTime && !deletedamege)
            {

                deletedamege = true;
                starttime = Time.time;
            }


            //消す時間を経過しているか
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                //ゲージを消す
                GaugeDelete();
            }
        }


    }

    public void Damage(int _damage)
    {
        //被ダメージ部分をあかにする
        slider.GetComponentInChildren<Image>().color = Color.red;
        //現在のHPからダメージを引く
        currentHp = currentHp - _damage;
        //時間計測
        starttime = Time.time;

        damege = true;
    }

    public void GaugeDelete()
    {
        ChangeColor();
        starttime = 0;
        deletedamege = false;
        damege = false;
    }

    private void ChangeColor()
    {
        if(!IsPlayer)
        {
            // Fill部分を取得します
            Transform fillArea = slider.fillRect;

            // Fill部分のRectTransformを取得します
            RectTransform fillRectTransform = fillArea.GetComponent<RectTransform>();

            // アンカーのmax値を取得します
            Vector2 anchorMax = fillRectTransform.anchorMin;

            Color currentColor = Color.black;
            currentColor.a = 0f;
            slider.GetComponentInChildren<Image>().color = currentColor;

            anchorMax.y = slider.GetComponentInChildren<Image>().rectTransform.anchorMin.y;

            slider.GetComponentInChildren<Image>().rectTransform.anchorMin = anchorMax;
        }
        else
        {
            // Fill部分を取得します
            Transform fillArea = slider.fillRect;

            // Fill部分のRectTransformを取得します
            RectTransform fillRectTransform = fillArea.GetComponent<RectTransform>();

            // アンカーのmax値を取得します
            Vector2 anchorMax = fillRectTransform.anchorMax;

            Color currentColor = Color.black;
            currentColor.a = 0f;
            slider.GetComponentInChildren<Image>().color = currentColor;

            anchorMax.y = slider.GetComponentInChildren<Image>().rectTransform.anchorMax.y;

            slider.GetComponentInChildren<Image>().rectTransform.anchorMax = anchorMax;

        }
    }
}
