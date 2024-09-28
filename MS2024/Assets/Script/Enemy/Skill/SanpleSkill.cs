using UnityEngine;

public class SampleSkill : SkillBase
{
    public override void UseSkill(Transform bossTransform, Transform targetTransform){
        Debug.Log("サンプルスキルを使用");

        // 例：プレイヤーとのベクトルで直線上にダメージ判定を設置
        Vector3 direction = (targetTransform.position - bossTransform.position).normalized;
        Vector3 position = bossTransform.position + direction * 5f; // 任意の距離

        Instantiate(skillObj, position, Quaternion.LookRotation(direction));
        // Vector3 position = transform.position + playerData[targetPlayer].direction.normalized * i; // 正規化
        // Instantiate(skill[0].skillObj, position, Quaternion.identity);
    }
}
