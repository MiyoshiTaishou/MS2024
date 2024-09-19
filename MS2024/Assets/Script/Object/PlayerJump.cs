using UnityEngine.InputSystem;
using UnityEngine;
using Fusion;

public class PlayerJump : MonoBehaviour
{
    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            GetComponent<NetworkCharacterController>().Jump();
            Debug.Log("ƒWƒƒƒ“ƒv‚µ‚Ü‚µ‚½aa");


        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<Rigidbody>().AddForce(new Vector3(0.0f,10.0f,0.0f));
    }
}
