using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandController : CloseWeaponController
{

    //Ȱ��ȭ ����
    public static bool isActiviate = false;
    //��ġ�Ϸ��� Ŷ
    public static Item currentKit;

    private bool isPreview = false;
    //��ġ�� ŰƮ ������
    private GameObject kitPreviewObj;
    //��ġ�� ŰƮ ��ġ
    private Vector3 previewPos;
    //����� �߰� �����Ÿ�
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
                quickSlotcontroller.DecreaseSelectedItem(); //���� ������ ���� -1
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
                //�浹 ����
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
