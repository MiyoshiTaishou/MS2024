using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaseSceneManager : MonoBehaviour
{
    public SceneInfoObject currentSceneInfo;

    [SerializeField] private InputField roomNameInputField; // ���[�����̓��̓t�B�[���h

    private void Start()
    {
        StartCoroutine(FadeIn(currentSceneInfo));
    }

    public void LoadScene(SceneInfoObject newSceneInfo)
    {
        string roomName = roomNameInputField.text;

        // ���[������PlayerPrefs�ɕۑ�
        PlayerPrefs.SetString("RoomName", roomName);

        StartCoroutine(HandleSceneTransition(newSceneInfo));
    }

    private IEnumerator HandleSceneTransition(SceneInfoObject newSceneInfo)
    {
        // ���݂̃V�[���̃t�F�[�h�A�E�g���J�n
        yield return StartCoroutine(FadeOut(currentSceneInfo));

        // ���̃V�[������ݒ�
        currentSceneInfo = newSceneInfo;

        // �V�[���̕ύX
        SceneManager.LoadScene(newSceneInfo.SceneName);      
    }

    // �t�F�[�h�A�E�g����
    protected virtual IEnumerator FadeOut(SceneInfoObject sceneInfo)
    {
        // �f�t�H���g�̃t�F�[�h�A�E�g����
        yield return new WaitForSeconds(1.0f);
    }

    // �t�F�[�h�C������
    protected virtual IEnumerator FadeIn(SceneInfoObject sceneInfo)
    {
        // �f�t�H���g�̃t�F�[�h�C������
        yield return new WaitForSeconds(1.0f);
    }
}
