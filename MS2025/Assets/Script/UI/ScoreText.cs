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
    [SerializeField, Header("�\������W������")]
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
                textMeshPro.text = "�ő�R���{��" + ScoreManager.maxCombo + "�R���{";
                break;
            case TEXTTYPE.Renkei:
                textMeshPro.text = "�W�����v��������" + ScoreManager.maxMultiAttack + "��";
                break;
            case TEXTTYPE.Time:
                int time = (int)(ScoreManager.clearTime / 60);
                int time2 = (int)(ScoreManager.clearTime % 60);
                textMeshPro.text = "�N���A�^�C��" + time + "��" + time2 + "�b";
                break;
        }
    }
}
