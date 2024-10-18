using UnityEngine;
using UnityEngine.UI;

public class ComboUINet : MonoBehaviour
{
    Text text;
    GameObject netobj;
    ShareNumbers combo;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        netobj = GameObject.Find("Networkbox");
        if(netobj != null )
        combo = netobj.GetComponent<ShareNumbers>();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    text.text = combo.nCombo.ToString();
    //}
}
