using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandController : CloseWeaponController
{

    //활성화 여부
    public static bool isActiviate = false;
    //설치하려는 킷
    public static Item currentKit;

    private bool isPreview = false;
    //설치할 키트 프리뷰
    private GameObject kitPreviewObj;
    //설치할 키트 위치
    private Vector3 previewPos;
    //건축시 추가 사정거리
    [SerializeField] private float rangeAdd;
    [SerializeField] private LayerMask previewKitLayerMask;


    [SerializeField] QuickSlotcontroller quickSlotcontroller;

    void Update()
    {
        if (isActiviate && !Inventory.inventoryActivated)
        {
            if(currentKit == null)
            {
                if (QuickSlotcontroller.handItem == null)
                {
                    TryAttack();
                }
                else
                {
                    TryEating();
                }
            }
            else
            {
                if(!isPreview)
                {
                    InstallPreviewKit();
                }
                PreviewPositionUpdate();
                Build();
            }   
        }
    }

    private void InstallPreviewKit()
    {
        isPreview = true;
        kitPreviewObj = Instantiate(currentKit.kitPreviewPrefab, transform.position, Quaternion.identity);
    }

    private void PreviewPositionUpdate()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range + rangeAdd, previewKitLayerMask))
        {
            previewPos = hitInfo.point;
            kitPreviewObj.transform.position = previewPos;
        }
    }

    private void Build()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if(kitPreviewObj.GetComponent<PreviewObject>().isBuildable())
            {
                quickSlotcontroller.DecreaseSelectedItem(); //슬롯 아이템 개수 -1
                GameObject temp = Instantiate(currentKit.kitPrefab, previewPos, Quaternion.identity);
                temp.name = currentKit.itemName;
                Destroy(kitPreviewObj);
                currentKit = null;
                isPreview = false;
            }
        }
    }

    public void Cancel()
    {
        Destroy(kitPreviewObj);
        currentKit = null;
        isPreview = false;
    }
    private void TryEating()
    {
        if(Input.GetButtonDown("Fire1") && !quickSlotcontroller.GetIsCoolTime())
        {
            currentCloseWeapon.anim.SetTrigger("Eat");
            quickSlotcontroller.DecreaseSelectedItem();
        }
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                //충돌 했음
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            else
            {

            }
            yield return null;
        }
    }
    public override void CloseWeaponChange(CloseWeapon closeWeapon)
    {
        base.CloseWeaponChange(closeWeapon);
        isActiviate = true;
    }
}
