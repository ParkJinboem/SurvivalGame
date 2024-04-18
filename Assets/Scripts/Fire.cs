using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    //���� �̸�(����,���� ���)
    [SerializeField] private string fireName;
    //���� ������
    [SerializeField] private int damage;
    //�������� �� ������
    [SerializeField] private float damageTime;
    private float currentDamageTime;
    //���� ���� �ð�
    [SerializeField] private float durationTime;
    private float currentDurationTime;
    //��ƼŬ �ý���
    [SerializeField] private ParticleSystem fireParticle;

    private StatusController statusController;

    //���º���
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
