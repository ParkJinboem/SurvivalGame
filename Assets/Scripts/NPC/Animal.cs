using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Animal : MonoBehaviour
{
    protected StatusController playerStatus;
    //������ �̸�
    public string animalName;
    //������ ü��
    [SerializeField] protected int hp;

    [SerializeField] Item itemPrefab;
    //������ȹ�� ����
    public int itemNumber = 1;

    //�ȱ� ���ǵ�
    [SerializeField] protected float walkSpeed;
    //�ٱ� ���ǵ�
    [SerializeField] protected float runSpeed;
    //������
    protected Vector3 destination;

    //���º���
    //�ൿ ������ �ƴ��� �Ǻ�
    protected bool isAction;
    protected bool isWalking;
    protected bool isRunning;
    protected bool isChasing;
    public bool isDead;
    protected bool isAttacking;
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
    protected NavMeshAgent nav;
    protected FieldOfViewAngle viewAngle;

    [SerializeField] protected AudioClip[] nomalSound;
    [SerializeField] protected AudioClip hurtSound;
    [SerializeField] protected AudioClip deadSound;
    void Start()
    {
        playerStatus = FindObjectOfType<StatusController>();
        viewAngle = GetComponent<FieldOfViewAngle>();
        nav = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!isDead)
        {
            Move();
            Elapsetime();
        }
    }
    protected void Move()
    {
        if (isWalking || isRunning)
        {
            Debug.Log("move����");
            nav.SetDestination(transform.position + destination * 5.0f);
        }
    }

    protected void Elapsetime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0 && !isChasing && !isAttacking)
            {
                ReSet();
            }
        }
    }

    protected virtual void ReSet()
    {
        Debug.Log("Reset����");
        isWalking = false;
        isRunning = false;
        isAction = true;
        nav.speed = walkSpeed;
        //������ �ʱ�ȭ
        nav.ResetPath();
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        //�������� ������ ����
        destination.Set(Random.Range(-0.2f, 0.2f), 0f, Random.Range(0.5f, 1.0f));
    }

    protected void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        nav.speed = walkSpeed;
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
        isChasing = false;
        isAttacking = false;
        isDead = true;
        nav.ResetPath();
        anim.SetTrigger("Dead");
        //StopAllCoroutines();
    }
    protected void RandomSound()
    {
        //�ϻ� ���� ���� ���
        int random = Random.Range(0, nomalSound.Length);
        PlaySE(nomalSound[random]);
    }
    protected void PlaySE(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public Item GetItem()
    {
        this.gameObject.tag = "Untagged";
        Destroy(this.gameObject, 3f);
        return itemPrefab;
    }
}
