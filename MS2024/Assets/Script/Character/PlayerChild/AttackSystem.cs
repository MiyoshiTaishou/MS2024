using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    [SerializeField, Tooltip("親オブジェクトを入れる")] GameObject player;
    LocalPlayerAttack attack;
    LocalCombo combo;
    // Start is called before the first frame update
    void Start()
    {
        combo= player.GetComponent<LocalCombo>();
        attack = player.GetComponent<LocalPlayerAttack>();
        if(attack == null)
        {
            Debug.LogError("アタックないよ");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            attack.AddHit();
            combo.AddCombo();
        }
    }
}
