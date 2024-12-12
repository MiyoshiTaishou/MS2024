using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCountSelect : MonoBehaviour
{
    [field:SerializeField,ReadOnly]  public bool aButtonTriggered { get; private set; } = false;
    [field: SerializeField, ReadOnly] public float time { get; set; } = 0;

    [SerializeField, Tooltip("��񌈒肵���㒼���ɓ������Ȃ��悤�ɂ���")] float movetime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            aButtonTriggered = true;
            time = 0;
        }

        time += Time.deltaTime;

        if (time >= movetime)
        {
            aButtonTriggered = false;
        }
    }
}
