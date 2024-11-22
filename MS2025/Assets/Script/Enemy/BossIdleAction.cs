using UnityEngine;

[CreateAssetMenu(fileName = "WaitAction", menuName = "Boss/Actions/Wait")]
public class WaitAction : BossActionData
{
    public float waitDuration;
    private float waitStartTime;
 

    public override void InitializeAction(GameObject boss, Transform player)
    {
        waitStartTime = Time.time;     
        boss.GetComponent<Rigidbody>().useGravity = true;
    }

    public override bool ExecuteAction(GameObject boss, Transform player)
    {
        boss.GetComponent<Rigidbody>().useGravity = true;

       
        if (Time.time - waitStartTime >= waitDuration)
        {
           
            return true; // アクション完了
        }

        return false; // まだ実行中
    }
 
    }
