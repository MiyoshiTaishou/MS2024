using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSFreeze : MonoBehaviour
{
    // Start is called before the first frame update
    public int targetFPS = 60;

    void Start()
    {
        // �^�[�Q�b�gFPS��ݒ�
        Application.targetFrameRate = targetFPS;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
