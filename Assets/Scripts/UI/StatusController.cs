using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    //ü��
    [SerializeField] private int hp;
    private int currentHp;
    //���¹̳�
    [SerializeField] private int sp;
    private int currentSp;
    //���¹̳� ������
    [SerializeField] private int spIncreaseSpeed;
    //���¹̳� ��ȸ�� ������
    [SerializeField] private int spRechargeTime;
    private int currentSpRechargeTime;
    //���¹̳� ���� ����
    private bool spUsed;
    //����
    [SerializeField] private int dp;
    private int currentDp;
    //�����
    [SerializeField] private int hungry;
    private int currentHungry;
    //�񸶸�
    [SerializeField] private int thirsty;
    private int currentThirsty;
    //������� �پ��� �ӵ�
    [SerializeField] private int hungryDecreaseTime;
    private int currentHungryDecreaseTime;
    //�񸶸��� �پ��� �ӵ�
    [SerializeField] private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;
    //������
    [SerializeField] private int satisfy;
    private int currentSatisfy;
    //�ʿ��� �̹���
    [SerializeField] private Image[] imagesGauge;

    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;
        currentDp = dp;
        currentSp = sp;
        currentHungry = hungry;
        currentThirsty = thirsty;
        currentSatisfy = satisfy;
    }

    // Update is called once per frame
    void Update()
    {
        Hungry();
        Thirsty();
        SPRechargeTime();
        SPRecover();
        GaugeUpdate();
    }
    private void SPRechargeTime()
    {
        if(spUsed)
        {
            if(currentSpRechargeTime < spRechargeTime)
            {
                currentSpRechargeTime++;
            }
            else
            {
                spUsed = false;
            }
        }
    }

    private void SPRecover()
    {
        if(!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed;
        }

    }
    private void Hungry()
    {
        if (currentHungry > 0)
        {
            if (currentHungryDecreaseTime <= hungryDecreaseTime)
            {
                currentHungryDecreaseTime++;
            }
            else
            {
                currentHungry--;
                currentHungryDecreaseTime = 0;
            }
        }
        else
        {
            Debug.Log("����� ��ġ�� 0�� �Ǿ����ϴ�.");
        }
    }

    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
            {
                currentThirstyDecreaseTime++;
            }
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }
        }
        else
        {
            Debug.Log("�񸶸� ��ġ�� 0�� �Ǿ����ϴ�.");
        }
    }

    private void GaugeUpdate()
    {
        imagesGauge[HP].fillAmount = (float)currentHp / hp;
        imagesGauge[DP].fillAmount = (float)currentDp / dp;
        imagesGauge[SP].fillAmount = (float)currentSp / sp;
        imagesGauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        imagesGauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        imagesGauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    public void IncreaseHP(int count)
    {
        if(currentHp + count < hp)
        {
            currentHp += count;
        }
        else
        {
            currentHp = hp;
        }
    }

    public void DecreaseHP(int count)
    {
        if (currentDp > 0)
        {
            DecreaseDP(count);
            return;
        }

        currentHp -= count;
        if(currentHp <=0)
        {
            Debug.Log("ĳ������ hp�� 0�̵Ǿ����ϴ�.");
        }
    }

    public void IncreaseSP(int count)
    {
        if (currentSp + count < sp)
        {
            currentSp += count;
        }
        else
        {
            currentSp = sp;
        }
    }

    public void DecreaseSP(int count)
    {
        if (currentSp - count < 0)
        {
            currentSp = 0;
        }
        else
        {
            currentSp -= count;
        }
    }

    public void IncreaseDP(int count)
    {
        if (currentDp + count < hp)
        {
            currentDp += count;
        }
        else
        {
            currentDp = dp;
        }
    }

    public void DecreaseDP(int count)
    {
        currentDp -= count;
        if (currentDp <= 0)
        {
            Debug.Log("ĳ������ dp�� 0�̵Ǿ����ϴ�.");
        }
    }

    public void IncreaseHungry(int count)
    {
        if (currentHungry + count < hungry)
        {
            currentHungry += count;
        }
        else
        {
            currentHungry = hungry;
        }
    }

    public void DecreaseHungry(int count)
    {
        if(currentHungry - count < 0)
        {
            currentHungry = 0;
        }
        else
        {
            currentHungry -= count;
        }
    }

    public void IncreaseThirsty(int count)
    {
        if (currentThirsty + count < thirsty)
        {
            currentThirsty += count;
        }
        else
        {
            currentThirsty = thirsty;
        }
    }

    public void DecreaseThirsty(int count)
    {
        if (currentThirsty - count < 0)
        {
            currentThirsty = 0;
        }
        else
        {
            currentThirsty -= count;
        }
    }
    public void DecreaseStamina(int count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;

        if(currentSp - count > 0)
        {
            currentSp -= count;
        }
        else
        {
            currentSp = 0;
        }
    }

    public int GetCurrentSp()
    {
        return currentSp;
    }
}
