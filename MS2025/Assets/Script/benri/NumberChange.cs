using Fusion;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NumberChange : NetworkBehaviour
{
    public Sprite[] numberSprites; // 0〜9までのスプライト
    public Image[] digitImages; // 各桁用のImageコンポーネント（3つ）

    [SerializeField] GameObject netobj;
    ShareNumbers sharenum;

    public override void Spawned()
    {
        netobj = GameObject.Find("Networkbox");
        if (netobj == null)
        {
            Debug.LogError("ネットの箱が無いよ");
        }
        sharenum = netobj.GetComponent<ShareNumbers>();

    }

    public override void FixedUpdateNetwork()
    {


        DisplayNumber(sharenum.nCombo);

    }



    public void DisplayNumber(int number)
    {
        // 数字を3桁の文字列として取得
        string numberStr = number.ToString("D3"); // 例：45 -> "045"

        // 各桁に対応するスプライトをImageに設定
        for (int i = 0; i < 3; i++)
        {
            int digit = numberStr[i] - '0'; // 文字から数字に変換
            digitImages[i].sprite = numberSprites[digit];
            Debug.Log("コンボカウント中" + digit);

        }
    }

    public override void Render()
    {

    }
}
