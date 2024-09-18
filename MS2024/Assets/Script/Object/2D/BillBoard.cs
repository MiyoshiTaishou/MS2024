using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    // カメラへの参照
    private Camera mainCamera;

    void Start()
    {
        // メインカメラを取得
        mainCamera = Camera.main;
    }

    void Update()
    {
        // カメラの方向を向くようにオブジェクトの回転を設定
        Vector3 direction = mainCamera.transform.position - transform.position;
        direction.x = direction.z = 0.0f; // オブジェクトが常にY軸のみを回転するように調整
        transform.LookAt(mainCamera.transform.position - direction); // オブジェクトの向きをカメラの方向に設定
    }
}
