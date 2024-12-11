using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    AudioSource bgmAudioSource;
    AudioSource PlayerSeAudioSource;
    AudioSource EnemySeAudioSource;
    AudioSource titleSeAudioSource;

    [SerializeField,Tooltip("プレイヤーについてるもの以外のSEを鳴らすためのAudioSourceをすべて入れる")] List<AudioSource> SeSoundDataList;
    //[SerializeField] List<EnemySESoundData> EnemySeSoundDatas;
    //[SerializeField] List<UtilitySESoundData> UtilitySeSoundDatas;

    public float masterVolume = 1;
    public float bgmMasterVolume = 1;
    public float seMasterVolume = 1;

    bool cameraon = false;
    bool playeron = false;
    bool bosson   = false;
    bool otheron = false;

    //[SerializeField, Tooltip("ミュートボタン")] bool isMute = false;

    //public static SoundManager Instance { get; private set; }

    //void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);

    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    private void Start()
    {
        masterVolume= SoundDataManager.masterVolume;
        bgmMasterVolume = SoundDataManager.bgmMasterVolume;
        seMasterVolume= SoundDataManager.seMasterVolume;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            SoundDataManager.isMute = !SoundDataManager.isMute;
        }

        if(SoundDataManager.isMute)
        {
            SoundDataManager.muteVolume = 0;

        }
        else
        {
            SoundDataManager.muteVolume = 1;
        }

        masterVolume = SoundDataManager.masterVolume;
        bgmMasterVolume = SoundDataManager.bgmMasterVolume;
        seMasterVolume = SoundDataManager.seMasterVolume;

        //シーンごとに個別で作っているAudioSourceの管理
        if(SeSoundDataList.Count > 0)
        {
            for(int i = 0;i < SeSoundDataList.Count;i++)
            {
                SeSoundDataList[i].volume = masterVolume * seMasterVolume * SoundDataManager.muteVolume;
            }
        }

        if (!bgmAudioSource)
        {
          //  Debug.Log("呼ばれた");
            bgmAudioSource = Camera.main.GetComponent<AudioSource>();
            bgmAudioSource.volume= masterVolume * bgmMasterVolume * SoundDataManager.muteVolume;
            cameraon = true;
        }

        if (!PlayerSeAudioSource)
        {
            if (GameObject.Find("Player(Clone)"))
            {
                PlayerSeAudioSource = GameObject.Find("Player(Clone)").GetComponent<AudioSource>();
                PlayerSeAudioSource.volume= masterVolume * seMasterVolume * SoundDataManager.muteVolume;
                playeron = true;
            }
            else
            {
                // Debug.LogError("プレイヤーがいないよ");
                playeron = false;
            }
        }

        if (!EnemySeAudioSource)
        {
            if (GameObject.Find("Boss2D"))
            {
                EnemySeAudioSource = GameObject.Find("Boss2D").GetComponent<AudioSource>();
                EnemySeAudioSource.volume = masterVolume * seMasterVolume * SoundDataManager.muteVolume;
                bosson = true;
            }
            else
            {
                bosson= false;
            }
        }

        if(!titleSeAudioSource)
        {
            if (GameObject.Find("SE"))
            {
                titleSeAudioSource = GameObject.Find("SE").GetComponent<AudioSource>();
                titleSeAudioSource.volume = masterVolume * seMasterVolume * SoundDataManager.muteVolume;
                otheron = true;
            }
            else
            {
                //Debug.LogError("ボスがいないよ");
                otheron = false;
            }
        }

        if(cameraon)
        {
            bgmAudioSource.volume = masterVolume * bgmMasterVolume * SoundDataManager.muteVolume;

        }

        if(playeron)
        {
            PlayerSeAudioSource.volume = masterVolume * seMasterVolume * SoundDataManager.muteVolume;

        }

        if(bosson)
        {
            EnemySeAudioSource.volume = masterVolume * seMasterVolume * SoundDataManager.muteVolume;

        }

        if(otheron)
        {
            titleSeAudioSource.volume = masterVolume * seMasterVolume * SoundDataManager.muteVolume;

        }
    }

    //public override void FixedUpdateNetwork()
    //{
    //    if (!bgmAudioSource)
    //    {
    //        Debug.Log("呼ばれた");
    //        bgmAudioSource = Camera.main.GetComponent<AudioSource>();
    //    }

    //    if (!PlayerSeAudioSource)
    //    {
    //        if (GameObject.Find("Player(Clone)"))
    //        {
    //            PlayerSeAudioSource = GameObject.Find("Player(Clone)").GetComponent<AudioSource>();

    //        }
    //        else
    //        {
    //            Debug.LogError("プレイヤーがいないよ");

    //        }
    //    }

    //    if (!EnemySeAudioSource)
    //    {
    //        if (GameObject.Find("Boss2D"))
    //        {
    //            EnemySeAudioSource = GameObject.Find("Boss2D").GetComponent<AudioSource>();

    //        }
    //        else
    //        {
    //            Debug.LogError("ボスがいないよ");
    //        }
    //    }

    //}

    //public void PlayBGM(BGMSoundData.BGM bgm)
    //{
    //    BGMSoundData data = bgmSoundDatas.Find(data => data.bgm == bgm);
    //    bgmAudioSource.clip = data.audioClip;
    //    bgmAudioSource.volume = data.volume * bgmMasterVolume * masterVolume;
    //    bgmAudioSource.Play();
    //}


    //public void PlayerSE(PlayerSESoundData.SE se)
    //{
    //    PlayerSESoundData data = PlayerSeSoundDatas.Find(data => data.se == se);
    //    PlayerSeAudioSource.volume = data.volume * seMasterVolume * masterVolume;
    //    PlayerSeAudioSource.PlayOneShot(data.audioClip);
    //}

    //public void EnemySE(EnemySESoundData.SE se)
    //{
    //    EnemySESoundData data = EnemySeSoundDatas.Find(data => data.se == se);
    //    EnemySeAudioSource.volume = data.volume * seMasterVolume * masterVolume;
    //    EnemySeAudioSource.PlayOneShot(data.audioClip);
    //}

    //public void UtilitySE(UtilitySESoundData.SE se)
    //{
    //    UtilitySESoundData data = UtilitySeSoundDatas.Find(data => data.se == se);
    //    PlayerSeAudioSource.volume = data.volume * seMasterVolume * masterVolume;
    //    PlayerSeAudioSource.PlayOneShot(data.audioClip);
    //}

}

[System.Serializable]
public class BGMSoundData
{
    public enum BGM
    {
        Title,
        Matu,
        Take,
        Ume,
        Result,
        Hoge, // これがラベルになる
    }

    public BGM bgm;
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume = 1;
}

[System.Serializable]
public class PlayerSESoundData
{
    public enum SE
    {
        Attack1,
        Attack2,
        Attack3,
        Damage,
        JumpUp,
        JumpDown,
        Parry,
        Charge,
        ChargeAttack,

        Hoge, // これがラベルになる
    }

    public SE se;
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume = 1;
}

[System.Serializable]
public class EnemySESoundData
{
    public enum SE
    {
        Kick,
        Attack2,
        Attack3,
        Damage,
        Jump,
        Parry,
        Charge,
        ChargeAttack,

        Hoge, // これがラベルになる
    }

    public SE se;
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume = 1;
}

[System.Serializable]
public class UtilitySESoundData
{
    public enum SE
    {
        Attack1,
        Attack2,
        Attack3,
        Damage,
        Jump,
        Parry,
        Charge,
        ChargeAttack,

        Hoge, // これがラベルになる
    }

    public SE se;
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume = 1;
}