using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : MonoBehaviour
{
    private bool isBurning = false;

    [SerializeField] private int damage;
    [SerializeField] private float damageTime;
    private float currentDamageTime;

    [SerializeField] private float durationTime;
    private float currentDurationTime;

    //불 붙으면 프리팹 생성
    [SerializeField] private GameObject flamePrefab;
    private GameObject tempFlame;

    public void StartBurning()
    {
        if(!isBurning)
        {
            tempFlame = Instantiate(flamePrefab, transform.position, Quaternion.Euler(new Vector3(-90,0,0)));
            tempFlame.transform.SetParent(transform);
        }
        isBurning = true;
        currentDurationTime = durationTime;

    }
    // Update is called once per frame
    void Update()
    {
        if(isBurning)
        {
            ElapseTime();
        }
    }

    private void ElapseTime()
    {
        if(isBurning)
        {
            currentDurationTime -= Time.deltaTime;

            if(currentDamageTime > 0)
            {
                currentDamageTime -= Time.deltaTime;
            }
            if(currentDamageTime <= 0)
            {
                Damage();
            }

            if(currentDurationTime <= 0)
            {
                Off();
            }
        }
    }

    private void Damage()
    {
        currentDamageTime = damageTime;
        GetComponent<StatusController>().DecreaseHP(damage);
    }
    private void Off()
    {
        isBurning = false;
        Destroy(tempFlame);
    }
}
