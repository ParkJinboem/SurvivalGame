using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    //활성화 여부
    public static bool isActiviate = true;

    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }

    void Update()
    {
        if (isActiviate)
        {
            TryAttack();
        }
    }
    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                //충돌 했음
                if(hitInfo.transform.tag == "Rock")
                {
                    hitInfo.transform.GetComponent<Rock>().Mining();
                }
                else if (hitInfo.transform.tag == "Twig")
                {
                    hitInfo.transform.GetComponent<Twig>().Damage(this.transform);
                }
                else if (hitInfo.transform.tag == "WeakAnimal")
                {
                    SoundManager.instance.PlaySE("AnimalHit");
                    hitInfo.transform.GetComponent<WeakAnimal>().Damage(1, transform.position);
                }
                else if (hitInfo.transform.tag == "StrongAnimal")
                {
                    SoundManager.instance.PlaySE("AnimalHit");
                    hitInfo.transform.GetComponent<StrongAnimal>().Damage(1, transform.position);
                }
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