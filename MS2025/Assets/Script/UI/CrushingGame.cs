using UnityEngine;

public class CrushingGame : MonoBehaviour{
    [SerializeField]private GameObject gekihaAnimator;

    public void StartAnimation() {
        gekihaAnimator.SetActive(true);
        gekihaAnimator.GetComponent<Animator>().SetTrigger("EndGame");
    }

    public bool IsAnimation() {
        // if (gekihaAnimator == null || gekihaAnimator.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).   normalizedTime < 1.0f) return;
        if (gekihaAnimator == null || gekihaAnimator.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).   normalizedTime < 1.0f) {Debug.LogWarning("アニメーション");return true;}
        Debug.LogWarning("終わり");
        return false;
    }
}