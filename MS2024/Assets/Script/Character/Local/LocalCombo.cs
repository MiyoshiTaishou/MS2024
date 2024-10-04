using UnityEngine;

public class LocalCombo : MonoBehaviour
{
    [ReadOnly] static int nCombo = 0;
    void AddCombo() 
    {
        nCombo++; 
        Debug.Log("ƒRƒ“ƒ{‰ÁŽZ!" + nCombo);
    }
    public int GetCombo() { return nCombo; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            AddCombo();
        }
    }
}
