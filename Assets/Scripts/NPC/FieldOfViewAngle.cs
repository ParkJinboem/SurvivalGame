using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    //시야갹_120도 
    [SerializeField] private float viewAngle;
    //시야거리_10미터
    [SerializeField] private float viewDistance;
    //타겟마스크_플레이어 
    [SerializeField] private LayerMask targetMask;

    private Pig pig;

    private void Start()
    {
        pig = GetComponent<Pig>();
    }
    void Update()
    {
        View();
    }
    private Vector3 BoundaryAngle(float angle)
    {
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    private void View()
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
                            Debug.Log("플레이어가 돼지 시야내 있습니다.");
                            Debug.DrawRay(transform.position + transform.up, direction, Color.blue);
                            pig.Run(hit.transform.position);
                        }
                    }
                }
            }
        }
    }
}
