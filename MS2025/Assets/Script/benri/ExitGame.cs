using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public ButtonSelect select;

    public void EndGame()
    {
        if (select.selectedIndex == 2)
        {
            Debug.Log("ÉQÅ[ÉÄèIóπ!");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}