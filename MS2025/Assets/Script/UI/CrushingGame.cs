using UnityEngine;
using System.Collections;

public class CrushingGame : MonoBehaviour{
    [SerializeField]private GameObject gekihaAnimator;
    private AudioSource SESource;
    [SerializeField] AudioClip SEClip;
    private float time;
    private bool isSe1 = false;
    private bool isSe2 = false;
    private int seNum = 0;
    private bool isAnimation;

    void Start() {
        SESource = GetComponent<AudioSource>();
    }

    void Update() {
        if (SESource == null || !isAnimation) return;

        // 音再生
        // if (Input.GetKeyDown(KeyCode.Space)) { // スペースキーを押すと音声再生
        //     SESource.PlayOneShot(SEClip);
        // }
        if (time > 0.1f && seNum == 1 && !isSe1) {
            Debug.LogWarning("1回目SE");
            StartSund();
            seNum = 2;
            isSe1 = true;
            time = 0.0f;
        }
        else if (time > 0.1f && seNum == 2 && !isSe2) {
            Debug.LogWarning("2回目SE");
            StartSund();
            seNum = 3;
            isSe2 = true;
        }
        time += Time.deltaTime;
    }

    public void StartAnimation() {
        if (isAnimation) return;
        gekihaAnimator.SetActive(true);
        gekihaAnimator.GetComponent<Animator>().SetTrigger("EndGame");
        // FirstSund(0.05f);
        isAnimation = true;
        seNum = 1;
    }

    public bool IsAnimation() {
        // if (gekihaAnimator == null || gekihaAnimator.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).   normalizedTime < 1.0f) return;
        if (gekihaAnimator != null && gekihaAnimator.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).   normalizedTime < 1.0f) return true;
        return false;
    }

    private void StartSund () {
        Debug.LogWarning("SE再生");
        SESource.PlayOneShot(SEClip);
    }
    private IEnumerator FirstSund (float time) {
        yield return new WaitForSeconds(time);
        SESource.Play();
        SESource.PlayOneShot(SEClip);
        SecondSund(0.1f);
    }

    private IEnumerator SecondSund (float time) {
        yield return new WaitForSeconds(time);
        SESource.Play();
        // SESource.PlayOneShot(SEClip);
    }
}