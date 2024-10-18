using UnityEngine;
using UnityEngine.UI;
using Fusion;
using TMPro;

public class ComboUINet : NetworkBehaviour
{
    TextMeshPro text;
    GameObject netobj;
    ShareNumbers combo;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        netobj = GameObject.Find("Networkbox");
        if(netobj != null )
        combo = netobj.GetComponent<ShareNumbers>();
    }

    public override void FixedUpdateNetwork()
    {
        netobj = GameObject.Find("Networkbox");
        if (netobj != null)
        {
            combo = netobj.GetComponent<ShareNumbers>();
        }
        Debug.LogError("‚±‚ñ‚Ú‚¨‚¨‚¨‚¨‚¨" + combo.nCombo);
        text.text = combo.nCombo.ToString();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    text.text = combo.nCombo.ToString();
    //}
}
