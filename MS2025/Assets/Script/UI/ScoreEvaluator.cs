using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEvaluator : NetworkBehaviour
{
    public enum ScoreRank { S, A, B }

    [SerializeField, Header("�X�R�A�摜")]
    private Sprite[] images = null;

    // �X�e�[�W�̃X�R�A����
    [System.Serializable]
    public class ScoreCriteria
    {
        public int maxComboForS;
        public int maxComboForA;
        public float clearTimeForS;
        public float clearTimeForA;
        public int jumpAttacksForS;
        public int jumpAttacksForA;
    }

    [Header("�X�e�[�W1�̃X�R�A����")]
    public ScoreCriteria stage1Criteria = new ScoreCriteria
    {
        maxComboForS = 12,
        maxComboForA = 8,
        clearTimeForS = 30f,
        clearTimeForA = 60f,
        jumpAttacksForS = 5,
        jumpAttacksForA = 2
    };

    // �X�R�A�]�����\�b�h
    public ScoreRank EvaluateScore(int maxCombo, float clearTime, int jumpAttacks)
    {
        ScoreCriteria criteria = stage1Criteria;

        // S�����N�̕]��
        if (maxCombo >= criteria.maxComboForS &&
            clearTime <= criteria.clearTimeForS &&
            jumpAttacks >= criteria.jumpAttacksForS)
        {
            return ScoreRank.S;
        }

        // A�����N�̕]��
        if (maxCombo >= criteria.maxComboForA &&
            clearTime <= criteria.clearTimeForA &&
            jumpAttacks >= criteria.jumpAttacksForA)
        {
            return ScoreRank.A;
        }

        // B�����N�̕]��
        return ScoreRank.B;
    }

    public override void Spawned()
    {
        ScoreRank rank = EvaluateScore(ScoreManager.maxCombo, ScoreManager.clearTime, ScoreManager.maxMultiAttack);

        Debug.Log(rank);
        switch (rank)
        {
            case ScoreRank.S:
                GetComponent<Image>().sprite = images[0];
                break;
            case ScoreRank.A:
                GetComponent<Image>().sprite = images[1];
                break;
            case ScoreRank.B:
                GetComponent<Image>().sprite = images[2];
                break;
        }
    }
}