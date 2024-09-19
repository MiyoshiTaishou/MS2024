using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : MonoBehaviour
{
    //�p���B�͈�
    [SerializeField] private GameObject ParryArea;

    [SerializeField] private float HitStop = 0.05f;


    HitStop hitStop;

    private bool PressKey = false; 

    // Start is called before the first frame update
    void Start()
    {
        hitStop = GetComponent<HitStop>();
    }

    // Update is called once per frame
    void Update()
    {
        //�L�[�{�[�h������
        if(Input.GetKey(KeyCode.K))
        {
            ParryArea.SetActive(true);
        }
        else
        {
            //ParryArea.SetActive(false);
        }

        

    }

    /// <summary>
    /// �R���g���[���[����
    /// </summary>
    /// <param name="context"></param>
    public void ParryPress(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ParryArea.SetActive(true);

        }

        if (context.canceled)
        {
            ParryArea.SetActive(false);

        }
    }

    /// <summary>
    /// �p���B�������̏���
    /// </summary>
    public void ParrySystem()
    {
        hitStop.ApplyHitStop(HitStop);

    }
}
