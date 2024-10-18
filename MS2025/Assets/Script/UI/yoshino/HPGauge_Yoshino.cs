using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGauge_Yoshino : MonoBehaviour
{
    

    [SerializeField,Header("HP�Q�[�W")] Slider slider;
    //�ő�HP�ƌ��݂�HP�B
    [SerializeField,Header("�ő�̗�")] int nMaxHp = 150;

    [SerializeField, Header("�R���{��t����")] float fDamageHitTime = 0.5f;

    [SerializeField, Header("�_���[�W��������")] float fDamageDeleteTime = 0.5f;

    [SerializeField, Header("�ő�̗�")] bool IsPlayer = true;

    //���݂�HP
    int currentHp;

    //�_���[�W��H�炢�I���������
    float starttime = 0;

    //�_���[�W���󂯂Ă��邩�ǂ���
    bool damege = false;

    //�󂯂��_���[�W���������Ԍv���J�n�p
    bool deletedamege = false;

    void Start()
    {
        //Slider�𖞃^���ɂ���B
        slider.value = 1;
        //���݂�HP���ő�HP�Ɠ����ɁB
        currentHp = nMaxHp;
        
    }

    private void Update()
    {
        //�Ƃ肠�����X�y�[�X����������_���[�W�����炤�悤�ɂ���
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //�_���[�W��1�`100�̒��Ń����_���Ɍ��߂�B
            int damage = Random.Range(1, 10);
            Damage(damage);

        }

        if(damege)
        {
            //�ő�HP�ɂ����錻�݂�HP��Slider�ɔ��f�B
            //int���m�̊���Z�͏����_�ȉ���0�ɂȂ�̂ŁA
            //(float)������float�̕ϐ��Ƃ��ĐU���킹��B
            slider.value = (float)currentHp / (float)nMaxHp;
            if (Time.time - starttime >= fDamageHitTime && !deletedamege)
            {

                deletedamege = true;
                starttime = Time.time;
            }


            //�������Ԃ��o�߂��Ă��邩
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                //�Q�[�W������
                GaugeDelete();
            }
        }


    }

    public void Damage(int _damage)
    {
        //��_���[�W�����������ɂ���
        slider.GetComponentInChildren<Image>().color = Color.red;
        //���݂�HP����_���[�W������
        currentHp = currentHp - _damage;
        //���Ԍv��
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
            // Fill�������擾���܂�
            Transform fillArea = slider.fillRect;

            // Fill������RectTransform���擾���܂�
            RectTransform fillRectTransform = fillArea.GetComponent<RectTransform>();

            // �A���J�[��max�l���擾���܂�
            Vector2 anchorMax = fillRectTransform.anchorMin;

            Color currentColor = Color.black;
            currentColor.a = 0f;
            slider.GetComponentInChildren<Image>().color = currentColor;

            anchorMax.y = slider.GetComponentInChildren<Image>().rectTransform.anchorMin.y;

            slider.GetComponentInChildren<Image>().rectTransform.anchorMin = anchorMax;
        }
        else
        {
            // Fill�������擾���܂�
            Transform fillArea = slider.fillRect;

            // Fill������RectTransform���擾���܂�
            RectTransform fillRectTransform = fillArea.GetComponent<RectTransform>();

            // �A���J�[��max�l���擾���܂�
            Vector2 anchorMax = fillRectTransform.anchorMax;

            Color currentColor = Color.black;
            currentColor.a = 0f;
            slider.GetComponentInChildren<Image>().color = currentColor;

            anchorMax.y = slider.GetComponentInChildren<Image>().rectTransform.anchorMax.y;

            slider.GetComponentInChildren<Image>().rectTransform.anchorMax = anchorMax;

        }
    }
}
