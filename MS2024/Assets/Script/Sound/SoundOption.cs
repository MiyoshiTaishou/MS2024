using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOption : MonoBehaviour
{
    AudioSource audioSource;

    AudioManager audioManager;
    [SerializeField, ReadOnly] private Slider Master;
    [SerializeField,ReadOnly] private Slider BGM;
    [SerializeField,ReadOnly] private Slider SE;

    // Start is called before the first frame update
    void Start()
    {
        //SE読み込み
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioSource = GetComponent<AudioSource>();

        //子オブジェクトからパリィエリアを取得
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "Master")
                Master = transform.GetChild(i).GetComponent<Slider>();

            if (transform.GetChild(i).gameObject.name == "BGM")
                BGM = transform.GetChild(i).GetComponent<Slider>();

            if (transform.GetChild(i).gameObject.name == "SE")
                SE = transform.GetChild(i).GetComponent<Slider>();


        }

    }

    // Update is called once per frame
    void Update()
    {
        audioManager.masterVolume = Master.value;
        audioManager.bgmMasterVolume = BGM.value;
        audioManager.seMasterVolume = SE.value;

    }
}
