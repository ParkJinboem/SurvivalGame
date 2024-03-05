using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    //원래 위치
    private Vector3 originPos;
    //현재 위치
    private Vector3 currentPos;

    //sway 한계
    [SerializeField] private Vector3 limitPos;
    //정조준 sway 한계
    [SerializeField] private Vector3 fineSightLimitPos;
    //부드러운 움직임 정도
    [SerializeField] private Vector3 smoothSway;


    [SerializeField] private GunController gunController;
    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Inventory.inventoryActivated)
        {
            TrySway();
        }
    }

    private void TrySway()
    {
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
        {
            Swaying();
        }
        else
        {
            BackToOriginPos();
        }
    }

    private void Swaying()
    {
        float moveX = Input.GetAxisRaw("Mouse X");
        float moveY = Input.GetAxisRaw("Mouse Y");

        if (!gunController.isFineSightMode)
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -moveX, smoothSway.x), -limitPos.x, limitPos.x),
                       Mathf.Clamp(Mathf.Lerp(currentPos.y, -moveY, smoothSway.x), -limitPos.y, limitPos.y),
                       originPos.z);
        }
        else
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -moveX, smoothSway.y), -fineSightLimitPos.x, fineSightLimitPos.x),
                       Mathf.Clamp(Mathf.Lerp(currentPos.y, -moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
                       originPos.z);
        }

        transform.localPosition = currentPos;
    }

    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;
    }
}