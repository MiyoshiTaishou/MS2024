using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class SampleSkill : SkillBase
{
    private float nowTime;
    private Transform bossTransform;
    private Transform targetTransform;
    private Vector3 direction;

    public void Update() {
        float preliminarySpace = preliminaryTime / maxSkillCount * (maxSkillCount - nowSkillCount);
        float attaclSpace = maxAttackRange / (maxSkillCount + 1);
        if (nowSkillCount < maxSkillCount && preliminarySpace <= nowTime){
            Vector3 position = bossTransform.position + direction * (attaclSpace * (nowSkillCount + 1)); // 任意の距離
            position.y = 0;//bossTransform.position.y;

            Instantiate(skillObj, position, Quaternion.LookRotation(direction));
            skillObj.GetComponent<AttackEffect>().delayEffect = preliminarySpace;
            nowSkillCount++;
        }
        nowTime += Time.deltaTime;
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
        if (nowSkillCount >= maxSkillCount) return false;
        return true;
    }
}
