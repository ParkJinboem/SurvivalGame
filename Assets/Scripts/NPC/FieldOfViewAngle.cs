using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfViewAngle : MonoBehaviour
{
    //시야갹_120도 
    [SerializeField] private float viewAngle;
    //시야거리_10미터
    [SerializeField] private float viewDistance;
    //타겟마스크_플레이어 
    [SerializeField] private LayerMask targetMask;

    private PlayerController playerController;
    private NavMeshAgent nav;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        nav = GetComponent<NavMeshAgent>();
    }

    public Vector3 GetTargetPos()
    {
        return playerController.transform.position;
    }

    private Vector3 BoundaryAngle(float angle)
    {
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public bool View()
    {
        Vector3 leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.position + transform.up, leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, rightBoundary, Color.red);

        Collider[] target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for (int i = 0; i < target.Length; i++)
        {
            Transform targetTf = target[i].transform;
            if(targetTf.name == "Player")
            {
                Vector3 direction = (targetTf.position - transform.position).normalized;
                float angle = Vector3.Angle(direction, transform.forward);

                if(angle < viewAngle * 0.5f)
                {
                    RaycastHit hit;
                    if(Physics.Raycast(transform.position + transform.up, direction, out hit, viewDistance))
                    {
                        if(hit.transform.name == "Player")
                        {
                            Debug.Log("플레이 어가 돼지 시야내 있습니다.");
                            Debug.DrawRay(transform.position + transform.up, direction, Color.blue);
                            return true;
                        }
                    }
                }
            }
            //플레이어가 주변에서 뛰고있다면
            if(playerController.GetRun())
            {
                if(CalcPathLength(playerController.transform.position) <= viewDistance)
                {
                    Debug.Log("돼지가 주변에서 뛰고잇는 플레이어를 파악");
                    return true;                      
                }
            }
        }
        return false;
    }

    private float CalcPathLength(Vector3 targetPos)
    {
        NavMeshPath path = new NavMeshPath();
        nav.CalculatePath(targetPos, path);

        Vector3[] wayPoint = new Vector3[path.corners.Length + 2];

        //웨이포인트 처음 부분
        wayPoint[0] = transform.position;
        //웨이포인트 마지막 부분
        wayPoint[path.corners.Length + 1] = targetPos;

        float pathLength = 0;
        for (int i = 0; i < path.corners.Length; i++)
        {
            //웨이 포인트에 경로를 넣음
            wayPoint[i + 1] = path.corners[i];
            //플레이어와 몬스터의 거리 (장애물이 있을시 장애물을 피하는 짧은거리를 계산)
            pathLength += Vector3.Distance(wayPoint[i], wayPoint[i + 1]);
        }
        return pathLength;
    }
}
