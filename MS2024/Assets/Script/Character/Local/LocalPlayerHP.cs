using UnityEngine;
using UnityEngine.UI;

public class LocalPlayerHP : MonoBehaviour
{
    [SerializeField, Tooltip("ç≈ëÂëÃóÕ")] int MaxHP;
    [SerializeField, ReadOnly] int currentHP;

    [SerializeField] Image[] image;

    void Damage(int _damage) 
    {
        currentHP -= _damage;
        image[currentHP].gameObject.SetActive(false);
        if(currentHP<=0)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHP = MaxHP;

       GameObject[] objects = GameObject.FindGameObjectsWithTag("HPUI");

        int num = 0;
        foreach (GameObject obj in objects)
        {
            image[num] = obj.GetComponent<Image>();
            num++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Enemy"))
        {
            Damage(1);
        }
    }
}
