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
    private bool diray = false;
    private float time = 0.0f;

	private void Start(){
	}
	private void Update() {
        if (diray){
            if (time <= 0.0f) {
                DisActive();
                diray = false;
            }
            time -= Time.deltaTime;
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
    public void DisActive(float t = 0.0f) {
        if (t > 0.0f) {
            diray = true;
            time = t;
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
