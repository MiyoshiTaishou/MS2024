using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class SceneSelector : MonoBehaviour
{
    [SerializeField] private Dropdown sceneDropdown; // UIのDropdownをインスペクターで設定
    private List<string> sceneNames = new List<string>();

    [SerializeField] InputField field;

    void Start()
    {
        // ビルド設定に含まれているシーンをすべて取得
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            sceneNames.Add(sceneName);
        }

        // Dropdownにシーン名を追加
        sceneDropdown.ClearOptions();
        sceneDropdown.AddOptions(sceneNames);

        // Dropdownの選択が変更された時のコールバックを登録
        sceneDropdown.onValueChanged.AddListener(delegate { OnSceneSelected(sceneDropdown.value); });
    }

    // Dropdownでシーンが選択された時に呼ばれる
    public void OnSceneSelected(int index)
    {
        string roomName = field.text;

        // ルーム名をPlayerPrefsに保存
        PlayerPrefs.SetString("RoomName", roomName);

        string selectedSceneName = sceneNames[index];
        Debug.Log("Selected Scene: " + selectedSceneName);

        // 選択されたシーンに切り替える
        SceneManager.LoadScene(selectedSceneName);
    }
}
