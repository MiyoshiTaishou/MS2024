using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    [SerializeField, Tooltip("�e�I�u�W�F�N�g")] GameObject player;
    PlayerAttack playerattack;

    // Start is called before the first frame update
    void Start()
    {
        if(!player)
        {
            Debug.LogError("player�Ȃ���");
        }
        playerattack = player.GetComponent<PlayerAttack>();
        if(!playerattack) 
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
        if (other.transform.CompareTag("Enemy"))
        {
            playerattack.AddHit();
        }
    }
}
