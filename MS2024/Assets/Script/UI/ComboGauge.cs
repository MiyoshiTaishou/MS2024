using UnityEngine.UI;
using UnityEngine;

public class ComboGauge : MonoBehaviour
{
    Slider slider;
    LocalCombo combo;
    private float currentGauge;
    public float maxGauge = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] obj = FindObjectsOfType<GameObject>();
        foreach (GameObject objs in obj)
        {
            if (objs.CompareTag("Player"))
            {
                combo = objs.transform.Find("PlayerAttackArea").GetComponent<LocalCombo>();
                maxGauge=combo.GetMaxcombotime();
            }
        }
        if (combo == null)
        {
            Debug.LogError("‚±‚ñ‚Ú‚È‚¢‚æ");
        }
        currentGauge = combo.Getcombotime();
        slider.value = currentGauge / maxGauge;
    }
}
