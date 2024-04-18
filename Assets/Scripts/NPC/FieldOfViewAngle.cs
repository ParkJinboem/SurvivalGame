using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfViewAngle : MonoBehaviour
{
    //�þ߰�_120�� 
    [SerializeField] private float viewAngle;
    //�þ߰Ÿ�_10����
    [SerializeField] private float viewDistance;
    //Ÿ�ٸ���ũ_�÷��̾� 
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
                            Debug.Log("�÷��� � ���� �þ߳� �ֽ��ϴ�.");
                            Debug.DrawRay(transform.position + transform.up, direction, Color.blue);
                            return true;
                        }
                    }
                }
            }
            //�÷��̾ �ֺ����� �ٰ��ִٸ�
            if(playerController.GetRun())
            {
                if(CalcPathLength(playerController.transform.position) <= viewDistance)
                {
                    Debug.Log("������ �ֺ����� �ٰ��մ� �÷��̾ �ľ�");
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

        //��������Ʈ ó�� �κ�
        wayPoint[0] = transform.position;
        //��������Ʈ ������ �κ�
        wayPoint[path.corners.Length + 1] = targetPos;

        float pathLength = 0;
        for (int i = 0; i < path.corners.Length; i++)
        {
            //���� ����Ʈ�� ��θ� ����
            wayPoint[i + 1] = path.corners[i];
            //�÷��̾�� ������ �Ÿ� (��ֹ��� ������ ��ֹ��� ���ϴ� ª���Ÿ��� ���)
            pathLength += Vector3.Distance(wayPoint[i], wayPoint[i + 1]);
        }
        return pathLength;
    }
}
