using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class PlayerRaise : NetworkBehaviour
{
    [SerializeField,Tooltip("‚©‚¿‚ ‚°—Í")] float jumpPower = 500.0f;
    PlayerJumpNet jump;

    [SerializeField, Tooltip("‚©‚¿‚ ‚°Žž‚ÌSE")] AudioClip jumpSE;
    AudioSource audioSource;

    [Networked] private bool isRaise { get; set; }
    public bool GetisRaise() { return isRaise; }

    ShareNumbers sharenum;

    public override void Spawned()
    {
        isRaise = false;
        jump= GetComponent<PlayerJumpNet>();
        audioSource = GetComponent<AudioSource>();
        sharenum = GameObject.Find("Networkbox").GetComponent<ShareNumbers>();

    }

    public override void FixedUpdateNetwork()
    {
        if(jump.GetisGround())
        {
            isRaise = false;
        }
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (sharenum.CurrentHP == 0)
        {
            return;
        }
        if (jump.GetisJumping() && other.GetComponent<ParryDisplayNet>()&&!isRaise)
        {
            audioSource.PlayOneShot(jumpSE);
            GetComponent<NetworkRigidbody3D>().Rigidbody.AddForce(new Vector3(0.0f,jumpPower,0.0f),ForceMode.Impulse);
            isRaise = true;
            //Debug.LogError("‚Æ‚ñ‚Å‚é‚æ‚§‚§‚§");
        }
    }
}
