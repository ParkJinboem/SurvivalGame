using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Animal : MonoBehaviour
{
    protected StatusController playerStatus;
    //동물의 이름
    public string animalName;
    //동물의 체력
    [SerializeField] protected int hp;

    [SerializeField] Item itemPrefab;
    //아이템획득 갯수
    public int itemNumber = 1;

    //걷기 스피드
    [SerializeField] protected float walkSpeed;
    //뛰기 스피드
    [SerializeField] protected float runSpeed;
    //목적지
    protected Vector3 destination;

    //상태변수
    //행동 중인지 아닌지 판별
    protected bool isAction;
    protected bool isWalking;
    protected bool isRunning;
    protected bool isChasing;
    public bool isDead;
    protected bool isAttacking;
    //걷기 시간
    [SerializeField] protected float walkTime;
    //대기 시간
    [SerializeField] protected float waitTime;
    //뛰는 시간
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
            Debug.Log("move실행");
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
        Debug.Log("Reset실행");
        isWalking = false;
        isRunning = false;
        isAction = true;
        nav.speed = walkSpeed;
        //목적지 초기화
        nav.ResetPath();
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        //랜덤으로 목적지 설정
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
        //일상 사운드 랜덤 재생
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
