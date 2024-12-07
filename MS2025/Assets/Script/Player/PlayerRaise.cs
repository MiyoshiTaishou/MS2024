using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class PlayerRaise : NetworkBehaviour
{
    [SerializeField,Tooltip("かちあげ力")] float jumpPower = 500.0f;
    PlayerJumpNet jump;

    [SerializeField, Tooltip("かちあげ時のSE")] AudioClip jumpSE;
    AudioSource audioSource;

    [Networked] private bool isRaise { get; set; }
    public bool GetisRaise() { return isRaise; }
    // Start is called before the first frame update
    void Start()
    {
    }

    public override void Spawned()
    {
        isRaise = false;
        jump= GetComponent<PlayerJumpNet>();
        audioSource = GetComponent<AudioSource>();
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
        if (jump.GetisJumping() && other.GetComponent<ParryDisplayNet>()&&!isRaise)
        {
            audioSource.PlayOneShot(jumpSE);
            GetComponent<NetworkRigidbody3D>().Rigidbody.AddForce(new Vector3(0.0f,jumpPower,0.0f),ForceMode.Impulse);
            isRaise = true;
            //Debug.LogError("とんでるよぉぉぉ");
        }
    }
}
