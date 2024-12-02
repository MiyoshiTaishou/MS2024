using UnityEngine;

public class AnimationAudioController : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // AudioSource���擾
        audioSource = GetComponent<AudioSource>();
    }

    // �A�j���[�V�����C�x���g����Ăяo����郁�\�b�h
    public void PlayAudio()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource���A�^�b�`����Ă��܂���I");
        }
    }
}
