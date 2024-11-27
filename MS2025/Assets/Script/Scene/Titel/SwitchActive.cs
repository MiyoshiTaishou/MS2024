using UnityEngine;

public class SwitchActive : MonoBehaviour
{
    [Header("アクティブ設定")]
	[Tooltip("アクティブにするオブジェクトを決めます")]
	[SerializeField]
    private GameObject[] activeObject;
	[Tooltip("アクティブを解除オブジェクトを決めます")]
	[SerializeField]
    private GameObject[] disActiveObject;
    private bool delayFlag = false;
    private float delayTime = 0.0f;

	private void Start(){
	}
	private void Update() {
        if (delayFlag){
            if (delayTime <= 0.0f) {
                DisActive();
                delayFlag = false;
            }
            delayTime -= Time.deltaTime;
        }
	}

	private void FixedUpdate() {
	}

    public void IsActive() {
        foreach (GameObject obj in activeObject) {
            obj.gameObject.SetActive(true);
        }
        foreach (GameObject obj in disActiveObject) {
            obj.gameObject.SetActive(false);
        }
    }
    public void DisActive(float time = 0.0f) {
        if (time > 0.0f) {
            delayFlag = true;
            delayTime = time;
            return;
        }
        foreach (GameObject obj in activeObject) {
            obj.gameObject.SetActive(false);
        }
        foreach (GameObject obj in disActiveObject) {
            obj.gameObject.SetActive(true);
        }
    }
}
