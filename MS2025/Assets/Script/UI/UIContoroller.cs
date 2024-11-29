using UnityEngine;
using Fusion;

public class UIController : NetworkBehaviour
{
    [SerializeField] private GameObject hostUI; // �z�X�g�p��UI
    [SerializeField] private GameObject clientUI; // �N���C�A���g�p��UI
    private NetworkRunner runner;

    public override void Spawned()
    {
        // NetworkRunner�̃C���X�^���X���擾
        runner = NetworkRunner.FindObjectOfType<NetworkRunner>();

        // �z�X�g/�N���C�A���g�𔻒肵��UI��\��
        if (runner != null)
        {
            if (runner.IsServer)
            {
                ShowHostUI();
            }
            else if (runner.IsClient)
            {
                ShowClientUI();
            }
        }
    }
    
    private void ShowHostUI()
    {
        hostUI.SetActive(true);
        clientUI.SetActive(false);
    }

    private void ShowClientUI()
    {
        hostUI.SetActive(false);
        clientUI.SetActive(true);
    }
}
