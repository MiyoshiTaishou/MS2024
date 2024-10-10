using UnityEngine;

public class LocalCombo : MonoBehaviour
{
    [ReadOnly] static int nCombo = 0;
    [SerializeField, Tooltip("ƒRƒ“ƒ{Œp‘±ŠÔ(f)")] int keepcombotime;
    [SerializeField,ReadOnly]static int count;
    public int Getcombotime() { return count; }
    public int GetMaxcombotime() { return keepcombotime; }

    public void ResetCombo() { nCombo = 0; }
    public void AddCombo() 
    {
        nCombo++;
        count = keepcombotime;
        Debug.Log("ƒRƒ“ƒ{‰ÁZ!" + nCombo);
    }
    public int GetCombo() { return nCombo; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(count>0)
        {
            count--;
        }
        if(count<=0) 
        {
            ResetCombo();
        }
    }
}
