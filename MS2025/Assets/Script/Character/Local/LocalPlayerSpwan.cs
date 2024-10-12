using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalPlayerSpwan : MonoBehaviour
{
    public  void OnPlayerJoined(PlayerInput playerInput)
    {
        // 生成されたプレイヤーオブジェクトを取得
        GameObject playerObject = playerInput.gameObject;

        // ここで、プレイヤーオブジェクトに対して操作を行う
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
