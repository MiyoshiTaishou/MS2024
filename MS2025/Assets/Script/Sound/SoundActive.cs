using UnityEngine;

public class SoundActive : MonoBehaviour
{



    // ボタンが押されたときに呼び出されるメソッド
    public void ShowObject()
    {

        this.gameObject.SetActive(!gameObject.activeSelf); // オブジェクトを表示
    }
}
