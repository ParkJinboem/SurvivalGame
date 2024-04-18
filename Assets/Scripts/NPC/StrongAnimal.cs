using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAnimal : Animal
{
    //공격 데미지
    [SerializeField] protected int attackDamage;
    //공격 딜레이
    [SerializeField] protected float attackDelay;
    //공격 딜레이
    [SerializeField] protected float attackDistance;
    //플레이어
    [SerializeField] protected LayerMask targetMask;

    //총 추적시간
    [SerializeField] protected float chaseTime;
    //계산
    protected float currentChaseTime;
    //추적 딜레이
    [SerializeField] protected float chaseDelayTime;

    //추적
    public void Chase(Vector3 targetPos)
    {
        isChasing = true;
        //플레이어 추적하도록 설정
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
            //돼지와 플레이어의 거리가 짧으면 공겨 시도
            if (Vector3.Distance(transform.position, viewAngle.GetTargetPos()) < 3.0f)
            {
                //플레이어가 돼지 눈앞에 있으면 공격 시도
                if (viewAngle.View())
                {
                    Debug.Log("플레이어 공격 시도");
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
            Debug.Log("플레이어 빗나감");
        }
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
        StartCoroutine(ChaseTargetCoroutine());
    }
}
