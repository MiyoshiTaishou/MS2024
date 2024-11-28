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
            //�g�����W�V�����Đ��J�n
            foreach (var tran in transition)
            {
                tran.GetComponent<Animator>().SetTrigger("Start");
            }
            StartCoroutine(PressButtonAfterDelay());
            Debug.Log("�Q�[���I��!");
        }
    }

    private IEnumerator PressButtonAfterDelay()
    {
        yield return new WaitForSeconds(2); // �w�肵���b�������ҋ@                                             
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}