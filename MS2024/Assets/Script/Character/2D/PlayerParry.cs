using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : NetworkBehaviour
{
    //パリィ範囲
    [SerializeField,Tooltip("パリィ可視化用")] private GameObject ParryArea;

    [SerializeField, Tooltip("パリィ範囲")] float parryradius = 3;

    //パリィの効果時間
    [SerializeField, Tooltip("パリィ効果時間")] float ParryActivetime = 3;

    //ヒットストップ時間
    [SerializeField, Tooltip("ヒットストップ時間")] private float HitStop = 0.05f;

    //ノックバック
    [SerializeField, Tooltip("ノックバック力")] float KnockbackPower = 50;


    Camera Maincamera;
    CinemaCharCamera cinemachar;

    [SerializeField,ReadOnly] bool Parryflg = false;

    HitStop hitStop;

    private bool PressKey = false;

    Knockback back;

    public float GetParryActiveTime() { return ParryActivetime; }

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

        //if (context.canceled)
        //{
        //    ParryArea.SetActive(false);
        //   // Parryflg= false;
        //}
    }

    /// <summary>
    /// パリィ成功時の処理
    /// </summary>
    public void ParrySystem()
    {
        hitStop.ApplyHitStop(HitStop);
        cinemachar.CameraZoom(this.transform,5,0.5f);
        back.ApplyKnockback(transform.forward, KnockbackPower);
        ParryArea.GetComponent<ParryDisplay>().Init();
    }

    public override void FixedUpdateNetwork()
    {
        if (ParryArea.activeSelf)
        {

            //とりあえずキーボードで仮実装
            if (Input.GetKeyDown(KeyCode.L))
            {
                ParrySystem();
            }
        }

    }


}
