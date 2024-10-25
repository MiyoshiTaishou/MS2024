using UnityEngine;

[CreateAssetMenu(fileName = "JumpAction", menuName = "Boss/Actions/Jump")]
public class BossJumpAction : BossActionData
{
    public float jumpDuration;  // 空中で留まる時間
    public float jumpPower;     // ジャンプ力
    public float jumpHeight;    // ジャンプの高さ制限

    private float jumpStartTime;
    private bool isJumping;     // ジャンプ中かどうか
    private bool atPeak;        // 最高到達点に達したかどうか
    private Vector3 startPos;

    private Rigidbody rb;

    public override void InitializeAction(GameObject boss, Transform player)
    {
        rb = boss.GetComponent<Rigidbody>();

        // ジャンプ開始時刻を記録
        jumpStartTime = Time.time;
        isJumping = true;
        atPeak = false;

        // ジャンプ力を瞬時に適用
        Vector3 jumpForce = Vector3.up * jumpPower;
        rb.AddForce(jumpForce, ForceMode.Impulse);

        // 開始地点の記録
        startPos = boss.transform.position;
    }

    public override bool ExecuteAction(GameObject boss)
    {
        if (isJumping)
        {
            Vector3 nowPos = boss.transform.position;

            // Yの距離を計測
            float currentHeight = nowPos.y - startPos.y;

            //Debug.Log(currentHeight + "今の高さ" + jumpHeight + "目標の高さ");

            // 指定した高さに達したら
            if (currentHeight >= jumpHeight && !atPeak)
            {                
                // 最高到達点に達したので速度をゼロにし、フワフワと空中で留まる
                rb.velocity = Vector3.zero;
                rb.useGravity = false; // 重力を無効化して空中に留まる
                boss.transform.position = new Vector3(nowPos.x, startPos.y + jumpHeight, nowPos.z); // 高さを固定

                atPeak = true;         // 最高到達点に達したことを記録
                jumpStartTime = Time.time; // 浮いている時間の計測をリセット
            }

            // 一定時間が経過したら、重力を元に戻して落下させる
            if (atPeak && Time.time - jumpStartTime >= jumpDuration)
            {
                rb.useGravity = true; // 重力を有効にして再び落下
                isJumping = false;    // ジャンプ終了
            }

            return false;
        }
        else
        {
            if(rb.velocity.y == 0.0f)
            {
                return true; // ジャンプが終了したら true を返す
            }
            else
            {
                return false;
            }
        }       
    }
}
