using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodOnFire : MonoBehaviour
{
    //�����ų� Ÿ�µ� �ɸ��� �ð�
    [SerializeField] private float time;
    private float currentTime;

    //�����ٸ� ���̻� �ҿ� �־ ��� ������ �� �ְ�
    private bool done;
    //������ Ȥ�� ź ������ ��ü
    [SerializeField] GameObject cookedItemPrefab;

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == "Fire" && !done)
        {
            currentTime += Time.deltaTime;

            if(currentTime >= time)
            {
                done = true;
                Instantiate(cookedItemPrefab, transform.position, Quaternion.Euler(transform.eulerAngles));
                Destroy(gameObject);
            }
        }
    }

}
