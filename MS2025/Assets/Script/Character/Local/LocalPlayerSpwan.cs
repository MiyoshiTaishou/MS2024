using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalPlayerSpwan : MonoBehaviour
{
    public  void OnPlayerJoined(PlayerInput playerInput)
    {
        // �������ꂽ�v���C���[�I�u�W�F�N�g���擾
        GameObject playerObject = playerInput.gameObject;

        // �����ŁA�v���C���[�I�u�W�F�N�g�ɑ΂��đ�����s��
        playerObject.transform.position = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
