using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class TreeComponent : MonoBehaviour
{
    //���� ���� ������
    [SerializeField] private GameObject[] treePices;
    [SerializeField] private GameObject treeCenter;
    //�볪��
    [SerializeField] private GameObject logPrefab;

    //���� �������� �������� ������ ���� ����
    [SerializeField] private float force;
    // �ڽ� Ʈ��
    [SerializeField] private GameObject childTree;

    //�θ� Ʈ�� �ı��Ǹ�, ĸ�� �ݶ��̴� ����
    [SerializeField] private CapsuleCollider parentCol;
    // �ڽ� Ʈ�� �������� �ʿ��� ������Ʈ Ȱ��ȭ �� �߷� Ȱ��ȭ
    [SerializeField] private CapsuleCollider childCol;
    [SerializeField] private Rigidbody childRigid;

    // ����
    [SerializeField] private GameObject hitEffectPrefab;
    //�������� �ð�
    [SerializeField] private float debrisDestroyTime;
    //���� ���� �ð�
    [SerializeField] private float destroyTime;

    //�ʿ��� ����
    [SerializeField] private string chopSound;
    [SerializeField] private string falldownSound;
    [SerializeField] private string logChangeSound;
        
    public void Chop(Vector3 pos, float angleY)
    {
        Hit(pos);
        AngleCalc(angleY);

        if(CheckTreePieces())
        {
            return;
        }
        FallDownTree();
    }

    //���� ����Ʈ
    private void Hit(Vector3 pos)
    {
        SoundManager.instance.PlaySE(chopSound);

        GameObject clone = Instantiate(hitEffectPrefab, pos, Quaternion.Euler(Vector3.zero));
        Destroy(clone, debrisDestroyTime);
    }

    private void AngleCalc(float angleY)
    {
        if(0 <= angleY && angleY <=70)
        {
            DestroyPiece(2);
        }
        else if (70 <= angleY && angleY <= 140)
        {
            DestroyPiece(3);
        }
        else if (140 <= angleY && angleY <= 210)
        {
            DestroyPiece(4);
        }
        else if (210 <= angleY && angleY <= 280)
        {
            DestroyPiece(0);
        }
        else if (280 <= angleY && angleY <= 360)
        {
            DestroyPiece(1);
        }
    }

    private void DestroyPiece(int num)
    {
        if (treePices[num] != null)
        {
            GameObject clone = Instantiate(hitEffectPrefab, treePices[num].transform.position, Quaternion.Euler(Vector3.zero));
            Destroy(clone, debrisDestroyTime);
            Destroy(treePices[num].gameObject);
        }
    }

    private bool CheckTreePieces()
    {
        for (int i = 0; i < treePices.Length; i++)
        {
            if (treePices[i].gameObject != null)
            {
                return true;
            }
        }
        return false; 
    }

    private void FallDownTree()
    {
        SoundManager.instance.PlaySE(falldownSound);
        Destroy(treeCenter);

        parentCol.enabled = false;
        childCol.enabled = true;
        childRigid.useGravity = true;
        childRigid.AddForce(Random.Range(-force,force), 0f, Random.Range(-force, force));

        StartCoroutine(LogCoroutine());
    }
    IEnumerator LogCoroutine()
    {
        
        yield return new WaitForSeconds(destroyTime);
        
        SoundManager.instance.PlaySE(logChangeSound);

        Instantiate(logPrefab, childTree.transform.position + (childTree.transform.up * 3f), Quaternion.LookRotation(childTree.transform.up));
        Instantiate(logPrefab, childTree.transform.position + (childTree.transform.up * 6f), Quaternion.LookRotation(childTree.transform.up));
        Instantiate(logPrefab, childTree.transform.position + (childTree.transform.up * 9f), Quaternion.LookRotation(childTree.transform.up));

        Destroy(childTree);
    }

    public Vector3 GetTreeCenterPosition()
    {
        return treeCenter.transform.position;
    }
}
