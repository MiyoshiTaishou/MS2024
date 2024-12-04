using UnityEngine;

public class AnimationAudioController : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // AudioSourceを取得
        audioSource = GetComponent<AudioSource>();
    }

    // アニメーションイベントから呼び出されるメソッド
    public void PlayAudio()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSourceがアタッチされていません！");
        }
    }
}
