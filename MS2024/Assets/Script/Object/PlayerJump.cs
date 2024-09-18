using UnityEngine.InputSystem;
using UnityEngine;
using Fusion;

public class PlayerJump : MonoBehaviour
{
    public void OnJump(InputAction.CallbackContext context)
    {
        GetComponent<NetworkCharacterController>().Jump();
        Debug.Log("ジャンプしましたaa");

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(0.0f,10.0f,0.0f));
    }
}
