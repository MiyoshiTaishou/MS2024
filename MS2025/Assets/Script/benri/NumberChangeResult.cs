using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NumberChangeResult : NetworkBehaviour
{
    [SerializeField, Header("表示するジャンル")]
    private TEXTTYPE type;

    public Sprite[] numberSprites; // 0〜9までのスプライト
    public Image[] digitImages;    // 各桁用のImageコンポーネント（任意の数）

    public override void Spawned()
    {
        DisplayNumberResult();
    }

    public override void FixedUpdateNetwork()
    {
        // 必要に応じて更新処理を追加
    }

    /// <summary>
    /// 数字を表示するメソッド
    /// </summary>
    public void DisplayNumberResult()
    {
        string numberStr;

        switch (type)
        {
            case TEXTTYPE.Combo:
                // 数字を任意の桁数の文字列として取得
                numberStr = ScoreManager.maxCombo.ToString();
                DisplayNumber(numberStr);
                break;

            case TEXTTYPE.Renkei:
                // 数字を任意の桁数の文字列として取得
                numberStr = ScoreManager.maxMultiAttack.ToString();
                DisplayNumber(numberStr);
                break;

            case TEXTTYPE.Time:
                // 時間を分と秒の形式に変換
                int totalSeconds = (int)ScoreManager.clearTime;
                int minutes = totalSeconds / 60;
                int seconds = totalSeconds % 60;

                // "MM:SS"の形式にフォーマット
                numberStr = $"{minutes:D2}{seconds:D2}";
                DisplayNumber(numberStr);
                break;
        }
    }

    /// <summary>
    /// 指定された文字列に基づいて数字を表示
    /// </summary>
    /// <param name="numberStr">表示する数字の文字列</param>
    private void DisplayNumber(string numberStr)
    {
        // 画像の数と桁数が一致しない場合は調整
        for (int i = 0; i < digitImages.Length; i++)
        {
            if (i < numberStr.Length)
            {
                // 各桁に対応するスプライトをImageに設定
                int digit = numberStr[i] - '0'; // 文字から数字に変換
                if (digit >= 0 && digit <= 9)
                {
                    digitImages[i].sprite = numberSprites[digit];
                    digitImages[i].enabled = true; // スプライトを表示
                }
            }
            else
            {
                // 桁が不足している場合は非表示にする
                digitImages[i].enabled = false;
            }
        }
    }
}
