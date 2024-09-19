using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : MonoBehaviour
{
    [SerializeField] private GameObject ParryArea;

    [SerializeField] private float HitStop = 0.05f;

    private bool PressKey = false; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //キーボードお試し
        if(Input.GetKeyDown(KeyCode.K))
        {
            Vector3 pos = this.transform.position;
            Instantiate(ParryArea,pos,Quaternion.identity);
        }

        

    }

    public void Parry(InputAction.CallbackContext context)
    {
        PressKey = true;

        Vector3 pos = this.transform.position;
        Instantiate(ParryArea, pos, Quaternion.identity);
    }
}
