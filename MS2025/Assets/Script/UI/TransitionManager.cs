using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ʑJ�ڊ֌W���Ǘ�����
/// </summary>
public class TransitionManager : MonoBehaviour
{
    [SerializeField, Header("�g�����W�V�����I�u�W�F�N�g")] private GameObject[] transitions;
   
    // Start is called before the first frame update
    void Start()
    {       

        // �e�g�����W�V�����I�u�W�F�N�g�ɑ΂��ăg���K�[���Z�b�g���A�t�Đ��̐ݒ���s��
        for (int i = 0; i < transitions.Length; i++)
        {
            Animator animator = transitions[i].GetComponent<Animator>();
         
            // �g���K�[��ݒ肵�ăA�j���[�V�������J�n
            animator.SetTrigger("Reverse");        
        }
    }
}
