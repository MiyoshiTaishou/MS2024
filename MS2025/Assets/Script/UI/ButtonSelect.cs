using TMPro;
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

    [SerializeField, Header("選択時の色")]
    private Color selectColor;

    [SerializeField, Header("ベースカラー")]
    private Color baseColor;

    [SerializeField, Header("選択時の色")]
    private Color selectColorImage;

    [SerializeField, Header("ベースカラー")]
    private Color baseColorImage;

    private float inputThreshold = 0.5f; // 入力を受け付ける最小値

    [SerializeField] AudioSource Audio;
    [SerializeField,Tooltip("決定音")] AudioClip Clip;
    [SerializeField, Tooltip("キャンセル音")] AudioClip CancelClip;

    // Start is called before the first frame update
    void Start()
    {
        if (buttons != null || buttons.Length >= 0)
        {
            buttons[selectedIndex].Select(); // 最初のボタンを選択状態にする
                                             //buttons[selectedIndex].GetComponent<Image>().color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f); // 最初のボタンを選択状態にする
                                             //buttons[selectedIndex].GetComponent<Animator>().SetBool("Loop",true);
            buttonObj[selectedIndex].GetComponent<Image>().color = selectColorImage;
            
            // 子オブジェクトが1つ以上ある場合に処理を実行
            if (buttonObj[selectedIndex].transform.childCount > 0)
            {
                Transform child = buttonObj[selectedIndex].transform.GetChild(0); // 最初の子オブジェクトを取得
                TextMeshProUGUI textMeshPro = child.GetComponent<TextMeshProUGUI>();
                if (textMeshPro != null) // TextMeshProがアタッチされているか確認
                {
                    textMeshPro.color = selectColor; // テキストの色をベースカラーに戻す
                }
            }          
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

    void OnEnable() {
        buttonObj[selectedIndex].GetComponent<Image>().color = selectColorImage;

        //Transform child = buttonObj[selectedIndex].transform.GetChild(1);
        //TextMeshProUGUI textMeshPro;
        //if (child != null)
        //{
        //    textMeshPro = buttonObj[selectedIndex].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        //}
        //else
        //{
        //    textMeshPro = null;
        //}

        //if (textMeshPro != null)
        //{
        //    textMeshPro.color = selectColor;
        //}
    }
    void OnDisable() {
        // buttons[selectedIndex].Select();
        buttonObj[selectedIndex].GetComponent<Image>().color = selectColorImage;

        //Transform child = buttonObj[selectedIndex].transform.GetChild(1);
        //TextMeshProUGUI textMeshPro;
        //if (child != null)
        //{
        //    textMeshPro = buttonObj[selectedIndex].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        //}
        //else
        //{
        //    textMeshPro = null;
        //}

        //if (textMeshPro != null)
        //{
        //    textMeshPro.color = selectColor;
        //}
    }

    private void HandleButtonSelection()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal > inputThreshold)
        {
            ChangeSelection(1);
        }
        else if (horizontal < -inputThreshold)
        {
            ChangeSelection(-1);
        }
    }

    private void ChangeSelection(int direction)
    {
        if (Time.time - lastInputTime < inputDelay) return;

        // 現在の選択を解除
        buttonObj[selectedIndex].GetComponent<Image>().color = baseColorImage;

        // 子オブジェクトが1つ以上ある場合に処理を実行
        if (buttonObj[selectedIndex].transform.childCount > 0)
        {
            Transform child = buttonObj[selectedIndex].transform.GetChild(0); // 最初の子オブジェクトを取得
            TextMeshProUGUI textMeshPro = child.GetComponent<TextMeshProUGUI>();
            if (textMeshPro != null) // TextMeshProがアタッチされているか確認
            {
                textMeshPro.color = baseColor; // テキストの色をベースカラーに戻す
            }
        }

        // 新しい選択
        selectedIndex = (selectedIndex + direction + buttons.Length) % buttons.Length;
        buttons[selectedIndex].Select();
        buttonObj[selectedIndex].GetComponent<Image>().color = selectColorImage;

        // 子オブジェクトが1つ以上ある場合に処理を実行
        if (buttonObj[selectedIndex].transform.childCount > 0)
        {
            Transform child = buttonObj[selectedIndex].transform.GetChild(0); // 最初の子オブジェクトを取得
            TextMeshProUGUI textMeshPro = child.GetComponent<TextMeshProUGUI>();
            if (textMeshPro != null) // TextMeshProがアタッチされているか確認
            {
                textMeshPro.color = selectColor; // テキストの色を選択中の色に変更
            }
        }

        lastInputTime = Time.time;
        Audio.PlayOneShot(Clip);
    }




    // 選択中のボタンを押す処理
    private void HandleButtonPress() {        
        //if (Input.GetButtonDown("Submit")/* && !aButtonTriggered*/)
        //{
        //    Debug.Log(selectedIndex + "インデックス");
        //    Debug.Log("決定" + buttons[selectedIndex]);
        //    // "Submit" は通常 "A" ボタンやエンターキーに対応
        //    if (buttons == null || buttons.Length == 0) return;            
        //    buttons[selectedIndex].onClick.Invoke(); // 選択中のボタンを押す
        //    aButtonTriggered = true;            
        //}
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

            Debug.Log("キャンセル" + CancelClip);

            Audio.PlayOneShot(CancelClip);

            // buttonObj[selectedIndex].GetComponent<Image>().color = baseColor;

        }
        //if (Input.GetButtonUp("Cancel")) {
        //    bButtonTriggered = false;
        //}
    }

    public void ResetButtonColor() {
        foreach (GameObject obj in buttonObj) {
            obj.GetComponent<Image>().color = baseColor;
        }
    }
}
