using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class SampleSkill : SkillBase
{
    private float nowTime;
    private Transform bossTransform;
    private Transform targetTransform;
    private Vector3 direction;

    public void Update() {
        nowTime += Time.deltaTime;
    }

    public void FixedUpdate() {
        float preliminarySpace = preliminaryTime / maxSkillCount * (maxSkillCount - nowSkillCount);
        float attaclSpace = maxAttackRange / (maxSkillCount + 1);
        if (nowSkillCount < maxSkillCount && preliminarySpace <= nowTime){
            Debug.LogError("");
            Debug.LogError("攻撃の発生までの時間："+preliminarySpace);
            Debug.LogError("攻撃と攻撃の間の距離："+attaclSpace);
            Vector3 position = bossTransform.position + direction * (attaclSpace * (nowSkillCount + 1));
            position.y = 0;//bossTransform.position.y;
            Debug.LogError("攻撃発生回数："+nowSkillCount);
            Debug.LogError("攻撃発生位置："+position);

            Instantiate(skillObj, position, Quaternion.LookRotation(direction));
            skillObj.GetComponent<AttackEffect>().delayEffect = preliminarySpace;
            nowSkillCount++;
        }
    }

    public override void UseSkill(Transform BTF, Transform TTF){
        bossTransform = BTF;
        targetTransform = TTF;
        nowSkillCount = 0;

        // 例：プレイヤーとのベクトルで直線上にダメージ判定を設置
        direction = (targetTransform.position - bossTransform.position).normalized;
        direction.y = 0;
        direction = direction.normalized;
    }

    public override bool IsSkillUsing() {
        if (nowSkillCount >= maxSkillCount ) return false;
        return true;
    }
}
