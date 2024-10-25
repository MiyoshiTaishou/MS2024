using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class PlayerRaise : NetworkBehaviour
{
    private float jumpPower = 10.0f;
    PlayerJumpNet jump;

    [Networked] private bool isRaise { get; set; }

    // Start is called before the first frame update
    void Start()
    {
    }

    public override void Spawned()
    {
        isRaise = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (jump.GetisGround() && other.GetComponent<ParryDisplayNet>()&&other.transform.parent!=this)
        {
            Vector3 vel =  GetComponent<NetworkRigidbody3D>().Rigidbody.velocity;
            vel.y = jumpPower;
            GetComponent<NetworkRigidbody3D>().Rigidbody.velocity = vel;
        }
    }
}
