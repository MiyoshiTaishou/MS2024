using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //�v���C���[�擾
    GameObject[] players;

    AudioSource bgmAudioSource;
    AudioSource PlayerSeAudioSource;
    AudioSource EnemySeAudioSource;
    AudioSource titleSeAudioSource;

    [SerializeField, Tooltip("�v���C���[�ɂ��Ă���̈ȊO��SE��炷���߂�AudioSource�����ׂē����")] List<AudioSource> SeSoundDataList;
    //[SerializeField] List<EnemySESoundData> EnemySeSoundDatas;
    //[SerializeField] List<UtilitySESoundData> UtilitySeSoundDatas;

    public float masterVolume = 1;
    public float bgmMasterVolume = 1;
    public float seMasterVolume = 1;

    bool cameraon = false;
    bool playeron = false;
    bool bosson = false;
    bool otheron = false;

    private void Start()
    {
        masterVolume = SoundDataManager.masterVolume;
        bgmMasterVolume = SoundDataManager.bgmMasterVolume;
        seMasterVolume = SoundDataManager.seMasterVolume;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SoundDataManager.isMute = !SoundDataManager.isMute;
        }

        if (SoundDataManager.isMute)
        {
            SoundDataManager.muteVolume = 0;
            Debug.Log("�T�E���h�~���[�g");
        }
        else
        {
            SoundDataManager.muteVolume = 1;
            Debug.Log("�T�E���h�~���[�g����");

        }

        masterVolume = SoundDataManager.masterVolume;
        bgmMasterVolume = SoundDataManager.bgmMasterVolume;
        seMasterVolume = SoundDataManager.seMasterVolume;

        //�V�[�����ƂɌʂō���Ă���AudioSource�̊Ǘ�
        if (SeSoundDataList.Count > 0)
        {
            for (int i = 0; i < SeSoundDataList.Count; i++)
            {
                SeSoundDataList[i].volume = masterVolume * seMasterVolume * SoundDataManager.muteVolume;
            }
        }

        if (!bgmAudioSource)
        {
            //  Debug.Log("�Ă΂ꂽ");
            bgmAudioSource = Camera.main.GetComponent<AudioSource>();
            bgmAudioSource.volume = masterVolume * bgmMasterVolume * SoundDataManager.muteVolume;
            cameraon = true;
        }

        if (!PlayerSeAudioSource)
        {
            // �^�O�𗘗p���ē���̃I�u�W�F�N�g���擾
             players = GameObject.FindGameObjectsWithTag("Player"); // �^�O "Player" ���g�p
            if (players.Length > 0)
            {
                Debug.Log("�T�E���h�v���C���[�̌�"+ players.Length);
                foreach (GameObject player in players)
                {
                    AudioSource playerSeAudioSource = player.GetComponent<AudioSource>();
                    if (playerSeAudioSource != null)
                    {
                        playerSeAudioSource.volume = masterVolume * seMasterVolume * SoundDataManager.muteVolume;
                    }
                }
                playeron = true;
            }
            else
            {
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
                bosson = false;
            }
        }

        if (!titleSeAudioSource)
        {
            if (GameObject.Find("SE"))
            {
                titleSeAudioSource = GameObject.Find("SE").GetComponent<AudioSource>();
                titleSeAudioSource.volume = masterVolume * seMasterVolume * SoundDataManager.muteVolume;
                otheron = true;
            }
            else
            {
                //Debug.LogError("�{�X�����Ȃ���");
                otheron = false;
            }
        }

        if (cameraon)
        {
            bgmAudioSource.volume = masterVolume * bgmMasterVolume * SoundDataManager.muteVolume;

        }

        if (playeron)
        {
            foreach (GameObject player in players)
            {
                AudioSource playerSeAudioSource = player.GetComponent<AudioSource>();
                if (playerSeAudioSource != null)
                {
                    playerSeAudioSource.volume = masterVolume * seMasterVolume * SoundDataManager.muteVolume;
                }
            }
        }

        if (bosson)
        {
            EnemySeAudioSource.volume = masterVolume * seMasterVolume * SoundDataManager.muteVolume;

        }

        if (otheron)
        {
            titleSeAudioSource.volume = masterVolume * seMasterVolume * SoundDataManager.muteVolume;

        }
    }

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
        Hoge, // ���ꂪ���x���ɂȂ�
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

        Hoge, // ���ꂪ���x���ɂȂ�
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

        Hoge, // ���ꂪ���x���ɂȂ�
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

        Hoge, // ���ꂪ���x���ɂȂ�
    }

    public SE se;
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume = 1;
}