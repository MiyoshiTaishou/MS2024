using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    [SerializeField, Header("�I���\�{�^��")] Button[] buttons;

    private int selectedIndex = 0;  // ���ݑI�𒆂̃{�^���̃C���f�b�N�X
    private float inputDelay = 0.2f; // ���͊Ԋu��݂��āA�A���I����h��
    private float lastInputTime;     // �Ō�ɓ��͂��s��ꂽ����

    // Start is called before the first frame update
    void Start()
    {
        buttons[selectedIndex].Select(); // �ŏ��̃{�^����I����Ԃɂ���
        buttons[selectedIndex].GetComponent<Animator>().SetBool("Loop",true);
    }

    // Update is called once per frame
    void Update()
    {
        HandleButtonSelection();
        HandleButtonPress();
    }

    // �{�^���̑I�����R���g���[���[�ŏ���
    private void HandleButtonSelection()
    {
        if (Time.time - lastInputTime < inputDelay) return; // ���͂̊Ԋu���Ǘ�

        float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");

        if (horizontal > 0/* || vertical < 0*/) // �E�܂��͉��Ɉړ�
        {
            buttons[selectedIndex].GetComponent<Animator>().SetBool("Loop", false);
            selectedIndex = (selectedIndex + 1) % buttons.Length;            
            buttons[selectedIndex].Select();
            buttons[selectedIndex].GetComponent<Animator>().SetBool("Loop", true);
            lastInputTime = Time.time;
        }
        else if (horizontal < 0/* || vertical > 0*/) // ���܂��͏�Ɉړ�
        {
            buttons[selectedIndex].GetComponent<Animator>().SetBool("Loop", false);
            selectedIndex = (selectedIndex - 1 + buttons.Length) % buttons.Length;            
            buttons[selectedIndex].Select();
            buttons[selectedIndex].GetComponent<Animator>().SetBool("Loop", true);
            lastInputTime = Time.time;
        }
    }

    // �I�𒆂̃{�^������������
    private void HandleButtonPress()
    {
        if (Input.GetButtonDown("Submit")) // "Submit" �͒ʏ� "A" �{�^����G���^�[�L�[�ɑΉ�
        {
            buttons[selectedIndex].onClick.Invoke(); // �I�𒆂̃{�^��������
        }
    }
}
