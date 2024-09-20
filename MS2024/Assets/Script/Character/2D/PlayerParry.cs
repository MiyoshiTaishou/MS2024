using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : MonoBehaviour
{
    //パリィ範囲
    [SerializeField,Tooltip("パリィ可視化用")] private GameObject ParryArea;

    [SerializeField, Tooltip("パリィ範囲")] float parryradius = 3;

    //ヒットストップ時間
    [SerializeField, Tooltip("ヒットストップ時間")] private float HitStop = 0.05f;

    //ノックバック
    [SerializeField, Tooltip("ノックバック力")] float KnockbackPower = 10;


    Camera Maincamera;
    CinemaCharCamera cinemachar;

    [SerializeField,ReadOnly] bool Parryflg = false;

    HitStop hitStop;

    private bool PressKey = false;

    Knockback back;

    // Start is called before the first frame update
    void Start()
    {
        hitStop = GetComponent<HitStop>();
        Maincamera = Camera.main;
        cinemachar = Maincamera.GetComponent<CinemaCharCamera>();
        back = GetComponent<Knockback>();
        Vector3 scale = new Vector3(parryradius, parryradius, parryradius);
        ParryArea.transform.localScale = scale;
    }

    // Update is called once per frame
    void Update()
    {

        if(Parryflg)
        {

            //とりあえずキーボードで仮実装
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
        cinemachar.CameraZoom(this.transform,5,0.5f);
        back.ApplyKnockback(transform.forward, KnockbackPower);
    }
}
