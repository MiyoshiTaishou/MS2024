using UnityEngine;

public class StarEffect : MonoBehaviour
{
    private float speed = 0.01f; //float�͏����_
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 pos = transform.position;
        if (Input.GetKey(KeyCode.DownArrow))//���L�[����������
        {
            pos.y -= speed;
            animator.SetInteger("direction", 0);//���ʂ�����
        }
        else if (Input.GetKey(KeyCode.LeftArrow))//���L�[����������
        {

            pos.x -= speed;
            animator.SetInteger("direction", 1);//��������
        }
        else if (Input.GetKey(KeyCode.RightArrow))//���L�[����������
        {
            pos.x += speed;
            animator.SetInteger("direction", 2);//�E������

        }
        else if (Input.GetKey(KeyCode.UpArrow))//���L�[����������
        {
            pos.y += speed;
            animator.SetInteger("direction", 3);//��������
        }
        transform.position = pos;
    }
}