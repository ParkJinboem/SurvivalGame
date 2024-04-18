using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    //불의 이름(장작,난로 등등)
    [SerializeField] private string fireName;
    //불의 데미지
    [SerializeField] private int damage;
    //데미지가 들어갈 딜레이
    [SerializeField] private float damageTime;
    private float currentDamageTime;
    //불의 지속 시간
    [SerializeField] private float durationTime;
    private float currentDurationTime;
    //파티클 시스템
    [SerializeField] private ParticleSystem fireParticle;

    private StatusController statusController;

    //상태변수
    private bool isFire = true;

    private void Start()
    {
        statusController = FindObjectOfType<StatusController>();
        currentDurationTime = durationTime;
    }
    void Update()
    {
        if(isFire)
        {
            ElapseTime();
        }
    }

    private void ElapseTime()
    {
        currentDurationTime -= Time.deltaTime;

        if(currentDamageTime > 0)
        {
            currentDamageTime -= Time.deltaTime;
        }
        if(currentDurationTime <= 0)
        {
            Off();
        }
    }

    private void Off()
    {
        fireParticle.Stop();
        isFire = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(isFire && other.transform.tag == "Player")
        {
            if(currentDamageTime <= 0)
            {
                other.GetComponent<Burn>().StartBurning();
                statusController.DecreaseHP(damage);
                currentDamageTime = damageTime;
            }
        }
    }

    public bool GetIsFire()
    {
        return isFire;
    }
}
