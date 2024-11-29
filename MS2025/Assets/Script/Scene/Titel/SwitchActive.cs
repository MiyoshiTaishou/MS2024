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

    public GameLauncher gameLauncher;

	private void Start(){
        // gameLauncher = GetComponent<GameLauncher>();
	}
	private void Update() {
        if (isDelayFlag && gameLauncher.IsAnimation()){
        // if (isDelayFlag){
            if (isDelayTime <= 0.0f) {
                IsActive();
                isDelayFlag = false;
            }
            isDelayTime -= Time.deltaTime;
        }
        if (disDelayFlag && gameLauncher.IsAnimation()){
        // if (disDelayFlag){
            if (disDelayTime <= 0.0f) {
                DisActive();
                disDelayFlag = false;
            }
            disDelayTime -= Time.deltaTime;
        }
	}

    public void IsActive(float time = 0.0f) {
        if (time > 0.0f) {
            isDelayFlag = true;
            isDelayTime = time;
            return;
        }
        foreach (GameObject obj in activeObject) {
            obj.gameObject.SetActive(true);
        }
        foreach (GameObject obj in disActiveObject) {
            obj.gameObject.SetActive(false);
        }
    }
    public void DisActive(float time = 0.0f) {
        if (time > 0.0f) {
            disDelayFlag = true;
            disDelayTime = time;
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
