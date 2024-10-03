using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    [SerializeField, Tooltip("�e�I�u�W�F�N�g������")] GameObject player;
    LocalPlayerAttack attack;

    // Start is called before the first frame update
    void Start()
    {
        attack = player.GetComponent<LocalPlayerAttack>();
        if(attack == null)
        {
            Debug.LogError("�A�^�b�N�Ȃ���");
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
        }

    }
}
