using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSliderSelect : MonoBehaviour
{
    [SerializeField] List<GameObject> m_SelectSlider;
    [SerializeField] GameObject numcorsorobj;

    //[SerializeField] List<GameObject> m_SelectHandle;

    [SerializeField] Sprite normalHandleObj;
    [SerializeField] Sprite handleObj;


    [SerializeField,Tooltip("スライダー移動スピード")] float SliderInterval = 0.01f; // 一回の移動間隔（秒）
    float slidenum = 0; 
    [SerializeField, ReadOnly] int curornum = 0;
    [SerializeField, Tooltip("SEがなる移動量スピード")] float SoundInterval = 0.1f; // 一回の移動間隔（秒）


    [SerializeField, Tooltip("カーソル移動スピード")] float CuorsorInterval = 0.5f; // 一回の移動間隔（秒）
    private float timer = 0f; // タイマー

    SoundActive m_ActiveSound;

    bool isSlider = false;

    [SerializeField] GameObject cancel;

    [SerializeField] AudioSource SESource;
    [SerializeField] AudioClip SEClip;

    // Start is called before the first frame update
    void Start()
    {
        m_ActiveSound = transform.parent.GetComponent<SoundActive>();
        SESource.clip = SEClip;

    }

    // Update is called once per frame
    void Update()
    {
        // Input Managerからの入力を取得
       
        float vertical = Input.GetAxis("Vertical");
        if (vertical == 0)
        {
            timer = CuorsorInterval;
        }
        // タイマー更新
        timer += Time.deltaTime;


        if(!isSlider)
        {
            if (vertical > 0)
            {
                // 一定時間が経過したらカーソルを移動
                if (timer >= CuorsorInterval)
                {
                    timer = 0f; // タイマーをリセット
                    curornum--;
                    slidenum = 0; 
                }
            }


            if (vertical < 0)
            {
                // 一定時間が経過したらカーソルを移動
                if (timer >= CuorsorInterval)
                {
                    timer = 0f; // タイマーをリセット
                    curornum++;
                    slidenum = 0;
                }
            }

        }


        //下限
        if (curornum < 0)
        {
            curornum = m_SelectSlider.Count - 1;
        }

        //上限
        if(curornum > m_SelectSlider.Count - 1)
        {
            curornum = 0;
        }

        if(numcorsorobj)
        {
            Vector3 pos = numcorsorobj.transform.position;
            pos.y = m_SelectSlider[curornum].transform.position.y;
            numcorsorobj.transform.position = pos;

        }
        if (Input.GetButtonDown("Submit"))
        {
            isSlider = !isSlider;
        }

        //選択されているスライダー移動
        float horizontal = Input.GetAxis("Horizontal");

        if (isSlider)
        {
            for (int i = 0; i < m_SelectSlider.Count; i++)
            {
                if (i == curornum)
                {
                    m_SelectSlider[i].GetComponent<Slider>().handleRect.GetComponent<Image>().sprite = handleObj;

                }
                else
                {
                    m_SelectSlider[i].GetComponent<Slider>().handleRect.GetComponent<Image>().sprite = normalHandleObj;

                }
            }
            // m_SelectHandle[curornum].GetComponent<Image>().sprite

            if (horizontal > 0)
            {
                m_SelectSlider[curornum].GetComponent<Slider>().value += SliderInterval;
                slidenum += SliderInterval;
            }
            if (horizontal < 0)
            {
                m_SelectSlider[curornum].GetComponent<Slider>().value -= SliderInterval;
                slidenum += SliderInterval;


            }

        }
        else
        {
            for (int i = 0; i < m_SelectSlider.Count; i++)
            {
                m_SelectSlider[i].GetComponent<Slider>().handleRect.GetComponent<Image>().sprite = normalHandleObj;
            }
        }

        if (slidenum >= SoundInterval)
        {
            SESource.PlayOneShot(SESource.clip);
            slidenum = 0;
        }


        if (slidenum - m_SelectSlider[curornum].GetComponent<Slider>().value  >= SoundInterval)
        {
        }


        // Bボタン（Xboxコントローラーの場合）
        if (Input.GetButtonDown("Cancel"))
        {
            if(isSlider)
            {
                isSlider = !isSlider;

            }
            else
            {
                // m_ActiveSound.ShowObject();
                cancel.GetComponent<SwitchActive>().DisActive(0);
            }

        }



    }
}
