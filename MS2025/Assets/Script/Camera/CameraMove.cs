using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
   public Transform player;  // プレイヤーのTransformを指定するための変数
    [SerializeField] private Vector3 offset;    // カメラとプレイヤーの位置差を指定するための変数

    void Update()
    {
        // プレイヤーの位置にオフセットを加えた位置にカメラを移動させる
        transform.position = player.position + offset;
    }
}
