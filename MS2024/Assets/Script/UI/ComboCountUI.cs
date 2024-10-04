using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ComboCountUI : MonoBehaviour
{
    Text displayText;
    LocalCombo combo = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    //void UpdateText()
    //{
    //    displayText.text = combo.GetCombo().ToString();
    //}

    // Update is called once per frame
    void Update()
    {
        GameObject[] obj = FindObjectsOfType<GameObject>();
        foreach (GameObject objs in obj)
        {
            if (objs.CompareTag("Player"))
            {
                combo = objs.transform.Find("PlayerAttackArea").GetComponent<LocalCombo>();
            }
        }
        if (combo == null)
        {
            Debug.LogError("����ڂȂ���");
        }
        displayText = GetComponent<Text>();
        if (displayText == null)
        {
            Debug.LogError("�Ă����ƂȂ���");
        }
        displayText.text=combo.GetCombo().ToString();
    }
}
