using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : MonoBehaviour
{
    //パリィ範囲
    [SerializeField] private GameObject ParryArea;

    //ヒットストップ時間
    [SerializeField] private float HitStop = 0.05f;

    Camera Maincamera;
    CinemaCharCamera cinemachar;

    [SerializeField] 　bool Parryflg = false;

    HitStop hitStop;

    private bool PressKey = false; 

    // Start is called before the first frame update
    void Start()
    {
        hitStop = GetComponent<HitStop>();
        Maincamera = Camera.main;
        cinemachar = Maincamera.GetComponent<CinemaCharCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        //キーボードお試し
        if(Input.GetKey(KeyCode.K))
        {
            ParryArea.SetActive(true);
        }
        else
        {
            //ParryArea.SetActive(false);
        }

        if(Parryflg)
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                ParrySystem();
            }
        }

    }

    /// <summary>
    /// コントローラー入力
    /// </summary>
    /// <param name="context"></param>
    public void ParryPress(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ParryArea.SetActive(true);
            Parryflg = true;
        }

        if (context.canceled)
        {
            ParryArea.SetActive(false);
           // Parryflg= false;
        }
    }



    /// <summary>
    /// パリィ成功時の処理
    /// </summary>
    public void ParrySystem()
    {
        Debug.Log("ズーム");
        hitStop.ApplyHitStop(HitStop);
        cinemachar.CameraZoom(Vector2.zero,5,2);
        
    }
}
