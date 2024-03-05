using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAnimal : Animal
{

    public void Run(Vector3 targetPos)
    {
        //�÷��̾� �ݴ�������κ��� �������� ����
        direction = Quaternion.LookRotation(transform.position - targetPos).eulerAngles;

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;
        anim.SetBool("Running", isRunning);
    }

    public override void Damage(int damge, Vector3 targetPos)
    {
        base.Damage(damge, targetPos);
        if(!isDead)
        {
            Run(targetPos);
        }
    }
}
