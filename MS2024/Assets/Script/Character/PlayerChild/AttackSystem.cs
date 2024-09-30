using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    [SerializeField, Tooltip("親オブジェクト")] GameObject player;
    PlayerAttack playerattack;

    // Start is called before the first frame update
    void Start()
    {
        if(!player)
        {
            Debug.LogError("playerないよ");
        }
        playerattack = player.GetComponent<PlayerAttack>();
        if(!playerattack) 
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
        if (other.transform.CompareTag("Enemy"))
        {
            playerattack.AddHit();
        }
    }
}
