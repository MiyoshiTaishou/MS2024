using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class StartSkyBoxChange : MonoBehaviour
{
    [SerializeField, Header("スカイボックスのマテリアル")] private Material material;
    [SerializeField, Header("ライト")] private GameObject light;
    [SerializeField, Header("ライト")] private GameObject light2;
    [SerializeField, Header("ポストプロセス")] private GameObject volume;
    [SerializeField, Header("ポストプロセス")] private GameObject volume2;
    private Material defaltMaterial;
    
    private void Start() {
        defaltMaterial = UnityEngine.RenderSettings.skybox;
    }

    public void StratChange(string name) {
        UnityEngine.RenderSettings.skybox = material;
        light.SetActive(false);
        light2.SetActive(true);
        volume.SetActive(false);
        volume2 .SetActive(true);

        //StartCoroutine(LoadScene(name));
    }

    public void CancelChange() {
        UnityEngine.RenderSettings.skybox = defaltMaterial;
        light.SetActive(true);
        light2.SetActive(true);
        volume.SetActive(false);
        volume2 .SetActive(false);
    }

    private IEnumerator LoadScene(string name)
    {
        // ２秒待機
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(name);
    }
}
