using UnityEngine.UI;
using UnityEngine;

public class ComboGauge : MonoBehaviour
{
    Slider slider;
    LocalCombo combo;
    private float currentGauge;
    public float maxGauge = 0f;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (combo == null)
        {
            GameObject[] obj = FindObjectsOfType<GameObject>();
            foreach (GameObject objs in obj)
            {
                if (objs.CompareTag("Player"))
                {
                    combo = objs.GetComponent<LocalCombo>();
                    maxGauge = combo.GetMaxcombotime();
                    break;
                }
            }
        }
        currentGauge = combo.Getcombotime();
        slider.value = currentGauge / maxGauge;
    }
}
