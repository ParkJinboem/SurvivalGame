using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAnimal : Animal
{
    public void Run(Vector3 targetPos)
    {
        //�÷��̾� �ݴ�������κ��� �������� ����
        destination = new Vector3(transform.position.x - targetPos.x, 0f, transform.position.z - targetPos.z).normalized;

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        nav.speed = runSpeed;
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
