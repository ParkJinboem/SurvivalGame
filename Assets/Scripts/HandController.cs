using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandController : CloseWeaponController
{

    //활성화 여부
    public static bool isActiviate = false;

    [SerializeField] QuickSlotcontroller quickSlotcontroller;

    void Update()
    {
        if (isActiviate && !Inventory.inventoryActivated)
        {
            if(QuickSlotcontroller.handItem == null)
            {
                TryAttack();
            }
            else
            {
                TryEating();
            }
        }
    }

    private void TryEating()
    {
        if(Input.GetButtonDown("Fire1") && !quickSlotcontroller.GetIsCoolTime())
        {
            currentCloseWeapon.anim.SetTrigger("Eat");
            quickSlotcontroller.EatItme();
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
