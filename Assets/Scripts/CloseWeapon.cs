using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName; //��Ŭ�̳� �Ǽ� ����

    //���� ����
    public bool isHand;
    public bool isAxe;
    public bool isPickaxe;

    public float range;     //���� ����
    public int damage;      //���ݷ�
    public float workSpeed; //�۾� �ӵ�
    public float attackDelay; //���� ������
    public float attackDelayA;  //���� Ȱ��ȭ ����
    public float attackDelayB;  //���� ��Ȱ��ȭ ����

    public float workDelay; //�۾� ������
    public float workDelayA;  //�۾� Ȱ��ȭ ����
    public float workDelayB;  //�۾� ��Ȱ��ȭ ����

    public Animator anim;       //�ִϸ��̼�
}