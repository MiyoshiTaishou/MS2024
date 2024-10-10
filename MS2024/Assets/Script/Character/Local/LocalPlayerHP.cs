using UnityEngine;

public class LocalPlayerHP : MonoBehaviour
{
    [SerializeField, Tooltip("Å‘å‘Ì—Í")] int MaxHP;
    [SerializeField, ReadOnly] int currentHP;

    void Damage(int _damage) 
    {
        currentHP -= _damage;
        if(currentHP<=0)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
