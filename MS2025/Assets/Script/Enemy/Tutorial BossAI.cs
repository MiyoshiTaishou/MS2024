using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UIElements;

public class TutoriarBossAI : NetworkBehaviour
{
    [Header("�ʏ�s��")]
    public BossActionSequence[] actionSequence;

    [Header("50%�ȉ��̍s��")]
    public BossActionSequence[] actionSequenceHalf;

    //�m�b�N�o�b�N�����Ԃ�
    [Networked] public bool isKnockBack { get; set; }

    //�p���B�\�ȏ�Ԃ�
    [Networked] public bool isParry { get; set; }
    [Networked] public int Nokezori { get; set; }
    [SerializeField, Tooltip("�p���B�A�g�U���ł̂������")] public int NokezoriLimit;
    private int currentActionIndex = 0;
    private BossActionData currentAction;
    public BossActionData GetCurrentAction() { return currentAction; }
    private bool isActionInitialized = false;
    private Animator animator;
    private bool isOnce = false;
    private bool isHalf = false;
    private int isParticle = 1;//�_�E���p�[�e�B�N�����o������������肷�邽�߂�int�ϐ�
    private int isAttack = 0;//�U���\���G�t�F�N�g���o���^�C�~���O���v�邽�߂�int�ϐ� 
    private Vector3 scale;

    [SerializeField, Header("�m�b�N�o�b�N�̃A�j���[�V������")]
    private string animName;

    // �v���C���[�^�[�Q�b�g�p
    public List<Transform> players;
    [Networked] public int currentPlayerIndex { get; set; }
    [Networked] private int currentSequenceIndex { get; set; }
    [Networked, SerializeField] private int maxPlayerIndex { get; set; }
    [Networked, SerializeField] public bool isInterrupted { get; set; }/*������Ă�*/
    [Networked, SerializeField] public bool isDown { get; set; }
    [Networked, SerializeField] public bool isAir { get; set; }
    [Networked, SerializeField] public bool isDir { get; set; }

    [SerializeField, Header("�_�E�����̍s���f�[�^")]
    public BossActionData downAction;

    [SerializeField, Header("�̂����莞�̍s���f�[�^")]
    public BossActionData parryction;
    [Tooltip("�_�E�����G�t�F�N�g")]
    private ParticleSystem Dawnparticle;

    [SerializeField, Header("�U���̗\���Ɋւ��鍀��")]
    [Tooltip("�U���\���G�t�F�N�g")]
    private ParticleSystem AttackOmenParticle;
    [Tooltip("�U���\���G�t�F�N�g���o���܂ł̎���(0.3f�����x�����C�����܂�)")]
    private float Omentime = 0.3f;
    [Tooltip("�U���\���G�t�F�N�g��X���W")]
    private float OmenPosX = 1.5f;
    [Tooltip("�U���\���G�t�F�N�g��Y���W")]
    private float OmenPosY = 1.7f;

    private ParticleSystem newParticle;

    private GameManager gameManager;

    private ShareNumbers shareNumbers;

    // �A�j���[�V���������l�b�g���[�N����������
    [Networked]
    private NetworkString<_16> networkedAnimationName { get; set; }

    public override void Spawned()
    {
        animator = GetComponent<Animator>(); // Animator �R���|�[�l���g���擾
        currentSequenceIndex = Random.Range(0, actionSequence.Length);

        Nokezori = 0;
        // �v���C���[�I�u�W�F�N�g�����ׂĎ擾���ă��X�g�ɕۑ�
        players = new List<Transform>();
        RefreshPlayerList();

        scale = transform.localScale;

        //�Q�[���}�l�[�W���[����
        gameManager = GameObject.FindObjectOfType<GameManager>();

        if (!gameManager)
        {
            Debug.LogWarning("������܂���ł���");
        }

        //�V�F�A�i���o�[����
        shareNumbers = GameObject.FindObjectOfType<ShareNumbers>();

        if (!shareNumbers)
        {
            Debug.LogWarning("������܂���ł���");
        }

        if (players.Count < maxPlayerIndex)
        {
            Debug.Log("Waiting for more players...");
        }
        else
        {
            StartNextAction(); // �v���C���[����l�ȏ㑵���Ă�����A�N�V�������J�n
        }
    }

    public override void FixedUpdateNetwork()
    {
        // �v���C���[����l�ȏア�Ȃ��ꍇ�A�s�����J�n�����T���𑱂���
        if (players.Count < maxPlayerIndex)
        {
            SearchForPlayers(); // �T�����̓���������Ɏ���
            return;
        }

        //�Q�[���J�n���ĂȂ������瓮�����Ȃ�
        if (!gameManager.GetBattleActive())
        {
            return;
        }

        //�K�E�Z���͓����Ȃ�
        if (shareNumbers.isSpecial)
        {
            return;
        }

        if (isInterrupted)
        {
            HandleInterruption();
            return;
        }

        if (isDown && !isOnce)
        {
            Debug.Log("�_�E��");
            currentAction = downAction;
            currentActionIndex = 0;
            isActionInitialized = false;
            isOnce = true;
            isParticle = 2;
            return;
        }

        if (currentAction == null) return;

        //�����ύX����
        if (transform.position.x > players[currentPlayerIndex].position.x)
        {
            isDir = true;
        }
        else if (transform.position.x < players[currentPlayerIndex].position.x)
        {
            isDir = false;
        }

        //������Ă������Ȃ��悤�ɂ���ׂ̏���
        GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0);

        if (!isActionInitialized)
        {
            RPC_InitAction();
        }

        if (currentAction.ExecuteAction(gameObject, players[currentPlayerIndex]))
        {
            StartNextAction(); // �A�N�V����������Ɏ��̃A�N�V�����ɐi��
        }

        //50%�ȉ��ōs���ύX
        if (!isHalf && GetComponent<BossStatus>().nBossHP < GetComponent<BossStatus>().InitHP / 2)
        {
            isHalf = true;
            actionSequence = actionSequenceHalf;
            currentActionIndex = 0;
            currentSequenceIndex = 0;
            StartNextAction(); // �v���C���[����l�ȏ㑵���Ă�����A�N�V�������J�n            
        }
    }

    private void HandleInterruption()
    {
        Debug.Log("�ς�ꂽ��������������");
        currentAction = parryction;
        currentActionIndex = 0;
        currentSequenceIndex = 0;
        isActionInitialized = false;
        isInterrupted = false;
    }

    private IEnumerator WaitAndStartNextAction(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isInterrupted = false;
        StartNextAction();
    }

    // �v���C���[����l�ȏ㑵���܂ŒT���𑱂��邽�߂̃��\�b�h
    private void SearchForPlayers()
    {
        // �v���C���[���X�g���Ċm�F����
        RefreshPlayerList();

        if (players.Count >= maxPlayerIndex)
        {
            Debug.Log("Players are now available. Starting actions.");
            StartNextAction(); // �v���C���[����������A�N�V�������J�n
        }
        else
        {
            Debug.Log("Searching for players...");
        }
    }

    // �v���C���[���X�g���X�V���郁�\�b�h
    private void RefreshPlayerList()
    {
        players.Clear();
        foreach (var playerObj in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(playerObj.transform);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_InitAction()
    {
        // ���݂̃^�[�Q�b�g�v���C���[�̎Q�Ƃ��A�N�V�����ɐݒ�
        currentAction.InitializeAction(gameObject, players[currentPlayerIndex]); // �^�[�Q�b�g�v���C���[��n��

        // �A�N�V�����ɑΉ�����A�j���[�V�������z�X�g���ōĐ�
        if (Object.HasStateAuthority && animator != null && !string.IsNullOrEmpty(currentAction.actionName))
        {
            Debug.Log($"Playing animation: {currentAction.actionName}");
            networkedAnimationName = currentAction.actionName; // �l�b�g���[�N�ϐ��ɃA�j���[�V���������Z�b�g
            if (networkedAnimationName != "Attack")
            {
                //Attack�ȊO�Ȃ�܂��U�����p�[�e�B�N�����o��悤�ɐݒ肷��
                isAttack = 0;
            }
        }

        //���̃A�j���[�V�������U�����[�V�����Ȃ�p�[�e�B�N�����o��
        if (networkedAnimationName == "Attack")
        {
            Invoke("Omen", Omentime);
        }

        isActionInitialized = true;
    }

    void Omen()
    {
        isAttack = 1;
    }

    void StartNextAction()
    {
        if (players == null || players.Count < maxPlayerIndex)
        {
            Debug.LogError("Not enough players available!");
            return;
        }

        if (isDown)
        {
            Debug.Log("�_�E������");
            currentActionIndex = 0;
            currentSequenceIndex = Random.Range(0, actionSequence.Length);
            currentAction = downAction; // �_�E���A�N�V������ݒ�
            isActionInitialized = false;

            isDown = false; // �_�E����Ԃ�����
            isOnce = false; // �t���O�����Z�b�g


            return;
        }

        // �ʏ�̃A�N�V�����V�[�P���X�̏���
        if (currentActionIndex >= actionSequence[currentSequenceIndex].actions.Length)
        {
            Debug.Log("All actions completed");
            currentActionIndex = 0;
            currentSequenceIndex = Random.Range(0, actionSequence.Length);
        }

        currentAction = actionSequence[currentSequenceIndex].actions[currentActionIndex];
        isActionInitialized = false;
        currentActionIndex++;

        Debug.Log($"Starting Action: {currentAction.name}");
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
    }


    public override void Render()
    {
        // �N���C�A���g���ł��A�j���[�V�������Đ��i�l�b�g���[�N�ϐ����ς�����Ƃ��Ɏ��s�j
        if (animator != null && !string.IsNullOrEmpty((string)networkedAnimationName) && animator.GetCurrentAnimatorStateInfo(0).IsName((string)networkedAnimationName) == false)
        {
            Debug.Log($"Synchronizing animation: {networkedAnimationName}");
            animator.Play((string)networkedAnimationName);

        }

        //�����ύX����
        if (isDir)
        {
            transform.localScale = scale;
        }
        else
        {
            Vector3 temp = scale;
            temp.x = -scale.x;
            transform.localScale = temp;
        }

        if (isParticle == 2 || isParticle == 3)
        {
            switch (isParticle)
            {
                case 2:
                    // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�
                    newParticle = Instantiate(Dawnparticle);

                    //�p�[�e�B�N���𐶐�
                    newParticle.transform.position = this.transform.position;
                    // �p�[�e�B�N���𔭐�������
                    newParticle.Play();
                    isParticle = 3;
                    break;
            }

        }

        //�_�E����Ԃ��������ꂽ��_�E���p�[�e�B�N�����폜����
        //if (!isDown)
        //{
        //    // �C���X�^���X�������p�[�e�B�N���V�X�e����GameObject���폜
        //    Destroy(newParticle.gameObject, 0.01f);

        //    isParticle = 1;
        //}

        switch (isAttack)
        {
            case 1:
                // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�
                ParticleSystem OmenParticle = Instantiate(AttackOmenParticle);

                if (this.transform.localScale.x > 0)
                {
                    //�p�[�e�B�N���𐶐�
                    OmenParticle.transform.position = new Vector3(this.transform.position.x + OmenPosX, this.transform.position.y + OmenPosY, this.transform.position.z - 0.8f);
                    // �p�[�e�B�N���𔭐�������
                    OmenParticle.Play();

                    Destroy(OmenParticle.gameObject, 0.8f);
                }
                else
                {
                    //�p�[�e�B�N���𐶐�
                    OmenParticle.transform.position = new Vector3(this.transform.position.x - OmenPosX, this.transform.position.y + OmenPosY, this.transform.position.z - 0.8f);
                    // �p�[�e�B�N���𔭐�������
                    OmenParticle.Play();

                    Destroy(OmenParticle.gameObject, 0.8f);
                }

                isAttack = 2;
                break;

            case 2:
                break;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_AnimName()
    {
        Nokezori = NokezoriLimit;
        isInterrupted = true;
    }
}
