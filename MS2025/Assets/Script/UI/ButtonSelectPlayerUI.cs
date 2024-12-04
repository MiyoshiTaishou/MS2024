using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSelectPlayerUI : MonoBehaviour
{
    [SerializeField, Tooltip("�\������摜")]
    private GameObject[] players;
    [SerializeField, Tooltip("�I�����O�ꂽ�Ƃ���\���ɂ��邩�����߂܂�")]
    private bool disActiveFlag = false;

    private int index = 0;

    public ButtonSelect select;

    private void Start()
    {
        players[index].SetActive(true);
    }

    private void Update()
    {
        if(select.selectedIndex == 0)
        {
            players[0].SetActive(true);
            players[1].SetActive(false);
            players[2].SetActive(false);
        }

        if (select.selectedIndex == 1)
        {
            players[0].SetActive(false);
            players[1].SetActive(true);
            players[2].SetActive(false);
        }

        if (select.selectedIndex == 2)
        {
            players[0].SetActive(false);
            players[1].SetActive(false);
            players[2].SetActive(true);
        }
    }

    void OnDisable() {
        if(!disActiveFlag) return;
            players[0].SetActive(false);
            players[1].SetActive(false);
            players[2].SetActive(false);
    }
}
