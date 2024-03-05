using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] int hp; //������ ü��
    [SerializeField] float destroyTime; //���� ���� �ð�
    [SerializeField] SphereCollider col; //��ü �ݶ��̴�
    [SerializeField] GameObject rock; //�Ϲݹ���
    [SerializeField] GameObject debrisRock; //�ı��� ����
    [SerializeField] GameObject effectPrefabs; //ä�� ����Ʈ
    [SerializeField] GameObject itemPrefab; //ä�� ������
    [SerializeField] private int count; //ä�� ������ ���� ����
    // �ʿ��� ���� �̸�
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

