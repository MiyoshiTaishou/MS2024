using System.Collections;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public ButtonSelect select;
    public GameObject[] transition;

    public void EndGame()
    {
        if (select.selectedIndex == 2)
        {            
            //トランジション再生開始
            foreach (var tran in transition)
            {
                tran.GetComponent<Animator>().SetTrigger("Start");
            }
            StartCoroutine(PressButtonAfterDelay());
            Debug.Log("ゲーム終了!");
        }
    }

    private IEnumerator PressButtonAfterDelay()
    {
        yield return new WaitForSeconds(2); // 指定した秒数だけ待機                                             
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}