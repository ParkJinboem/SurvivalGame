using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodOnFire : MonoBehaviour
{
    //익히거나 타는데 걸리는 시간
    [SerializeField] private float time;
    private float currentTime;

    //끝났다면 더이상 불에 있어도 계산 무시할 수 있게
    private bool done;
    //익혀진 혹은 탄 아이템 교체
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
