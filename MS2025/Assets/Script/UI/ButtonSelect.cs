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

    public int selectedIndex = 0;  // 現在選択中のボタンのインデックス
    private float inputDelay = 0.2f; // 入力間隔を設けて、連続選択を防ぐ
    private float lastInputTime;     // 最後に入力が行われた時間
    // ボタンを押した瞬間をとるためのフラグ
    private bool aButtonTriggered = false;
    private bool bButtonTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        if (buttons != null || buttons.Length >= 0)
        {
            buttons[selectedIndex].Select(); // 最初のボタンを選択状態にする
            //buttons[selectedIndex].GetComponent<Image>().color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f); // 最初のボタンを選択状態にする
            //buttons[selectedIndex].GetComponent<Animator>().SetBool("Loop",true);
            buttonObj[selectedIndex].GetComponent<Image>().color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
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
            //buttonObj[selectedIndex].GetComponentInChildren<Animator>().SetBool("Loop",false);
            buttonObj[selectedIndex].GetComponent<Image>().color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            selectedIndex = (selectedIndex + 1) % buttons.Length;
            buttons[selectedIndex].Select();
            // buttons[selectedIndex].GetComponent<Animator>().SetBool("Loop", true);
            //buttonObj[selectedIndex].GetComponentInChildren<Animator>().SetBool("Loop",true);
            buttonObj[selectedIndex].GetComponent<Image>().color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            lastInputTime = Time.time;
        }
        else if (horizontal < 0/* || vertical > 0*/) // 左または上に移動
        {
            // buttons[selectedIndex].GetComponent<Animator>().SetBool("Loop", false);
            //buttonObj[selectedIndex].GetComponentInChildren<Animator>().SetBool("Loop",false);
            buttonObj[selectedIndex].GetComponent<Image>().color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            selectedIndex = (selectedIndex - 1 + buttons.Length) % buttons.Length;
            buttons[selectedIndex].Select();
            // buttons[selectedIndex].GetComponent<Animator>().SetBool("Loop", true);
            //buttonObj[selectedIndex].GetComponentInChildren<Animator>().SetBool("Loop",true);
            buttonObj[selectedIndex].GetComponent<Image>().color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            lastInputTime = Time.time;
        }

       Debug.Log(buttons[selectedIndex]);
    }

    // 選択中のボタンを押す処理
    private void HandleButtonPress() {        
        if (Input.GetButtonDown("Submit")/* && !aButtonTriggered*/) {// "Submit" は通常 "A" ボタンやエンターキーに対応
            if(buttons == null || buttons.Length == 0) return;
            Debug.Log(buttons[selectedIndex]);
            buttons[selectedIndex].onClick.Invoke(); // 選択中のボタンを押す
            aButtonTriggered = true;
            //Debug.Log("決定");
        }
        //if (Input.GetButtonUp("Submit")) {
        //    aButtonTriggered = false;
        //    Debug.Log("戻れ！");

        //}
    }

    public void CanselButtonPress() {
        if (Input.GetButtonDown("Cancel")/* && !bButtonTriggered*/) { // "Cancel" は通常 "B" ボタンやescキーに対応
            if(!CancelButton) return;
            selectedIndex = 0;
            CancelButton.onClick.Invoke();
            bButtonTriggered = true;
        }
        //if (Input.GetButtonUp("Cancel")) {
        //    bButtonTriggered = false;
        //}
    }
}
