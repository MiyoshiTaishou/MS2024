using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSliderSelect : MonoBehaviour
{
    [SerializeField] List<GameObject> m_SelectSlider;
    [SerializeField] GameObject numcorsorobj;

    [SerializeField] float SliderInterval = 0.01f; // ���̈ړ��Ԋu�i�b�j

    [SerializeField, ReadOnly] int curornum = 0;

    [SerializeField] float CuorsorInterval = 0.5f; // ���̈ړ��Ԋu�i�b�j
    private float timer = 0f; // �^�C�}�[

    SoundActive m_ActiveSound;



    // Start is called before the first frame update
    void Start()
    {
        m_ActiveSound = transform.parent.GetComponent<SoundActive>();
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




        if (vertical > 0)
        {
            // ��莞�Ԃ��o�߂�����J�[�\�����ړ�
            if (timer >= CuorsorInterval)
            {
                timer = 0f; // �^�C�}�[�����Z�b�g
                curornum--;
            }
        }


        if (vertical < 0)
        {
            // ��莞�Ԃ��o�߂�����J�[�\�����ړ�
            if (timer >= CuorsorInterval)
            {
                timer = 0f; // �^�C�}�[�����Z�b�g
                curornum++;
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

        //�I������Ă���X���C�_�[�ړ�
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal > 0)
        {
            m_SelectSlider[curornum].GetComponent<Slider>().value += SliderInterval;
        }
        if (horizontal < 0)
        {
            m_SelectSlider[curornum].GetComponent<Slider>().value -= SliderInterval;

        }

        // B�{�^���iXbox�R���g���[���[�̏ꍇ�j
        if (Input.GetButtonDown("Cancel"))
        {
            m_ActiveSound.ShowObject();
        }
    }
}
