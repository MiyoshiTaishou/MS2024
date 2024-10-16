using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class SceneSelector : MonoBehaviour
{
    [SerializeField] private Dropdown sceneDropdown; // UI��Dropdown���C���X�y�N�^�[�Őݒ�
    private List<string> sceneNames = new List<string>();

    [SerializeField] InputField field;

    void Start()
    {
        // �r���h�ݒ�Ɋ܂܂�Ă���V�[�������ׂĎ擾
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            sceneNames.Add(sceneName);
        }

        // Dropdown�ɃV�[������ǉ�
        sceneDropdown.ClearOptions();
        sceneDropdown.AddOptions(sceneNames);

        // Dropdown�̑I�����ύX���ꂽ���̃R�[���o�b�N��o�^
        sceneDropdown.onValueChanged.AddListener(delegate { OnSceneSelected(sceneDropdown.value); });
    }

    // Dropdown�ŃV�[�����I�����ꂽ���ɌĂ΂��
    public void OnSceneSelected(int index)
    {
        string roomName = field.text;

        // ���[������PlayerPrefs�ɕۑ�
        PlayerPrefs.SetString("RoomName", roomName);

        string selectedSceneName = sceneNames[index];
        Debug.Log("Selected Scene: " + selectedSceneName);

        // �I�����ꂽ�V�[���ɐ؂�ւ���
        SceneManager.LoadScene(selectedSceneName);
    }
}
