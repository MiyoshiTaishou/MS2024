using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public ButtonSelect select;

    public void EndGame()
    {
        if (select.selectedIndex == 2)
        {
            Debug.Log("�Q�[���I��!");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}