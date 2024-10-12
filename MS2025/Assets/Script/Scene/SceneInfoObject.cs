using UnityEngine;

/// <summary>
/// �V�[���J�ڂɕK�v�ȏ��N���X
/// </summary>
[CreateAssetMenu(fileName = "NewScene", menuName = "SceneScript/SceneInfo")]
public class SceneInfoObject : ScriptableObject
{
    [Header("�V�[����")]
    public string SceneName;

    [Header("�V�[���̊T�v")]
    public string DescriptionText;  
   
    [Header("�J�X�^���t�F�[�h�C���A�j���[�V����")]
    public AnimationCurve FadeInCurve;

    [Header("�J�X�^���t�F�[�h�A�E�g�A�j���[�V����")]
    public AnimationCurve FadeOutCurve;
}
