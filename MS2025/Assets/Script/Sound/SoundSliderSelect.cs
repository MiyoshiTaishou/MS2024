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


    [SerializeField,Tooltip("�X���C�_�[�ړ��X�s�[�h")] float SliderInterval = 0.01f; // ���̈ړ��Ԋu�i�b�j
    float slidenum = 0; 
    [SerializeField, ReadOnly] int curornum = 0;
    [SerializeField, Tooltip("SE���Ȃ�ړ��ʃX�s�[�h")] float SoundInterval = 0.1f; // ���̈ړ��Ԋu�i�b�j


    [SerializeField, Tooltip("�J�[�\���ړ��X�s�[�h")] float CuorsorInterval = 0.5f; // ���̈ړ��Ԋu�i�b�j
    private float timer = 0f; // �^�C�}�[

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
        // Input Manager����̓��͂��擾
       
        float vertical = Input.GetAxis("Vertical");
        if (vertical == 0)
        {
            timer = CuorsorInterval;
        }
        // �^�C�}�[�X�V
        timer += Time.deltaTime;


        if(!isSlider)
        {
            if (vertical > 0)
            {
                // ��莞�Ԃ��o�߂�����J�[�\�����ړ�
                if (timer >= CuorsorInterval)
                {
                    timer = 0f; // �^�C�}�[�����Z�b�g
                    curornum--;
                    slidenum = 0; 
                }
            }


            if (vertical < 0)
            {
                // ��莞�Ԃ��o�߂�����J�[�\�����ړ�
                if (timer >= CuorsorInterval)
                {
                    timer = 0f; // �^�C�}�[�����Z�b�g
                    curornum++;
                    slidenum = 0;
                }
            }

        }


        //����
        if (curornum < 0)
        {
            curornum = m_SelectSlider.Count - 1;
        }

        //���
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

        //�I������Ă���X���C�_�[�ړ�
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


        // B�{�^���iXbox�R���g���[���[�̏ꍇ�j
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
