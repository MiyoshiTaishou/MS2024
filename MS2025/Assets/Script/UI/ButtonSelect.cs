using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    [Header("選択可能ボタン")]
    [Tooltip("Aボタンを押したときに動作するボタンを決めます")]
    [SerializeField]
    private Button[] buttons;
    [SerializeField]
    private GameObject[] buttonObj;
	[Tooltip("Bボタンを押したときに動作するボタンを決めます")]
    [SerializeField]
    private Button CancelButton;

    private int selectedIndex = 0;  // 現在選択中のボタンのインデックス
    private float inputDelay = 0.2f; // 入力間隔を設けて、連続選択を防ぐ
    private float lastInputTime;     // 最後に入力が行われた時間
    // ボタンを押した瞬間をとるためのフラグ
    private bool aButtonTriggered = false;
    private bool bButtonTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        if (buttons != null || buttons.Length >= 0) {
            buttons[selectedIndex].Select(); // 最初のボタンを選択状態にする
            // buttons[selectedIndex].GetComponent<Animator>().SetBool("Loop",true);
            buttonObj[selectedIndex].GetComponentInChildren<Animator>().SetBool("Loop",true);
        }

        if (Input.GetButtonDown("Submit")) {
            aButtonTriggered = true;
        }
        if (Input.GetButtonDown("Cancel")) {
            bButtonTriggered = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleButtonSelection();
        HandleButtonPress();
        CanselButtonPress();
    }

    // ボタンの選択をコントローラーで処理
    private void HandleButtonSelection()
    {
        if (Time.time - lastInputTime < inputDelay) return; // 入力の間隔を管理

        float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");

        if (horizontal > 0/* || vertical < 0*/) // 右または下に移動
        {
            // buttons[selectedIndex].GetComponent<Animator>().SetBool("Loop", false);
            buttonObj[selectedIndex].GetComponentInChildren<Animator>().SetBool("Loop",false);
            selectedIndex = (selectedIndex + 1) % buttons.Length;
            buttons[selectedIndex].Select();
            // buttons[selectedIndex].GetComponent<Animator>().SetBool("Loop", true);
            buttonObj[selectedIndex].GetComponentInChildren<Animator>().SetBool("Loop",true);
            lastInputTime = Time.time;
        }
        else if (horizontal < 0/* || vertical > 0*/) // 左または上に移動
        {
            // buttons[selectedIndex].GetComponent<Animator>().SetBool("Loop", false);
            buttonObj[selectedIndex].GetComponentInChildren<Animator>().SetBool("Loop",false);
            selectedIndex = (selectedIndex - 1 + buttons.Length) % buttons.Length;
            buttons[selectedIndex].Select();
            // buttons[selectedIndex].GetComponent<Animator>().SetBool("Loop", true);
            buttonObj[selectedIndex].GetComponentInChildren<Animator>().SetBool("Loop",true);
            lastInputTime = Time.time;
        }
    }

    // 選択中のボタンを押す処理
    private void HandleButtonPress() {
        if (Input.GetButtonDown("Submit") && !aButtonTriggered) {// "Submit" は通常 "A" ボタンやエンターキーに対応
            if(buttons == null || buttons.Length == 0) return;
            buttons[selectedIndex].onClick.Invoke(); // 選択中のボタンを押す
            aButtonTriggered = true;
        }
        if (Input.GetButtonUp("Submit")) {
            aButtonTriggered = false;
        }
    }

    public void CanselButtonPress() {
        if (Input.GetButtonDown("Cancel") && !bButtonTriggered) { // "Cancel" は通常 "B" ボタンやescキーに対応
            if(!CancelButton) return;
            CancelButton.onClick.Invoke();
            bButtonTriggered = true;
        }
        if (Input.GetButtonUp("Cancel")) {
            bButtonTriggered = false;
        }
    }
}
