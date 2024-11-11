using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

enum TEXTTYPE
{
    Time,
    Combo,
    Renkei,
}

public class ScoreText : MonoBehaviour
{
    [SerializeField, Header("表示するジャンル")]
    private TEXTTYPE type;

    TextMeshProUGUI textMeshPro;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case TEXTTYPE.Combo:
                textMeshPro.text = "最大コンボ数" + ScoreManager.maxCombo + "コンボ";
                break;
            case TEXTTYPE.Renkei:
                textMeshPro.text = "ジャンプこうげき" + ScoreManager.maxMultiAttack + "回";
                break;
            case TEXTTYPE.Time:
                int time = (int)(ScoreManager.clearTime / 60);
                int time2 = (int)(ScoreManager.clearTime % 60);
                textMeshPro.text = "クリアタイム" + time + "分" + time2 + "秒";
                break;
        }
    }
}
