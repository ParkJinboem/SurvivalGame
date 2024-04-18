using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAnimal : Animal
{
    //���� ������
    [SerializeField] protected int attackDamage;
    //���� ������
    [SerializeField] protected float attackDelay;
    //���� ������
    [SerializeField] protected float attackDistance;
    //�÷��̾�
    [SerializeField] protected LayerMask targetMask;

    //�� �����ð�
    [SerializeField] protected float chaseTime;
    //���
    protected float currentChaseTime;
    //���� ������
    [SerializeField] protected float chaseDelayTime;

    //����
    public void Chase(Vector3 targetPos)
    {
        isChasing = true;
        //�÷��̾� �����ϵ��� ����
        destination = targetPos; 
        nav.speed = runSpeed;
        isRunning = true;
        anim.SetBool("Running", isRunning);
        nav.SetDestination(destination);
    }

    public override void Damage(int damge, Vector3 targetPos)
    {
        base.Damage(damge, targetPos);
        if (!isDead)
        {
            Chase(targetPos);
        }
    }

    protected IEnumerator ChaseTargetCoroutine()
    {
        currentChaseTime = 0;
        Chase(viewAngle.GetTargetPos());

        while (currentChaseTime < chaseTime)
        {
            Chase(viewAngle.GetTargetPos());
            //������ �÷��̾��� �Ÿ��� ª���� ���� �õ�
            if (Vector3.Distance(transform.position, viewAngle.GetTargetPos()) < 3.0f)
            {
                //�÷��̾ ���� ���տ� ������ ���� �õ�
                if (viewAngle.View())
                {
                    Debug.Log("�÷��̾� ���� �õ�");
                    StartCoroutine(AttackCoroutine());
                }
            }
            yield return new WaitForSeconds(chaseDelayTime);
            currentChaseTime += chaseDelayTime;
        }

        isChasing = false;
        isRunning = false;
        anim.SetBool("Running", isRunning);
        nav.ResetPath();
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        nav.ResetPath();
        currentTime = chaseTime;
        yield return new WaitForSeconds(0.5f);
        transform.LookAt(viewAngle.GetTargetPos());
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, attackDistance, targetMask))
        {
            playerStatus.DecreaseDP(attackDamage);
        }
        else
        {
            Debug.Log("�÷��̾� ������");
        }
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
        StartCoroutine(ChaseTargetCoroutine());
    }
}
