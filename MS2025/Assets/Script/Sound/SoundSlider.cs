using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    public enum SoundType
    {
        MASTER,
        BGM,
        SE
    }

    [SerializeField] SoundType soundType = SoundType.MASTER;

    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (soundType)
        {
            case SoundType.MASTER:
                SoundDataManager.masterVolume = slider.value;
                break;
            case SoundType.BGM:
                SoundDataManager.bgmMasterVolume = slider.value;

                break;
           case SoundType.SE:
                SoundDataManager.seMasterVolume = slider.value;

                break;
        }
    }
}
