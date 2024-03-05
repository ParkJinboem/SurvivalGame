using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    //������ �̸�
    [SerializeField] protected string animalName;
    //������ ü��
    [SerializeField] protected int hp;
    //�ȱ� ���ǵ�
    [SerializeField] protected float walkSpeed;
    //�ٱ� ���ǵ�
    [SerializeField] protected float runSpeed;
    //ȸ�� ���ǵ�
    [SerializeField] protected float  turningSpeed;
    //���� ���ǵ�
    [SerializeField] protected float applySpeed;
    //����
    protected Vector3 direction;

    //���º���
    //�ൿ ������ �ƴ��� �Ǻ�
    protected bool isAction;
    protected bool isWalking;
    protected bool isRunning;
    protected bool isDead;
    //�ȱ� �ð�
    [SerializeField] protected float walkTime;
    //��� �ð�
    [SerializeField] protected float waitTime;
    //�ٴ� �ð�
    [SerializeField] protected float runTime;
    protected float currentTime;

    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] protected BoxCollider boxCol;
    protected AudioSource audioSource;
    [SerializeField] protected AudioClip[] nomalSound;
    [SerializeField] protected AudioClip hurtSound;
    [SerializeField] protected AudioClip deadSound;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Move();
            Rotation();
            Elapsetime();
        }
    }
    protected void Move()
    {
        if (isWalking || isRunning)
        {
            rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
        }
    }

    protected void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), turningSpeed);
            rigid.MoveRotation(Quaternion.Euler(rotation));
        }
    }

    protected void Elapsetime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                ReSet();
            }
        }
    }

    protected virtual void ReSet()
    {
        isWalking = false;
        isRunning = false;
        isAction = true;
        applySpeed = walkSpeed;
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        direction.Set(0f, Random.Range(0f, 360f), 0);
    }



    protected void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        applySpeed = walkSpeed;
    }

    public virtual void Damage(int damge, Vector3 targetPos)
    {
        if (!isDead)
        {
            hp -= damge;
            if (hp <= 0)
            {
                Dead();
                return;
            }
            PlaySE(hurtSound);
            anim.SetTrigger("Hurt");
        }
    }

    protected void Dead()
    {
        PlaySE(deadSound);
        isWalking = false;
        isRunning = false;
        isDead = true;
        anim.SetTrigger("Dead");
    }
    protected void RandomSound()
    {
        //�ϻ� ���� ���� ���
        int random = Random.Range(0, 3);
        PlaySE(nomalSound[random]);
    }
    protected void PlaySE(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
