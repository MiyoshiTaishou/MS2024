using Fusion;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DawnParticle : NetworkBehaviour
{
    [SerializeField,Header("������܂ł̃J�E���g 100�ő��1�b")]
    private float count;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Render()
    {
        count++;
        if (count == 600)
        {
            gameObject.SetActive(false);
            count = 0;
        }
    }

        // Update is called once per frame
        void Update()
    {
        

    }
}
