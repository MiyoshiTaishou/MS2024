using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSliderSelect : MonoBehaviour
{
    [SerializeField] List<GameObject> m_SelectSlider;
    [SerializeField] GameObject numcorsorobj;

    [SerializeField] float SliderInterval = 0.01f; // 一回の移動間隔（秒）

    [SerializeField, ReadOnly] int curornum = 0;

    [SerializeField] float CuorsorInterval = 0.5f; // 一回の移動間隔（秒）
    private float timer = 0f; // タイマー

    SoundActive m_ActiveSound;



    // Start is called before the first frame update
    void Start()
    {
        m_ActiveSound = transform.parent.GetComponent<SoundActive>();
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




        if (vertical > 0)
        {
            // 一定時間が経過したらカーソルを移動
            if (timer >= CuorsorInterval)
            {
                timer = 0f; // タイマーをリセット
                curornum--;
            }
        }


        if (vertical < 0)
        {
            // 一定時間が経過したらカーソルを移動
            if (timer >= CuorsorInterval)
            {
                timer = 0f; // タイマーをリセット
                curornum++;
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

        //選択されているスライダー移動
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal > 0)
        {
            m_SelectSlider[curornum].GetComponent<Slider>().value += SliderInterval;
        }
        if (horizontal < 0)
        {
            m_SelectSlider[curornum].GetComponent<Slider>().value -= SliderInterval;

        }

        // Bボタン（Xboxコントローラーの場合）
        if (Input.GetButtonDown("Cancel"))
        {
            m_ActiveSound.ShowObject();
        }
    }
}
