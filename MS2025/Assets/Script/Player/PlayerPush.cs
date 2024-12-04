using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class PlayerPush : NetworkBehaviour
{
    Rigidbody rb;
    GameObject player;
    Vector3 playerPos;
    public override void Spawned()
    {
        player = transform.parent.gameObject;
    }
    public override void FixedUpdateNetwork()
    {
        playerPos = player.transform.position;
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Vector3 newpos = playerPos;
            newpos.x += other.transform.position.x < playerPos.x ? 1.0f : -1.0f;
            playerPos=newpos;
            Debug.Log("Œˆ‚Ü‚èŽè‰Ÿ‚µo‚µ");
        }
    }
}
