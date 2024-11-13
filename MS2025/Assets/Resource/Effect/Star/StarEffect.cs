using UnityEngine;

public class StarEffect : MonoBehaviour
{
    private float speed = 0.01f; //floatは小数点
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 pos = transform.position;
        if (Input.GetKey(KeyCode.DownArrow))//↓キーを押したら
        {
            pos.y -= speed;
            animator.SetInteger("direction", 0);//正面を向く
        }
        else if (Input.GetKey(KeyCode.LeftArrow))//←キーを押したら
        {

            pos.x -= speed;
            animator.SetInteger("direction", 1);//左を向く
        }
        else if (Input.GetKey(KeyCode.RightArrow))//→キーを押したら
        {
            pos.x += speed;
            animator.SetInteger("direction", 2);//右を向く

        }
        else if (Input.GetKey(KeyCode.UpArrow))//↑キーを押したら
        {
            pos.y += speed;
            animator.SetInteger("direction", 3);//後ろを向く
        }
        transform.position = pos;
    }
}