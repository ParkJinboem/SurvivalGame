using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] int hp; //바위의 체력
    [SerializeField] float destroyTime; //파편 제거 시간
    [SerializeField] SphereCollider col; //구체 콜라이더
    [SerializeField] GameObject rock; //일반바위
    [SerializeField] GameObject debrisRock; //파괴된 바위
    [SerializeField] GameObject effectPrefabs; //채굴 이펙트
    [SerializeField] GameObject itemPrefab; //채굴 아이템
    [SerializeField] private int count; //채굴 아이템 등장 개수
    // 필요한 사운드 이름
    [SerializeField] private string strikeSound;
    [SerializeField] private string destroySound;
    public void Mining()
    {
        SoundManager.instance.PlaySE(strikeSound);
        GameObject clone = Instantiate(effectPrefabs, col.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime);
        hp--;
        if(hp <= 0)
        {
            Destruction();
        }
    }

    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroySound);
        col.enabled = false;
        for (int i = 0; i < count; i++)
        {
            Instantiate(itemPrefab, rock.transform.position, Quaternion.identity);
        }
        Destroy(rock);
        debrisRock.SetActive(true);
        Destroy(debrisRock, destroyTime);
    }
}

