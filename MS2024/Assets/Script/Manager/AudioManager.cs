using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource bgmAudioSource;
    [SerializeField] AudioSource seAudioSource;

    [SerializeField] List<BGMSoundData> bgmSoundDatas;
    [SerializeField] List<SESoundData> seSoundDatas;

    public float masterVolume = 1;
    public float bgmMasterVolume = 1;
    public float seMasterVolume = 1;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(AudioSource source,BGMSoundData.BGM bgm)
    {
        BGMSoundData data = bgmSoundDatas.Find(data => data.bgm == bgm);
        bgmAudioSource.clip = data.audioClip;
        bgmAudioSource.volume = data.volume * bgmMasterVolume * masterVolume;
        bgmAudioSource.Play();
    }


    public void PlaySE(AudioSource source,SESoundData.SE se)
    {
        SESoundData data = seSoundDatas.Find(data => data.se == se);
        seAudioSource = source;
        seAudioSource.volume = data.volume * seMasterVolume * masterVolume;
        seAudioSource.PlayOneShot(data.audioClip);
    }

    [System.Serializable]
    public class BGMSoundData
    {
        public enum BGM
        {
            Title,
            StageSelect,
            Stage,
            Hoge, // ‚±‚ê‚ªƒ‰ƒxƒ‹‚É‚È‚é
        }

        public BGM bgm;
        public AudioClip audioClip;
        [Range(0, 1)]
        public float volume = 1;
    }

    [System.Serializable]
    public class SESoundData
    {
        public enum SE
        {
            Move,
            JumpUp,
            JumpDown,
            Attack,
            Damage,
            Parry,
            ParrySuccess,
            Hoge, // ‚±‚ê‚ªƒ‰ƒxƒ‹‚É‚È‚é
        }

        public SE se;
        public AudioClip audioClip;
        [Range(0, 1)]
        public float volume = 1;
    }
}
