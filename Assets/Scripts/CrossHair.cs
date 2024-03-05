using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // ũ�ν���� ���¿� ���� ���� ��Ȯ��
    private float gunAccuracy;

    //ũ�ν���� Ȱ��ȭ,��Ȱ��ȭ
    [SerializeField] private GameObject crossHairHUd;
    [SerializeField] private GunController gunController;
    public void WalkingAnimation(bool flag)
    {
        WeaponManager.currentWeaponAnim.SetBool("Walk", flag);
        animator.SetBool("Walking", flag);
    }

    public void RunningAnimation(bool flag)
    {
        WeaponManager.currentWeaponAnim.SetBool("Run", flag);
        animator.SetBool("Running", flag);
    }

    public void JumpingAnimation(bool flag)
    {
        animator.SetBool("Running", flag);
    }

    public void CrouchingAnimation(bool flag)
    {
        animator.SetBool("Crouching", flag);
    }

    public void FineSightAnimation(bool flag)
    {
        animator.SetBool("FineSight", flag);
    }

    public void FireAnimation()
    {
        if(animator.GetBool("Walking"))
        {
            animator.SetTrigger("WalkFire");
        }
        else if(animator.GetBool("Crouching"))
        {
            animator.SetTrigger("CrouchFire");
        }
        else
        {
            animator.SetTrigger("IdleFire");
        }
    }

    public float GetAccuracy()
    {
        if (animator.GetBool("Walking"))
        {
            gunAccuracy = 0.06f;
        }
        else if (animator.GetBool("Crouching"))
        {
            gunAccuracy = 0.015f;
        }
        else if(gunController.GetFineSightMode())
        {
            gunAccuracy = 0.001f;
        }
        else
        {
            gunAccuracy = 0.035f;
        }
        return gunAccuracy;
    }
}