using UnityEngine;

public class SelectRoom : MonoBehaviour
{
    [Header("アクティブ設定")]
	[Tooltip("アクティブにするオブジェクトを決めます")]
	[SerializeField]
    private GameObject[] activeObject;
    
	[Tooltip("アクティブを解除オブジェクトを決めます")]
	[SerializeField]
    private GameObject[] disActiveObject;

	private void Start(){
	}
	private void Update() {
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
}
