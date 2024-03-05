using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twig : MonoBehaviour
{
    //ü��
    [SerializeField] private int hp;
    //����Ʈ ���� �ð�
    [SerializeField] private float destroyTime;
    //Ÿ�� ����Ʈ
    [SerializeField] private GameObject hitEffectPrefab;
    //���� �������� ������
    [SerializeField] private GameObject littleTwigPrefab;

    //ȸ���� ����
    private Vector3 originRot;
    private Vector3 wantedRot;
    private Vector3 currentRot;

    //�ʿ��� ���� �̸�
    [SerializeField] string hitSound;
    [SerializeField] string brokenSound;
    // Start is called before the first frame update
    void Start()
    {
        originRot = this.transform.eulerAngles;
        currentRot = originRot;
    }

    public void Damage(Transform playerTf)
    {
        hp--;
        Hit();
        StartCoroutine(HitSwayCoroutine(playerTf));
        if (hp <= 0)
        {
            Destruction();
        }
    }

    private void Hit()
    {
        SoundManager.instance.PlaySE(hitSound);

        GameObject clone = Instantiate(hitEffectPrefab,
                                       gameObject.GetComponent<BoxCollider>().bounds.center + (Vector3.up * 0.5f) ,
                                       Quaternion.identity);

        Destroy(clone, destroyTime);
    }

    IEnumerator HitSwayCoroutine(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 rotationDir = Quaternion.LookRotation(direction).eulerAngles;

        CheckDirection(rotationDir);
        
        while (!CheckThreshold())
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, 0.25f);
            transform.rotation = Quaternion.Euler(currentRot);

            yield return null;
        }
        
        wantedRot = originRot;

        while (!CheckThreshold())
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, 0.15f);
            transform.rotation = Quaternion.Euler(currentRot);

            yield return null;
        }
    }

    private bool CheckThreshold()
    {
        if(Mathf.Abs(wantedRot.x - currentRot.x) <= 0.5f && Mathf.Abs(wantedRot.z - currentRot.z) <= 0.5f)
        {
            return true;
        }

        return false;
    }


    private void CheckDirection(Vector3 rotationDir)
    {
        Debug.Log(rotationDir);
        if(rotationDir.y > 180)
        {
            if(rotationDir.y > 300)
            {
                wantedRot = new Vector3(-50f, 0f, -50f);
            }
            else if(rotationDir.y > 240)
            {
                wantedRot = new Vector3(0f, 0f, -50f);
            }
            else
            {
                wantedRot = new Vector3(50f, 0f, -50f);
            }
        }
        else if (rotationDir.y <= 180)
        {
            if (rotationDir.y < 60)
            {
                wantedRot = new Vector3(-50f, 0f, 50f);
            }
            else if (rotationDir.y > 120)
            {
                wantedRot = new Vector3(0f, 0f, 50f);
            }
            else
            {
                wantedRot = new Vector3(50f, 0f, 50f);
            }
        }
    }

    private void Destruction()
    {
        SoundManager.instance.PlaySE(brokenSound);
        GameObject clone = Instantiate(littleTwigPrefab,
                                      gameObject.GetComponent<BoxCollider>().bounds.center + (Vector3.up * 0.5f),
                                      Quaternion.identity);

        GameObject clone2 = Instantiate(littleTwigPrefab,
                                      gameObject.GetComponent<BoxCollider>().bounds.center - (Vector3.up * 0.5f),
                                      Quaternion.identity);

        Destroy(clone, destroyTime);
        Destroy(clone2, destroyTime);
        Destroy(gameObject);
    }
}