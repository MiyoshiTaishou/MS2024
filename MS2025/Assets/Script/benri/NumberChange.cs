using Fusion;
using TMPro;
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
        //DisplayNumber(sharenum.nCombo);


    }

    public void DisplayNumber(int number)
    {
        // 数字を文字列として取得（例：20 -> "20"）
        string numberStr = number.ToString();

        // 数字を右詰めで配置
        for (int i = 0; i < digitImages.Length; i++)
        {
            int reverseIndex = numberStr.Length - 1 - (digitImages.Length - 1 - i);

            if (reverseIndex >= 0)
            {
                // 有効な数字を取得して表示
                int digit = numberStr[reverseIndex] - '0'; // 文字から数字に変換
                digitImages[i].sprite = numberSprites[digit];
                digitImages[i].enabled = true; // スプライトを有効化
            }
            else
            {
                // 数字が無い場合は非表示
                digitImages[i].enabled = false;
            }
        }
    }






}
