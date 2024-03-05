using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour
{
    //현재 장차괸 Hand형 타입 무기
    [SerializeField] protected CloseWeapon currentCloseWeapon;

    //공격중
    protected bool isAttack = false;
    protected bool isSwing = false;
    protected RaycastHit hitInfo;
    [SerializeField] protected LayerMask layerMask;
    private PlayerController playercontroller;

    private void Start()
    {
        playercontroller = FindObjectOfType<PlayerController>();
    }

    protected void TryAttack()
    {
        if (!Inventory.inventoryActivated)
        {
            if (Input.GetButton("Fire1"))
            {
                if (!isAttack)
                {
                    if (CheckObject())
                    {
                        if (currentCloseWeapon.isAxe && hitInfo.transform.tag == "Tree")
                        {
                            StartCoroutine(playercontroller.TreeLookCoroutine(hitInfo.transform.GetComponent<TreeComponent>().GetTreeCenterPosition()));
                            StartCoroutine(AttackCoroutin("Chop", currentCloseWeapon.workDelayA, currentCloseWeapon.workDelayB, currentCloseWeapon.workDelay));
                            return;
                        }
                    }
                    StartCoroutine(AttackCoroutin("Attack", currentCloseWeapon.attackDelayA, currentCloseWeapon.attackDelayB, currentCloseWeapon.attackDelay));
                }
            }
        }
    }

    protected IEnumerator AttackCoroutin(string swingType, float delayA, float delayB, float delayC)
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger(swingType);

        yield return new WaitForSeconds(delayA);
        isSwing = true;
        StartCoroutine(HitCoroutine());
        //공격 활성화 시점
        yield return new WaitForSeconds(delayB);
        isSwing = false;
        yield return new WaitForSeconds(delayC- delayA - delayB);
        isAttack = false;
    }

    //추상 코루틴
    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range, layerMask))
        {
            return true;
        }
        return false;
    }

    public virtual void CloseWeaponChange(CloseWeapon closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        currentCloseWeapon = closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
    }
}
