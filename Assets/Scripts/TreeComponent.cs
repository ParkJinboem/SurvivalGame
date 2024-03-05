using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class TreeComponent : MonoBehaviour
{
    //깍일 나무 조각들
    [SerializeField] private GameObject[] treePices;
    [SerializeField] private GameObject treeCenter;
    //통나무
    [SerializeField] private GameObject logPrefab;

    //나무 쓰러질때 랜덤으로 가해질 힘의 세기
    [SerializeField] private float force;
    // 자식 트리
    [SerializeField] private GameObject childTree;

    //부모 트리 파괴되면, 캡슐 콜라이더 제거
    [SerializeField] private CapsuleCollider parentCol;
    // 자식 트리 쓰러지면 필요한 컴포넌트 활성화 및 중력 활성화
    [SerializeField] private CapsuleCollider childCol;
    [SerializeField] private Rigidbody childRigid;

    // 파편
    [SerializeField] private GameObject hitEffectPrefab;
    //파편제거 시간
    [SerializeField] private float debrisDestroyTime;
    //나무 제거 시간
    [SerializeField] private float destroyTime;

    //필요한 사운드
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

    //적중 이펙트
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
