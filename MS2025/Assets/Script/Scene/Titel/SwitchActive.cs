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
    private bool isDelayFlag = false;
    private bool disDelayFlag = false;
    private float isDelayTime = 0.0f;
    private float disDelayTime = 0.0f;

	private void Start(){
	}
	private void Update() {
	}

    public void IsActive() {
        foreach (GameObject obj in activeObject) {
            obj.gameObject.SetActive(true);
        }
        foreach (GameObject obj in disActiveObject) {
            obj.gameObject.SetActive(false);
        }
    }
    public void DisActive() {
        foreach (GameObject obj in activeObject) {
            obj.gameObject.SetActive(false);
        }
        foreach (GameObject obj in disActiveObject) {
            obj.gameObject.SetActive(true);
        }
    }
}
