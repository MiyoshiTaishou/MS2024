using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class PlayerPush : NetworkBehaviour
{
    Rigidbody rb;
    GameObject player;
    Vector3 playerPos;
    GameObject otherPlayer;

    [Networked] public bool onPlayer { get; set;}

    public override void Spawned()
    {
        player = transform.root.gameObject;
    }
    public override void FixedUpdateNetwork()
    {
        playerPos = player.transform.position;
    }

    public override void Render()
    {
        if(!onPlayer)
        {
            return;
        }
        if (onPlayer)
        {
            Vector3 newpos = playerPos;
            Debug.Log("åàÇ‹ÇËéËâüÇµèoÇµ" + newpos.x);

            newpos.x += otherPlayer.transform.position.x < playerPos.x ? 1.0f : -1.0f;
            //newpos.x = 3;
            playerPos = newpos;
            player.transform.position = newpos;
            Debug.Log("åàÇ‹ÇËéËâüÇµèoÇµ" + newpos.x);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")&&other.gameObject!=player)
        {
            otherPlayer = other.gameObject;
            onPlayer= true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject != player)
        {
            otherPlayer = null;
            onPlayer= false;
        }
    }
}
