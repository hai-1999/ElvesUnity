using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 3;
    private bool isMoving;

    private Vector2 input;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();//�ӵ�ǰ��Ϸ�����ȡ�������������
    }
     
    private void Update()//ÿ֡����һ��
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");// ��ȡX�������ֵ
            input.y = Input.GetAxisRaw("Vertical");// ��ȡY�������ֵ

            if (input.x != 0) input.y = 0;//��ֹб���ƶ�

            if (input != Vector2.zero)//���X���Y�����벻Ϊ��
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos =this.transform.position;//���Ŀ��λ������
                targetPos.x += input.x;//�����ƶ����X������
                targetPos.y += input.y;//�����ƶ����Y������

                StartCoroutine(Move(targetPos));//����Э��
            }
        }
        animator.SetBool("isMoving", isMoving);
    }

 /*Э����һ������һ����������ִͣ�У������Ժ�ָ�ִ�еĺ�����������һ��������Ϊ���С����ÿһС����������ڶ��֡����ɣ���������һ֡���������̡߳���Unity�У�Э��ͨ��StartCoroutine����������������ͨ��yield�ؼ�������ִͣ�У�ֱ�������ض�����������ȴ�ʱ�䡢��֡���ȴ���һ��Э����ɵȣ���������С�*/
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
           
            yield return null;//�ȴ�һ֡         
        }
        //transform.position = targetPos;
        isMoving = false;
    }
}
