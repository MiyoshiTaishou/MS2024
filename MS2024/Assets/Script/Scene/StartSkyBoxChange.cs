using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class StartSkyBoxChange : MonoBehaviour
{
    [SerializeField, Header("�X�J�C�{�b�N�X�̃}�e���A��")] private Material material;    
    [SerializeField, Header("���C�g")] private GameObject light;    
    [SerializeField, Header("���C�g")] private GameObject light2;    
    [SerializeField, Header("�|�X�g�v���Z�X")] private GameObject volume;
    [SerializeField, Header("�|�X�g�v���Z�X")] private GameObject volume2;
    
    public  void StratChange(string name)
    {
        UnityEngine.RenderSettings.skybox = material;   
        light.SetActive(false);
        light2.SetActive(true);
        volume.SetActive(false);
        volume2 .SetActive(true);

        StartCoroutine(LoadScene(name));
    }

    private IEnumerator LoadScene(string name)
    {
        // �Q�b�ҋ@
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(name);
    }
}
